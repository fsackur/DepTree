using System;
using System.Management.Automation;
using System.Collections.Generic;

namespace DependencyTree
{
    public class Walker
    {
        public IList<Dependency> Walk(Dependency dependency)
        {
            var list = new List<Dependency>();
            list.Add(dependency);

            foreach (var subdep in dependency.Requires)
            {
                // Depth-first
                list.AddRange(Walk(subdep));
            }

            return list;
        }
    }
}
