using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Task.ModelObjects;

namespace Task
{
    public class TaskActivity : ModelObject
    {
        public TaskActivity()
        {
            ActivityType = string.Empty;
            Description = string.Empty;
            User = new TaskUser();
            Task = new TaskTask();
        }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskActivity>()
                .HasOne(a => a.Task)
                .WithMany(t => t.Activities)
                .HasForeignKey(a => a.TaskGu)
                .HasPrincipalKey(t => t.TaskGu)
                .OnDelete(DeleteBehavior.Restrict);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ActivityGu { get; set; }

        [Required]
        public Guid TaskGu { get; set; }

        [Required]
        [ForeignKey(nameof(TaskGu))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public TaskTask Task { get; set; }

        [Required]
        public Guid UserGu { get; set; }

        [Required]
        [ForeignKey(nameof(UserGu))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public TaskUser User { get; set; }

        [Required]
        [StringLength(5)]
        public string ActivityType { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
    }
}
