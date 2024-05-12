using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task
{
    public class Context : DbContext
    { 

        public DbSet<TaskUser> TaskUsers { get; set; }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {

        }
    }

    public class TaskUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserGU { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        public DateTime? AddTimestamp { get; set; }
        
        public DateTime? UpdateTimestamp { get; set; }
    }

}
