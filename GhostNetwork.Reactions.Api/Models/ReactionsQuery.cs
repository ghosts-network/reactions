using System;
using System.Collections.Generic;

namespace GhostNetwork.Reactions.Api.Models
{
    public class ReactionsQuery
    {
        [ObsoleteAttribute("This property is obsolete. Use 'Keys' property instead.", false)]
        public IEnumerable<string> PublicationIds { get; set; }
        public IEnumerable<string> Keys { get; set; }
    }
}