using System;

// workaround in C# 9 preview
// https://stackoverflow.com/questions/62648189/testing-c-sharp-9-0-in-vs2019-cs0518-isexternalinit-is-not-defined-or-imported
// https://github.com/dotnet/roslyn/issues/45510
namespace System.Runtime.CompilerServices
{
    public class IsExternalInit { }
}


namespace DependencyTree
{
    public record VersionConstraint : ISatisfiable<Version>
    {
        private Version? minimumVersion;
        private Version? maximumVersion;
        private Version? requiredVersion;
        private bool minIsExclusive = false;
        private bool maxIsExclusive = false;

        public Version? MinimumVersion
        {
            get => minimumVersion;
            init
            {
                minimumVersion = value;
                normalise();
            }
        }
        public Version? MaximumVersion
        {
            get => maximumVersion;
            init
            {
                maximumVersion = value;
                normalise();
            }
        }
        public Version? RequiredVersion
        {
            get => requiredVersion;
            init
            {
                requiredVersion = value;
                normalise();
            }
        }
        public bool MinimumVersionIsExclusive
        {
            get => minIsExclusive;
            init
            {
                minIsExclusive = value;
                normalise();
            }
        }
        public bool MaximumVersionIsExclusive
        {
            get => maxIsExclusive;
            init
            {
                maxIsExclusive = value;
                normalise();
            }
        }


        private void normalise()
        {
            if ((minimumVersion is not null || maximumVersion is not null) && requiredVersion is not null)
            {
                throw new ArgumentException($"Cannot have strict and range version constraints.");
            }

            if (minimumVersion is not null && maximumVersion is not null && minimumVersion > maximumVersion)
            {
                throw new ArgumentException($"{nameof(minimumVersion)} cannot be greater than {nameof(maximumVersion)}.");
            }

            if (minimumVersion is not null && minimumVersion == maximumVersion)
            {
                requiredVersion = minimumVersion;
                minimumVersion = null;
                maximumVersion = null;
            }

            if (requiredVersion is not null && (minIsExclusive || maxIsExclusive))
            {
                throw new ArgumentException($"Cannot have exclusive version constraints with strict version constraint.");
            }
        }


        public VersionConstraint() { }

        public VersionConstraint(Version requiredVersion) => this.requiredVersion = requiredVersion;

        public VersionConstraint(Version? minimumVersion, Version? maximumVersion, bool minIsExclusive = false, bool maxIsExclusive = false)
        {
            this.minimumVersion = minimumVersion;
            this.maximumVersion = maximumVersion;
            this.minIsExclusive = minIsExclusive;
            this.maxIsExclusive = maxIsExclusive;
            normalise();
        }


        public bool IsSatisfiedBy(Version? other)
        {
            if (other is null) return this switch
            {
                { requiredVersion: null, minimumVersion: null, maximumVersion: null } => true,
                _ => false
            };

            return this switch
            {
                { requiredVersion: not null } => requiredVersion == other,
                { minimumVersion: not null, maximumVersion: not null } => minimumVersion <= other && maximumVersion >= other,
                { maximumVersion: not null } => maximumVersion >= other,
                { minimumVersion: not null } => minimumVersion <= other,
                _ => true
            };
        }


        public override string ToString()
        {
            string verString(Version version, bool withEquals) => withEquals ? $"={version}" : $"{version}";

            return this switch
            {
                { requiredVersion: not null } => verString(requiredVersion, true),
                { minimumVersion: not null, maximumVersion: not null } => String.Format(
                    ">{0} <{1}",
                    verString(minimumVersion, !minIsExclusive),
                    verString(maximumVersion, !maxIsExclusive)
                ),
                { minimumVersion: not null } => $">{verString(minimumVersion, !minIsExclusive)}",
                { maximumVersion: not null } => $"<{verString(maximumVersion, !maxIsExclusive)}",
                _ => "*"
            };
        }
    }
}