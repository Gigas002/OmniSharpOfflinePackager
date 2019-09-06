# OmniSharpOfflinePackager

Simple app on **.NET Core 3.0** (**preview9**) to create [omnisharp-vscode](https://github.com/OmniSharp/omnisharp-vscode) offline package on online PC.

You can build project in **VS2019** (**16.2.4**+) or in **VSCode** (**1.38.0**+) with mentioned [omnisharp-vscode](https://github.com/OmniSharp/omnisharp-vscode) extension (**1.21.2**+).

*todo appveyor badge...*

## Current version

Current stable can be found here: [![Release](https://img.shields.io/github/release/Gigas002/OmniSharpOfflinePackager.svg)](https://github.com/Gigas002/OmniSharpOfflinePackager/releases/latest).

Information about changes since previous releases can be found in [changelog](https://github.com/Gigas002/OmniSharpOfflinePackager/blob/master/CHANGELOG.md). This project supports [SemVer 2.0.0](https://semver.org/) (template is `{MAJOR}.{MINOR}.{PATCH}.{BUILD}`).

Previous versions can be found on [releases](https://github.com/Gigas002/OmniSharpOfflinePackager/releases) and [branches](https://github.com/Gigas002/OmniSharpOfflinePackager/branches) pages.

## Requirements

- [Git](https://git-scm.com/downloads) – 2.22.0 or later
- [Node.js](https://nodejs.org/en/download/current/) – 12.8.0 or later

## Dependencies

- [CommandLineParser](https://www.nuget.org/packages/CommandLineParser/) – 2.6.0;

## Usage

| Short |       Long        |            Description             | Required? |
| :---: | :---------------: | :--------------------------------: | :-------: |
|       | --package-version |     Package version to create      |    Yes    |
|  -o   |     --output      |   Full path to output directory    |    Yes    |
|       |     --version     |          Current version           |           |
|       |      --help       | Message about command line options |           |

`--package-version` is a `string`, representing **omnisharp-vscode** extension version to create.

`-o/--output` is a `string`, representing full path to ready **omnisharp-vscode** packages.

Simple example looks like this: `OmniSharpOfflinePackager --package-version 1.21.0 --output "C:/OmniSharpPackage"`.

## Localization

Localizable strings are located in `Localization/Strings.resx` file. You can add your translation (e.g. added `Strings.Ru.resx`file) and create pull request.

Currently, application is available on **English** and **Russian** languages.

## Contributing

Feel free to contribute, make forks, change some code, add [issues](https://github.com/Gigas002/OmniSharpOfflinePackager/issues), etc.
