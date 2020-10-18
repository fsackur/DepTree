using System;
using System.Management.Automation;
using System.Collections.Generic;

namespace DependencyTree
{
    public class Dependency<T> : Dependency
    {
        public T? ResolvedBy;
        public override bool IsResolved { get => ResolvedBy != null; }


        public Dependency(string name, VersionConstraint versionConstraint) : base(name, versionConstraint) { }


        public void ResolveWith(T dependencyObject)
        {
            if (dependencyObject is null) throw new ArgumentNullException(nameof(dependencyObject));

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
