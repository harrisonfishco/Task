using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Task
{
    public class Authentication
    {
        private static readonly Dictionary<Guid, TaskUser> UserDictionary = new Dictionary<Guid, TaskUser>();

        public async Task<TaskUser> GetCurrentUserAsync(Session session)
        {
            UserDictionary.TryGetValue(await session.GetSessionId(), out TaskUser? user);
            return user;
        }

        public async void IsAuthenticated(NavigationManager navigationManager, Session session, IDbContextFactory<Context> ContextFactory)
        {
            Guid sessionId = await session.GetSessionId();

            using (Context ctx = ContextFactory.CreateDbContext())
            {
                TaskUserSession? sess = await ctx.TaskUserSessions.Where(s => s.UserSessionGu == sessionId).FirstOrDefaultAsync();

                if (TypeCheck.NotEmpty(sess))
                {

                    navigationManager.NavigateTo("/");
                }
            }
        }
        public async void Authenticate(NavigationManager navigationManager, Session session, IDbContextFactory<Context> ContextFactory, String username, String password, String status)
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

                    UserDictionary[await session.GetSessionId()] = user;

                    navigationManager.NavigateTo("/");
                }
                status = canLogin ? "Success" : "Error";
            }
        }

    }
}
