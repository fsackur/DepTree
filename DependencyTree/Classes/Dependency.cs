using System;
using System.Management.Automation;
using System.Collections.Generic;

namespace DependencyTree
{
    public class Dependency : IComparable<Dependency>, IComparable, ISatisfiable<Dependency>, ISatisfiable
    {
        public Dependency(string name)
        {
            this.name = name;
            this.requires = new List<Dependency>();
        }

        public Dependency(string name, Version requiredVersion) : this(name) => this.requiredVersion = requiredVersion;

        public Dependency(string name, Version? minimumVersion, Version? maximumVersion) : this(name)
        {
            if (minimumVersion == maximumVersion)
            {
                this.requiredVersion = minimumVersion;
            }
            else
            {
                this.minimumVersion = minimumVersion;
                this.maximumVersion = maximumVersion;
            }
        }

        private Dependency? requiredBy;
        private IList<Dependency> requires;
        private string name;

        private Version? resolvedVersion;    // Version of the discovered object that satisfies the requirement
        private Version? minimumVersion;
        private Version? maximumVersion;
        private Version? requiredVersion;


        private object? satisfiedBy;

        public bool IsSatisfiedBy(Dependency other)
        {
            if (name != other.name) return false;

            return other switch
            {
                { resolvedVersion: not null } => IsSatisfiedBy(other.resolvedVersion),
                { resolvedVersion: null } => minimumVersion is null && maximumVersion is null && requiredVersion is null,
                null => throw new ArgumentNullException(nameof(other)),
            };
        }

        public bool IsSatisfiedBy(Version other)
        {
            if (other is null) { throw new ArgumentNullException(nameof(other)); }

            return this switch
            {
                { requiredVersion: not null } => requiredVersion == other,
                { minimumVersion: not null, maximumVersion: not null } => minimumVersion <= other && maximumVersion >= other,
                { maximumVersion: not null } => maximumVersion >= other,
                { minimumVersion: not null } => minimumVersion <= other,
                _ => true
            };
        }

        public bool IsSatisfiedBy(object other)
        {
            return other switch
            {
                Dependency => IsSatisfiedBy((Dependency)other),
                Version => IsSatisfiedBy((Version)other),
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


        public string VersionString() =>
            this switch
            {
                { requiredVersion: not null } => requiredVersion.ToString(),
                { minimumVersion: not null, maximumVersion: not null } => $">={minimumVersion} <={maximumVersion}",
                { maximumVersion: not null } => $"<={maximumVersion}",
                { minimumVersion: not null } => $">={minimumVersion}",
                _ => "*"
            };

        public override string ToString()
        {
            return $"{name} {VersionString()}";
        }
    }
}
