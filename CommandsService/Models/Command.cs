using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models
{
    public class Command
    {
        [Key]
        [Required]
        public long Id { get; set; }

        [Required]
        public string HowTo { get; set; }

        [Required]
        public string CommandLine { get; set; }

        [Required]
        public long Platform_Id { get; set; }

        public virtual Platform Platform { get; set; }
    }
}