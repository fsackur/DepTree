using System;
using System.Management.Automation;
using System.Collections.Generic;

namespace DependencyTree
{
    abstract public class Dependency : IComparable<Dependency>, IComparable
    {
        public Dependency? RequiredBy { get; init; }
        public IList<Dependency> Requires { get; init; }
        public string Name { get; init; }
        public VersionConstraint VersionConstraint { get; init; }
        public virtual bool IsResolved { get => false; }


        public Dependency(string name, VersionConstraint versionConstraint)
        {
            this.Name = name;
            this.Requires = new List<Dependency>();
            this.VersionConstraint = versionConstraint;
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


        public override string ToString() => $"{Name} {VersionConstraint}";
    }
}
