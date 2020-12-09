using System;
using System.Collections.Generic;
using System.Text;

namespace GhostNetwork.Reactions
{
    public class Reaction
    {
        public Reaction(string type)
        {
            Type = type;
        }

        public string Type { get; set; }
    }
}
