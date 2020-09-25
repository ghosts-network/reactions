using System;
using System.Collections.Generic;
using System.Text;

namespace GhostNetwork.Reactions.DAL
{
    public class ReactionEntity
    {
        public ReactionEntity(string author, string type)
        {
            Author = author;
            Type = type;
        }

        public string Author { get; set; }
        public string Type { get; set; }
    }
}
