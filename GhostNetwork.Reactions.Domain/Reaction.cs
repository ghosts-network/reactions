using System;
using System.Collections.Generic;
using System.Text;

namespace GhostNetwork.Reactions
{
    public class Reaction
    {
        public Reaction(string key, string type)
        {
            Key = key;
            Type = type;
        }

        public string Key { get; set; }

        public string Type { get; set; }
    }
}
