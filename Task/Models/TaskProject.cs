using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Task.ModelObjects;

namespace Task.Models
{
    public class TaskProject : ModelObject
    {
        public TaskProject()
        {
            Owner = new TaskUser();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProjectGu { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Guid OwnerGu { get; set; }

        [Required]
        [ForeignKey(nameof(OwnerGu))]
        public TaskUser Owner { get; set; }

        public ICollection<TaskTask>? Tasks { get; set; }
    }
}
