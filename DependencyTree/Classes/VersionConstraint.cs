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
        private bool minimumVersionIsExclusive = false;
        private bool maximumVersionIsExclusive = false;

        public VersionConstraint() { }

        public VersionConstraint(Version requiredVersion) => this.requiredVersion = requiredVersion;

        public VersionConstraint(Version? minimumVersion, Version? maximumVersion, bool minimumVersionIsExclusive = false, bool maximumVersionIsExclusive = false)
        {
            if (minimumVersion == null || maximumVersion == null)
            {
                this.minimumVersion = minimumVersion;
                this.maximumVersion = maximumVersion;
                this.minimumVersionIsExclusive = minimumVersionIsExclusive;
                this.maximumVersionIsExclusive = maximumVersionIsExclusive;
                return;
            }

            if (minimumVersion > maximumVersion)
            {
                throw new ArgumentException($"{nameof(minimumVersion)} cannot be greater than {nameof(maximumVersion)}.");
            }

            if (minimumVersion == maximumVersion)
            {
                if (minimumVersionIsExclusive || maximumVersionIsExclusive)
                {
                    throw new ArgumentException(
                        $"Cannot have exclusive version constraints when {nameof(minimumVersion)} is equal to {nameof(maximumVersion)}."
                    );
                }
                this.requiredVersion = minimumVersion;
            }
            else
            {
                this.minimumVersion = minimumVersion;
                this.maximumVersion = maximumVersion;
            }

            this.minimumVersionIsExclusive = minimumVersionIsExclusive;
            this.maximumVersionIsExclusive = maximumVersionIsExclusive;
        }

        public Version? MinimumVersion { get => minimumVersion; }
        public Version? MaximumVersion { get => maximumVersion; }
        public Version? RequiredVersion { get => requiredVersion; }
        public bool MinimumVersionIsExclusive { get => minimumVersionIsExclusive; }
        public bool MaximumVersionIsExclusive { get => maximumVersionIsExclusive; }
    }
}