using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLiquid
{
    public class Node
    {
        public string Name { get; set; }

        public Dictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, List<Node>> Nodes { get; set; } = new Dictionary<string, List<Node>>();

        public string Value { get; set; }

    }
}
