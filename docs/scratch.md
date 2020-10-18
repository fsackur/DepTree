# Scratch notes

Powershell classes still have difficulties: https://github.com/PowerShell/PowerShell/issues/6652

I may want to use interfaces.

Decision: write in C#
https://docs.microsoft.com/en-us/powershell/scripting/dev-cross-plat/writing-portable-modules?view=powershell-7

.NET 5 currently at RC2, will offer C# 9 and single-file builds. I believe I'll enjoy .NET 5 so let's go with that.

Pattern-matching:
https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/pattern-matching


Testing:
https://xpirit.com/netcore-withvscode-should-haveunit-tests/
https://dev.to/hatsrumandcode/net-core-2-why-xunit-and-not-nunit-or-mstest--aei
http://blog.cleancoder.com/uncle-bob/2017/05/05/TestDefinitions.html
https://xunit.net/docs/getting-started/netcore/cmdline
dotnet new xunit
dotnet add reference ..\DependencyTree\DependencyTree.csproj
dotnet add package coverlet.msbuild     # test coverage
install formulahendry.dotnet-test-explorer
