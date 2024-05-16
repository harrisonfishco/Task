using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Task.Login;
using Task.Lookup;

namespace Task
{
    public class Context : DbContext
    { 

        public DbSet<TaskUser> TaskUsers { get; set; }
        public DbSet<TaskUserSession> TaskUserSessions { get; set; }
        public DbSet<TaskLookup> TaskLookups { get; set; }
        public DbSet<TaskLookupValue> TaskLookupValues { get; set; }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskUser>()
                .HasAlternateKey(e => e.Username);

            modelBuilder.Entity<TaskUser>()
                .HasData(new TaskUser
                {
                    UserGu = Guid.NewGuid(),
                    Username = "admin",
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@webadmin.com",
                    Password = "$2a$10$XsoM977PKMnbd1zzIEpJH.UoHjKb5VuFqF/Sq1es/xtYyijshEItG", //password
                    AddTimestamp = DateTime.Now,
                    Role = "0" //Admin
                });

            TaskLookup.OnModelCreating(modelBuilder);
        }

        public async Task<bool> VerifyUser(string username, string password)
        {
            bool res = false;

            TaskUser? user = await this.TaskUsers.FirstOrDefaultAsync(u => u.Username == username);

            if (user != null)
            {
                res = PasswordHelper.VerifyPassword(password, user.Password);
            }

            return res;
        }
    }

    public class TaskUser
    {
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
        public TaskUser()
        {
            Username = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Role = string.Empty;
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserGu { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(5)]
        public string Role { get; set; }

        public DateTime? AddTimestamp { get; set; }
        
        public DateTime? UpdateTimestamp { get; set; }
    }

    public class TaskUserSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserSessionGu { get; set; }
        public DateTime? Expires { get; set; }
        public DateTime? AddTimestamp { get; set; }
        public DateTime? UpdateTimestamp { get; set; }

        [Required]
        public Guid UserGu { get; set; }

        [Required]
        public TaskUser? TaskUser { get; set; }
    }
}
