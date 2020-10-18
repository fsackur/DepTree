using System;
using System.Management.Automation;
using System.Collections.Generic;

namespace DependencyTree
{
    public class Dependency<T> : Dependency, ISatisfiable<Dependency>
    {
        public T? ResolvedBy;
        public override bool IsResolved { get => ResolvedBy != null; }
        public override Version? ResolvedVersion { get; protected set; }


        public Dependency(string name, VersionConstraint versionConstraint) : base(name, versionConstraint) { }


        public bool IsSatisfiedBy(Dependency other)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));

            return (
                Name == other.Name &&
                GetType() == other.GetType() &&
                VersionConstraint.IsSatisfiedBy(other.ResolvedVersion)
            );
        }


        public void ResolveWith(T dependencyObject, Version? version = null)
        {
            if (dependencyObject is null) throw new ArgumentNullException(nameof(dependencyObject));

            ResolvedVersion = version;
            ResolvedBy = dependencyObject;
        }


        public void AddDependency(Dependency dependency)
        {
            if (dependency is null) throw new ArgumentNullException(nameof(dependency));

            if (!IsResolved) throw new InvalidOperationException("Cannot add dependencies of an unresolved dependency.");

            Requires.Add(dependency);
        }
    }
}
