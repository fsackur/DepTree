using System;
using DependencyTree;
using Xunit;

namespace DependencyTree.Tests
{
    public class VersionConstraintTest
    {
        Version lowVersion = new Version("1.0.0");
        Version middleVersion = new Version("1.2.3");
        Version highVersion = new Version("9.9.9");

        [Fact]
        public void TestInitializers()
        {
            VersionConstraint vc;

            vc = new VersionConstraint
            {
                RequiredVersion = middleVersion
            };
            Assert.Null(vc.MinimumVersion);
            Assert.Equal(vc.RequiredVersion, middleVersion);
            Assert.Null(vc.MaximumVersion);


            vc = new VersionConstraint
            {
                MinimumVersion = lowVersion,
                MaximumVersion = highVersion
            };
            Assert.Equal(vc.MinimumVersion, lowVersion);
            Assert.Null(vc.RequiredVersion);
            Assert.Equal(vc.MaximumVersion, highVersion);


            vc = new VersionConstraint
            {
                MinimumVersion = middleVersion,
                MaximumVersion = middleVersion
            };
            Assert.Null(vc.MinimumVersion);
            Assert.Equal(vc.RequiredVersion, middleVersion);
            Assert.Null(vc.MaximumVersion);


            Assert.Throws<ArgumentException>(() => new VersionConstraint
            {
                MinimumVersion = middleVersion,
                MaximumVersion = middleVersion,
                MaximumVersionIsExclusive = true
            });
        }
    }
}
