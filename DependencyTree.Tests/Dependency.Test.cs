using System;
using System.Collections.Generic;
using DependencyTree;
using Xunit;

namespace DependencyTree.Tests
{
    public class DependencyTest
    {
        public static Version v1 = new Version("0.2.0");
        public static Version v2 = new Version("1.0.0");
        public static Version v3 = new Version("1.2.3");
        public static Version v4 = new Version("2.2.2");
        public static Version v5 = new Version("9.9.9");

        public static VersionConstraint vc = new VersionConstraint { MinimumVersion = v2, MaximumVersion = v4 };

        [Fact]
        public void TestResolveWith()
        {
            var d1 = new Dependency<object>("dep", vc);
            object o = new object();

            d1.ResolveWith(o, v3);

            Assert.Equal(d1.ResolvedBy, o);
            Assert.Equal(d1.ResolvedVersion, v3);
        }

        [Fact]
        public void TestAddDependency()
        {
            var d1 = new Dependency<object>("dep", vc);
            object o = new object();
            d1.ResolveWith(o, v3);

            var d2 = new Dependency<object>("dep2", new VersionConstraint { MinimumVersion = v2, MaximumVersion = v4 });
            var d3 = new Dependency<object>("dep3", new VersionConstraint { MinimumVersion = v2, MaximumVersion = v4 });

            d1.AddDependency(d2);
            d1.AddDependency(d3);

            Assert.Equal(d1.Requires[0], d2);
            Assert.Equal(d1.Requires[1], d3);
        }

                [Fact]
        public void TestIsSatisfiedBy()
        {
            var d1 = new Dependency<object>("dep", vc);
            var d2 = new Dependency<object>("dep", new VersionConstraint { MinimumVersion = v2, MaximumVersion = v4 });
            object o = new object();
            d2.ResolveWith(o, v3);
            var d3 = new Dependency<object>("dep", new VersionConstraint { MinimumVersion = v1, MaximumVersion = v4 });
            d3.ResolveWith(o, v1);
            var d4 = new Dependency<object>("not_dep", new VersionConstraint { RequiredVersion = v3 });

            var d5 = new Dependency<object>("dep", new VersionConstraint());

            // Resolved, version is OK
            Assert.True(d1.IsSatisfiedBy(d2));

            // Resolved, version is not OK
            Assert.False(d1.IsSatisfiedBy(d3));

            // Name is not OK
            Assert.False(d1.IsSatisfiedBy(d4));

            // No version constraint
            Assert.True(d5.IsSatisfiedBy(d2));

            // No version constraint, name is not OK
            Assert.False(d5.IsSatisfiedBy(d4));
        }
    }
}
