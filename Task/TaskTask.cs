using Task.ModelObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Task
{
    public class TaskTask : ModelObject
    {
        public TaskTask()
        {
            Title = string.Empty;
            Description = string.Empty;
            Status = string.Empty;
            Opener = new TaskUser();
        }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskTask>()
                .HasOne(t => t.Opener)
                .WithMany(u => u.OpenedTasks)
                .HasForeignKey(t => t.OpenerGu)
                .HasPrincipalKey(u => u.UserGu)
                .OnDelete(DeleteBehavior.Restrict);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TaskGu { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [StringLength(5)]
        public string Status { get; set; }

        public Guid OpenerGu { get; set; }

        [Required]
        [ForeignKey(nameof(OpenerGu))]
        public TaskUser Opener { get; set; }

        public ICollection<TaskActivity>? Activities { get; set; }
    }
}
