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
    public record VersionConstraint
    {
        private Version? minimumVersion;
        private Version? maximumVersion;
        private Version? requiredVersion;
        private bool minIsExclusive = false;
        private bool maxIsExclusive = false;

        public VersionConstraint() { }

        public VersionConstraint(Version requiredVersion) => this.requiredVersion = requiredVersion;

        public VersionConstraint(Version? minimumVersion, Version? maximumVersion, bool minIsExclusive = false, bool maxIsExclusive = false)
        {
            this.minIsExclusive = minIsExclusive;
            this.maxIsExclusive = maxIsExclusive;

            if (minimumVersion == null || maximumVersion == null)
            {
                this.minimumVersion = minimumVersion;
                this.maximumVersion = maximumVersion;
                return;
            }

            if (minimumVersion > maximumVersion)
            {
                throw new ArgumentException($"{nameof(minimumVersion)} cannot be greater than {nameof(maximumVersion)}.");
            }

            if (minimumVersion == maximumVersion)
            {
                if (minIsExclusive || maxIsExclusive)
                {
                    throw new ArgumentException($"Cannot have exclusive version constraints when {nameof(minimumVersion)} is equal to {nameof(maximumVersion)}.");
                }
                this.requiredVersion = minimumVersion;
            }
            else
            {
                this.minimumVersion = minimumVersion;
                this.maximumVersion = maximumVersion;
            }
        }


        public Version? MinimumVersion { get => minimumVersion; }
        public Version? MaximumVersion { get => maximumVersion; }
        public Version? RequiredVersion { get => requiredVersion; }
        public bool MinimumVersionIsExclusive { get => minIsExclusive; }
        public bool MaximumVersionIsExclusive { get => maxIsExclusive; }
    }
}