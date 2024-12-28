# Function definitions
function New-License($Path, $CopyrightBearer, $CopyrightYear) {
    $Content = @"
MIT License

Copyright (c) $($CopyrightYear) $($CopyrightBearer)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

"@

    New-Item -Path $Path -Name LICENSE.md -ItemType File -Value $Content -Force
}

function New-ReadMe($Path, $PackageName, $PackageDescription) {
    $Content = @"
# $($PackageName)
$($PackageDescription)

"@

    New-Item -Path $Path -Name README.md -ItemType File -Value $Content -Force
}

function New-PackageJson($Path, $OrganizationName, $PackageName, $Version) {
    $Content = @"
{
    "name": "com.$($OrganizationName).$($PackageName)",
    "version": "$($Version)"
}

"@

    New-Item -Path $Path -Name package.json -ItemType File -Value $Content -Force
}

function New-RuntimeAssemblyDefinition($Path, $OrganizationName, $PackageName) {
    $Name = "$($OrganizationName).$($PackageName).Runtime"
    $Content = @"
{
    `"name`": `"$($Name)`",
    `"rootNamespace`": `"$($Name)`",
    `"references`": [],
    `"includePlatforms`": [],
    `"excludePlatforms`": [],
    `"allowUnsafeCode`": false,
    `"overrideReferences`": false,
    `"precompiledReferences`": [],
    `"autoReferenced`": true,
    `"defineConstraints`": [],
    `"versionDefines`": [],
    `"noEngineReferences`": false
}

"@

    New-Item -Path $Path -Name "$($Name).asmdef" -ItemType File -Value $Content -Force
}

function New-RuntimeAssemblyDefinition($Path, $OrganizationName, $PackageName) {
    $Name = "$($OrganizationName).$($PackageName).Runtime"
    $Content = @"
{
    `"name`": `"$($Name)`",
    `"rootNamespace`": `"$($Name)`",
    `"references`": [],
    `"includePlatforms`": [],
    `"excludePlatforms`": [],
    `"allowUnsafeCode`": false,
    `"overrideReferences`": false,
    `"precompiledReferences`": [],
    `"autoReferenced`": true,
    `"defineConstraints`": [],
    `"versionDefines`": [],
    `"noEngineReferences`": false
}

"@

    New-Item -Path $Path -Name "$($Name).asmdef" -ItemType File -Value $Content -Force
}

function New-EditorAssemblyDefinition($Path, $OrganizationName, $PackageName) {
    $Name = "$($OrganizationName).$($PackageName).Editor"
    $Content = @"
{
    `"name`": `"$($Name)`",
    `"rootNamespace`": `"$($Name)`",
    `"references`": [],
    `"includePlatforms`": [
        `"Editor`"
    ],
    `"excludePlatforms`": [],
    `"allowUnsafeCode`": false,
    `"overrideReferences`": false,
    `"precompiledReferences`": [],
    `"autoReferenced`": true,
    `"defineConstraints`": [],
    `"versionDefines`": [],
    `"noEngineReferences`": false
}

"@

    New-Item -Path $Path -Name "$($Name).asmdef" -ItemType File -Value $Content -Force
}

function New-EditorTestsAssemblyDefinition($Path, $OrganizationName, $PackageName) {
    $Name = "$($OrganizationName).$($PackageName).Tests.Editor"
    $Content = @"
{
    `"name`": `"$($Name)`",
    `"rootNamespace`": `"$($Name)`",
    `"references`": [
        `"UnityEngine.TestRunner`",
        `"UnityEditor.TestRunner`"
    ],
    `"includePlatforms`": [
        `"Editor`"
    ],
    `"excludePlatforms`": [],
    `"allowUnsafeCode`": false,
    `"overrideReferences`": true,
    `"precompiledReferences`": [
        `"nunit.framework.dll`"
    ],
    `"autoReferenced`": false,
    `"defineConstraints`": [
        `"UNITY_INCLUDE_TESTS`"
    ],
    `"versionDefines`": [],
    `"noEngineReferences`": false
}

"@

    New-Item -Path $Path -Name "$($Name).asmdef" -ItemType File -Value $Content -Force
}

function New-RuntimeTestsAssemblyDefinition($Path, $OrganizationName, $PackageName) {
    $Name = "$($OrganizationName).$($PackageName).Tests.Runtime"
    $Content = @"
{
    `"name`": `"$($Name)`",
    `"rootNamespace`": `"$($Name)`",
    `"references`": [
        `"UnityEngine.TestRunner`",
        `"UnityEditor.TestRunner`"
    ],
    `"includePlatforms`": [],
    `"excludePlatforms`": [],
    `"allowUnsafeCode`": false,
    `"overrideReferences`": true,
    `"precompiledReferences`": [
        `"nunit.framework.dll`"
    ],
    `"autoReferenced`": false,
    `"defineConstraints`": [
        `"UNITY_INCLUDE_TESTS`"
    ],
    `"versionDefines`": [],
    `"noEngineReferences`": false
}

"@

    New-Item -Path $Path -Name "$($Name).asmdef" -ItemType File -Value $Content -Force
}

# Setup configuration is defined in a single setup.json file
$Config = Get-Content ./setup.json | ConvertFrom-Json

# Shared variables
$ProjectRoot = $PSScriptRoot
$CurrentYear = (Get-Date | Select-Object -ExpandProperty Year)
$AssemblyDefinitionOrganizationName = ($Config.organization.displayName -replace ' ','')
$AssemblyDefinitionPackageName = ($Config.package.displayName -replace ' ','')

# Procedure logic
New-ReadMe -Path $ProjectRoot -PackageName $Config.package.displayName -PackageDescription $Config.package.description
New-License -Path $ProjectRoot -CopyrightYear $CurrentYear -CopyrightBearer $Config.author.name
New-PackageJson -Path $ProjectRoot -OrganizationName $Config.organization.name -PackageName $Config.package.name -Version $Config.package.version
New-RuntimeAssemblyDefinition -Path "$ProjectRoot\Runtime" -OrganizationName $AssemblyDefinitionOrganizationName -PackageName ($Config.package.displayName -replace ' ','')
New-EditorAssemblyDefinition -Path "$ProjectRoot\Editor" -OrganizationName $AssemblyDefinitionOrganizationName -PackageName $AssemblyDefinitionPackageName
New-EditorTestsAssemblyDefinition -Path "$ProjectRoot\Tests\Editor" -OrganizationName $AssemblyDefinitionOrganizationName -PackageName $AssemblyDefinitionPackageName
New-RuntimeTestsAssemblyDefinition -Path "$ProjectRoot\Tests\Runtime" -OrganizationName $AssemblyDefinitionOrganizationName -PackageName $AssemblyDefinitionPackageName
