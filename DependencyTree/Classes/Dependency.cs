using System;
using System.Management.Automation;
using System.Collections.Generic;

namespace DependencyTree
{
    public class Dependency : IComparable<Dependency>, IComparable, ISatisfiable<Dependency>, ISatisfiable
    {
        private Dependency? requiredBy;
        private IList<Dependency> requires;
        private string name;

        private Version? resolvedVersion;   // Version of the discovered object that satisfies the requirement
        private object? satisfiedBy;        // the discovered object that satisfies the dependency
        private VersionConstraint versionConstraint;


        public Dependency(string name, VersionConstraint versionConstraint)
        {
            this.name = name;
            this.requires = new List<Dependency>();
            this.versionConstraint = versionConstraint;
        }


        public bool IsSatisfiedBy(Dependency other)
        {
            if (name != other.name) return false;

            return other switch
            {
                { resolvedVersion: not null } => IsSatisfiedBy(other.resolvedVersion),
                { resolvedVersion: null } => versionConstraint.MinimumVersion is null && versionConstraint.MaximumVersion is null && versionConstraint.RequiredVersion is null,
                null => throw new ArgumentNullException(nameof(other)),
            };
        }


        public bool IsSatisfiedBy(object other)
        {
            return other switch
            {
                Dependency => IsSatisfiedBy((Dependency)other),
                Version => versionConstraint.IsSatisfiedBy((Version)other),
                null => throw new ArgumentNullException(nameof(other)),
                _ => throw new ArgumentException($"Cannot satisfy a dependency with object of type {other.GetType()}", nameof(other))
            };
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

        public Dependency? RequiredBy { get; set; }

        public void AddRequirement(Dependency subdep)
        {
            subdep.RequiredBy = this;
            requires.Add(subdep);
        }


        public override string ToString() => $"{name} {versionConstraint}";
    }
}
