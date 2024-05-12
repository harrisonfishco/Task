﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Task.Login;

namespace Task
{
    public class Context : DbContext
    { 

        public DbSet<TaskUser> TaskUsers { get; set; }
        public DbSet<TaskUserSession> TaskUserSessions { get; set; }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskUser>()
                .HasAlternateKey(e => e.Username);
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
        public TaskUser()
        {
            Username = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserGU { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        public DateTime? AddTimestamp { get; set; }
        
        public DateTime? UpdateTimestamp { get; set; }
    }

    public class TaskUserSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserSessionGU { get; set; }
        [Required]
        public Guid UserGU { get; set; }
        public DateTime? Expires { get; set; }
        public DateTime? AddTimestamp { get; set; }
        public DateTime? UpdateTimestamp { get; set; }

        public TaskUser? TaskUser { get; set; }
    }
}
