using System;
using System.Management.Automation;
using System.Collections.Generic;

namespace DependencyTree
{
    public class Dependency
    {
        public Dependency (string name) => this.name = name;
        public Dependency (string name, Version requiredVersion) : this(name) => this.requiredVersion = requiredVersion;
        public Dependency (string name, Version minimumVersion, Version maximumVersion) : this(name)
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

        private Dependency requiredBy;
        private List<Dependency> requires;
        private string name;

        private Version resolvedVersion;    // Version of the discovered object that satisfies the requirement
        private Version minimumVersion;
        private Version maximumVersion;
        private Version requiredVersion;

        private object satisfiedBy;

        public string VersionString()
        {
            if (requiredVersion is not null)
            {
                return requiredVersion.ToString();
            }
            else if (minimumVersion is not null and maximumVersion is null)
            {
                return $">={minimumVersion}";
            }
            else if (minimumVersion is null and maximumVersion is not null)
            {
                return $"<={maximumVersion}";
            }
            else if (minimumVersion is not null and maximumVersion is not null)
            {
                return $"{minimumVersion}-{maximumVersion}";
            }
            else
            {
                return "*";
            }
        }

        public override string ToString()
        {
            return name;
        }
    }
}
