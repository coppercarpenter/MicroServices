using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models
{
    public class Platform
    {
        public Platform()
        {
            Commands = new HashSet<Command>();
        }

        [Key]
        [Required]
        public long Id { get; set; }

        [Required]
        public long External_Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Command> Commands { get; set; }
    }
}