using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Task
{
    public class Session
    {
        public readonly ProtectedLocalStorage ProtectedBrowserStorage;

        public Session(ProtectedLocalStorage ProtectedBrowserStorage)
        {
            this.ProtectedBrowserStorage = ProtectedBrowserStorage;
        }

        public async Task<Guid> GetSessionId(NavigationManager nav)
        {
            ProtectedBrowserStorageResult<Guid> res;
            try
            {
                res = await ProtectedBrowserStorage.GetAsync<Guid>("session");
                return res.Value;
            }
            catch (Exception ex)
            {
                await ProtectedBrowserStorage.DeleteAsync("session");
                return Guid.Empty;
            }
        }

        public async System.Threading.Tasks.Task SetSessionId(Guid sessionId)
        {
            await ProtectedBrowserStorage.SetAsync("session", sessionId);
        }

        public async System.Threading.Tasks.Task DeleteSession()
        {
            await ProtectedBrowserStorage.DeleteAsync("session");
        }
    }
}
