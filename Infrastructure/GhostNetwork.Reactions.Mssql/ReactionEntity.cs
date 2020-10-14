using System;
using System.ComponentModel.DataAnnotations;

namespace GhostNetwork.Reactions.Mssql
{
    public class ReactionEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
