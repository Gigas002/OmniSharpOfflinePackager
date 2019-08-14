# OmniSharpOfflinePackager

Simple app on .NET Core 3.0-preview8 to create [omnisharp-vscode](https://github.com/OmniSharp/omnisharp-vscode) offline package on online PC.

*todo appveyor badge*

## Current version

Current stable can be found here: [![Release](https://img.shields.io/github/release/Gigas002/OmniSharpOfflinePackager.svg)](https://github.com/Gigas002/OmniSharpOfflinePackager/releases/latest).

Information about changes since previous releases can be found in [changelog](https://github.com/Gigas002/OmniSharpOfflinePackager/blob/master/CHANGELOG.md). This project supports [SemVer 2.0.0](https://semver.org/) (template is `{MAJOR}.{MINOR}.{PATCH}.{BUILD}`).

Previous versions can be found on [releases](https://github.com/Gigas002/OmniSharpOfflinePackager/releases) and [branches](https://github.com/Gigas002/OmniSharpOfflinePackager/branches) pages.

## Requirements

- [Git](https://git-scm.com/downloads) – 2.22.0 or later
- [Node.js](https://nodejs.org/en/download/current/) – 12.8.0 or later
- **TEMP** *[.NET Core 3.0 Runtime 3.0.0-preview8-013656](https://dotnet.microsoft.com/download/dotnet-core/3.0)*

## Dependencies

- [CommandLineParser](https://www.nuget.org/packages/CommandLineParser/) – 2.6.0;

## Usage

| Short |       Long        |          Description          | Required? |
| :---: | :---------------: | :---------------------------: | :-------: |
|       | --package-version |   Package version to create   |    Yes    |
|  -o   |     --output      | Full path to output directory |    Yes    |

`--package-version` is a `string`, representing **omnisharp-vscode** extension version to create.

`-o/--output` is a `string`, representing full path to ready **omnisharp-vscode** packages.

Simple example looks like this: `OmniSharpOfflinePackager --package-version 1.21.0 --output "C:/OmniSharpPackage"`.

## Localization

*todo?*

## Contributing

Feel free to contribute, make forks, change some code, add [issues](https://github.com/Gigas002/OmniSharpOfflinePackager/issues), etc.
