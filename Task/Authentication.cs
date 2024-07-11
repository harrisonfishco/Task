using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;

namespace Task
{
    public class Authentication
    {
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
    }
}
