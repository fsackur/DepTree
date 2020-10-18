using System;
using System.Management.Automation;
using System.Collections.Generic;

namespace DependencyTree
{
    public class Dependency : IComparable<Version>
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

        public int CompareTo(Version other)
        {
            if (other is null) { throw new ArgumentNullException(nameof(other)); }

            // other is higher than this => -1
            // other is lower => 1
            // this's version reqs are satisfied => 0
            // this has no version attached => 0
            return this switch
            {
                { requiredVersion: not null } => requiredVersion.CompareTo(other),
                { minimumVersion: not null, maximumVersion: not null } => (
                    Math.Max(0, maximumVersion.CompareTo(other)) +      // -1 or 0
                    Math.Min(0, minimumVersion.CompareTo(other))        // 0 or 1
                ),
                { maximumVersion: not null } => Math.Max(0, maximumVersion.CompareTo(other)),   // -1 or 0
                { minimumVersion: not null } => Math.Min(0, minimumVersion.CompareTo(other)),   // 0 or 1
                _ => 0
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
                { minimumVersion: not null, maximumVersion: not null } => $"{minimumVersion}-{maximumVersion}",
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
