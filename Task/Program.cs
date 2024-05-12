using Microsoft.EntityFrameworkCore;
using Task.Components;

namespace Task
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string? connectionString;

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddEnvironmentVariables();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            connectionString = builder.Configuration["TASK_CONNECTIONSTRING"];

            if(connectionString == null || connectionString == string.Empty) 
            {
                throw new Exception("Database Connection String is not defined");
            }

            builder.Services.AddDbContextFactory<Context>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<Session>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
            
            app.Run();
        }
    }
}
