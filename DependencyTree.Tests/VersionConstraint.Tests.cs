using System;
using System.Collections.Generic;
using DependencyTree;
using Xunit;

namespace DependencyTree.Tests
{
    public class VersionConstraintTest
    {
        public static Version v1 = new Version("0.2.0");
        public static Version v2 = new Version("1.0.0");
        public static Version v3 = new Version("1.2.3");
        public static Version v4 = new Version("2.2.2");
        public static Version v5 = new Version("9.9.9");


        [Fact]
        public void TestInitializers()
        {
            VersionConstraint vc;

            vc = new VersionConstraint { RequiredVersion = v3 };
            Assert.Null(vc.MinimumVersion);
            Assert.Equal(vc.RequiredVersion, v3);
            Assert.Null(vc.MaximumVersion);


            vc = new VersionConstraint { MinimumVersion = v2, MaximumVersion = v4 };
            Assert.Equal(vc.MinimumVersion, v2);
            Assert.Null(vc.RequiredVersion);
            Assert.Equal(vc.MaximumVersion, v4);


            vc = new VersionConstraint { MinimumVersion = v3, MaximumVersion = v3 };
            Assert.Null(vc.MinimumVersion);
            Assert.Equal(vc.RequiredVersion, v3);
            Assert.Null(vc.MaximumVersion);


            Assert.Throws<ArgumentException>(() => new VersionConstraint
            {
                MinimumVersion = v3,
                MaximumVersion = v3,
                MaximumVersionIsExclusive = true
            });
        }


        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { new VersionConstraint(null), new object[] {
                new object[] {v3, true},
                new object[] {null, true},
            }},

            new object[] { new VersionConstraint(v3), new object[] {
                new object[] {v2, false},
                new object[] {v3, true},
                new object[] {v4, false},
                new object[] {null, false},
            }},

            new object[] { new VersionConstraint(v2, v4), new object[] {
                new object[] {v1, false},
                new object[] {v2, true},
                new object[] {v3, true},
                new object[] {v4, true},
                new object[] {v5, false},
                new object[] {null, false},
            }},

            new object[] { new VersionConstraint(v2, v4, true, false), new object[] {
                new object[] {v1, false},
                new object[] {v2, false},
                new object[] {v3, true},
                new object[] {v4, true},
                new object[] {v5, false},
                new object[] {null, false},
            }},

            new object[] { new VersionConstraint(v2, v4, false, true), new object[] {
                new object[] {v1, false},
                new object[] {v2, true},
                new object[] {v3, true},
                new object[] {v4, false},
                new object[] {v5, false},
                new object[] {null, false},
            }},

            new object[] { new VersionConstraint(v2, v4, true, true), new object[] {
                new object[] {v1, false},
                new object[] {v2, false},
                new object[] {v3, true},
                new object[] {v4, false},
                new object[] {v5, false},
                new object[] {null, false},
            }},
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void TestSatisfaction(VersionConstraint vc, object[][] matrix)
        {
            foreach (var pair in matrix)
            {
                var testVersion = (Version)pair[0];
                var expectedResult = (bool)pair[1];

                var result = vc.IsSatisfiedBy(testVersion);
                Assert.Equal(result, expectedResult);
            }
        }
    }
}
