using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GhostNetwork.Reactions.MSsql
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
