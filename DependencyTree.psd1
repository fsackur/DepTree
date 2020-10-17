@{
    Description          = 'Generate a dependency tree object for things that need other things, e.g. Powershell modules.'
    ModuleVersion        = '0.0.1'
    HelpInfoURI          = 'https://pages.github.com/fsackur/DependencyTree'

    GUID                 = '7cbc5970-d4be-44ac-b63a-c5eb2ea32f87'

    RequiredModules      = @()

    Author               = 'Freddie Sackur'
    CompanyName          = 'DustyFox'
    Copyright            = '(c) 2020 Freddie Sackur. All rights reserved.'

    RootModule           = 'DependencyTree.psm1'

    FunctionsToExport    = @(
        '*'
    )

    PrivateData          = @{
        PSData = @{
            LicenseUri = 'https://raw.githubusercontent.com/fsackur/DependencyTree/main/LICENSE'
            ProjectUri = 'https://github.com/fsackur/DependencyTree'
            Tags       = @(
                'Tree',
                'Dependency',
                'Require',
                'Required',
                'RequiredModule',
                'RequiredModules'
            )
        }
    }
}
