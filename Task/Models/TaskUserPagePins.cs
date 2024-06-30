using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Task.ModelObjects;

namespace Task.Models
{
    public class TaskUserPagePins : ModelObject
    {
        public TaskUserPagePins()
        {
            User = new TaskUser();
            PagePath = string.Empty;
            PageName = string.Empty;
        }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskUserPagePins>()
               .HasOne(p => p.User)
               .WithMany(u => u.UserPagePins)
               .HasForeignKey(p => p.UserGu)
               .HasPrincipalKey(u => u.UserGu);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserPagePinGu { get; set; }

        [Required]
        public Guid UserGu { get; set; }

        [Required]
        [ForeignKey(nameof(UserGu))]
        public TaskUser User { get; set; }

        [Required]
        [StringLength(20)]
        public string PagePath { get; set; }

        [Required]
        [StringLength(20)]
        public string PageName { get; set; }
    }
}
