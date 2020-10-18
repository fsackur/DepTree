using System;
using System.Management.Automation;
using System.Collections.Generic;

namespace DependencyTree
{
    public class Dependency : IComparable<Dependency>, IComparable
    {
        private Dependency? requiredBy;
        private IList<Dependency> requires;
        private string name;
        private VersionConstraint versionConstraint;


        public Dependency(string name, VersionConstraint versionConstraint)
        {
            this.name = name;
            this.requires = new List<Dependency>();
            this.versionConstraint = versionConstraint;
        }


        public int CompareTo(Dependency other)
        {
            if (other is null) { throw new ArgumentNullException(nameof(other)); }

            return this.ToString().CompareTo(other.ToString());
        }


        public int CompareTo(object other)
        {
            return other switch
            {
                Dependency => CompareTo((Dependency)other),
                null => throw new ArgumentNullException(nameof(other)),
                _ => throw new ArgumentException($"Cannot compare object of type {other.GetType()}", nameof(other))
            };
        }


        public override string ToString() => $"{name} {versionConstraint}";
    }
}
