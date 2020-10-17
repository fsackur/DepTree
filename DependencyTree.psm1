
# Testing state of play with PS classes
# classes not exported;
#   $host.ExitNestedPrompt(); $Module = ipmo .\DependencyTree.psd1 -Force -PassThru; & $Module {$host.EnterNestedPrompt()}
#
# https://github.com/poshbotio/PoshBot => build process puts classes in the .psm1
#
class Dependency
{
    hidden [Dependency]$RequiredBy
    hidden [Collections.Generic.IList[Dependency]]$Requires = [Collections.Generic.List[Dependency]]::new()

    [void] AddRequirement([Dependency]$Dep)
    {
        [void]$this.Requires.Add($Dep)
    }

    [string] ToString()
    {
        return "bar"
    }
}

class Visitor
{

}

Get-ChildItem $PSScriptRoot\Private\*.ps1 | ForEach-Object {. $_.FullName}
Get-ChildItem $PSScriptRoot\Public\*.ps1  | ForEach-Object {. $_.FullName}
