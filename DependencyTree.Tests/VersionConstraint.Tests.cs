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
            var vc = new VersionConstraint
            {
                RequiredVersion = middleVersion
            };
            Assert.Null(vc.MinimumVersion);
            Assert.Equal(vc.RequiredVersion, middleVersion);
        }
    }
}
