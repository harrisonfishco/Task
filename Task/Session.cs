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

        public async Task<Guid> GetSessionId()
        {
            ProtectedBrowserStorageResult<Guid> res = await ProtectedBrowserStorage.GetAsync<Guid>("session");

            return res.Value;
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
