using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;
using Task.Components.Pages;

namespace Task
{
    public class Authentication
    {
        public readonly string TASK_LDAP_SETTINGS_KEY = "TASK_LDAP_SETTINGS_KEY";

        private string LdapSettingsFilePath
        {
            get
            {
                string res = string.Empty;
                try
                {
                    res = Path.Combine(Directory.GetCurrentDirectory(), "ldapSettings.json");
#if DEBUG
                    res = Path.Combine(Directory.GetCurrentDirectory(), @"bin/Debug/net8.0", "ldapSettings.json");
#endif
                } catch(Exception ex) { TaskError.HandleError(ex); }
                return res;
            }
        }

        private static readonly ConcurrentDictionary<Guid, TaskUser> UserDictionary = new ConcurrentDictionary<Guid, TaskUser>();

        public async Task<TaskUser?> GetCurrentUser(Session session, NavigationManager navigationManager)
        {
            UserDictionary.TryGetValue(await session.GetSessionId(navigationManager), out TaskUser? user);
            return user;
        }

        public async Task<bool> IsAuthenticated(NavigationManager navigationManager, Session session, IDbContextFactory<Context> ContextFactory)
        {
            Guid sessionId = await session.GetSessionId(navigationManager);

            using (Context ctx = ContextFactory.CreateDbContext())
            {
                TaskUserSession? sess = await ctx.TaskUserSessions.Where(s => s.UserSessionGu == sessionId).FirstOrDefaultAsync();

                return TypeCheck.NotEmpty(sess);
            }
        }
        public async void Authenticate(NavigationManager navigationManager, Session session, IDbContextFactory<Context> ContextFactory, string username, string password, string status)
        {
            using (Context ctx = ContextFactory.CreateDbContext())
            {
                bool canLogin = await ctx.VerifyUser(username, password);

                if (canLogin)
                {
                    TaskUser user = await ctx.TaskUsers.FirstAsync(u => u.Username == username);

                    TaskCache.SetKey("TASKUSER_ROLE", user.Role);

                    TaskUserSession tus = new TaskUserSession();
                    tus.TaskUser = user;
                    tus.UserGu = user.UserGu;
                    tus.AddTimestamp = DateTime.Now;

                    await ctx.TaskUserSessions.AddAsync(tus);
                    await ctx.SaveChangesAsync();

                    Guid sessionId = tus.UserSessionGu;

                    await session.SetSessionId(sessionId);

                    UserDictionary[await session.GetSessionId(navigationManager)] = user;

                    navigationManager.NavigateTo("/");
                }
                status = canLogin ? "Success" : "Error";
            }
        }

        public async void SaveLdapSettings(LdapSettings settings)
        {
            try
            {
                string json = JsonSerializer.Serialize(settings);

                TaskCache.SetKey(TASK_LDAP_SETTINGS_KEY, json);

                await File.WriteAllTextAsync(LdapSettingsFilePath, json);

            } catch(Exception ex) { TaskError.HandleError(ex); }
        }

        public async Task<LdapSettings> ReadLdapSettings()
        {
            string json;
            LdapSettings? res = null;

            try
            {
                if(TaskCache.ContainsKey(TASK_LDAP_SETTINGS_KEY))
                {
                    json = TaskCache.GetKey(TASK_LDAP_SETTINGS_KEY)!;
                }
                else
                {
                    if(!File.Exists(LdapSettingsFilePath))
                    {
                        await CreateDefaultLdapSettings();
                    }

                    json = await File.ReadAllTextAsync(LdapSettingsFilePath);

                    TaskCache.SetKey(TASK_LDAP_SETTINGS_KEY, json);

                    res = JsonSerializer.Deserialize<LdapSettings>(json);
                    if(TypeCheck.Empty(res))
                    {
                        TaskError.CreateUserError("Ldap Settings Missing");
                    }
                }
            } catch(Exception ex) { TaskError.HandleError(ex); }

            return res!;
        }

        private async System.Threading.Tasks.Task CreateDefaultLdapSettings(LdapSettings? settings = null)
        {
            try
            {
                if(TypeCheck.Empty(settings))
                {
                    settings = new LdapSettings();
                }

                string json = JsonSerializer.Serialize(settings);

                await File.WriteAllTextAsync(LdapSettingsFilePath, json);
            } catch(Exception ex) { TaskError.HandleError(ex); }
        }
    }
}
