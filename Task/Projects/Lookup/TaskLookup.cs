using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task.Lookup
{
    public class TaskLookup
    {
        //Define some product owned lookups
        public const string LOOKUPTABLE_ROLE = "c02def0b-9e62-4620-a981-00915bea53d9";

        public TaskLookup()
        {
            Name = string.Empty;
            Locked = false;
        }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskLookup>() //Setup FK relationship
                .HasMany(e => e.TaskLookupValues)
                .WithOne(e => e.TaskLookup)
                .HasForeignKey(e => e.LookupGu)
                .HasPrincipalKey(e => e.LookupGu);

            modelBuilder.Entity<TaskLookup>().HasData( // Create default roles lookup
                new TaskLookup()
                {
                    LookupGu = Guid.Parse(LOOKUPTABLE_ROLE),
                    Name = "Task.Roles",
                    Locked = true,
                    AddTimestamp = DateTime.Now
                });

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid LookupGu {  get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public bool Locked { get; set; }

        public DateTime? AddTimestamp { get; set; }

        public DateTime? UpdateTimestamp { get; set; }

        public ICollection<TaskLookupValue> TaskLookupValues { get; set; }
    }

    public class TaskLookupValue
    {
        public TaskLookupValue()
        {
            Code = string.Empty;
            Value = string.Empty;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid LookupValueGu { get; set; }

        [Required]
        [StringLength(5)]
        public string Code { get; set; }

        [Required]
        [StringLength(50)]
        public string Value { get; set; }

        public DateTime? AddTimestamp { get; set; }
        public DateTime? UpdateTimestamp { get; set; }

        public Guid LookupGu { get; set; }
        [Required]
        public TaskLookup TaskLookup;  
    }
}
