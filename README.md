# OmniSharpOfflinePackager

Simple app on **.NET Core 3.1** to create [omnisharp-vscode](https://github.com/OmniSharp/omnisharp-vscode) offline package on online PC. It’s just implemented into app [official guide to create offline package](https://github.com/OmniSharp/omnisharp-vscode/wiki/Installing-the-C%23-extension-to-a-computer-without-internet-connectivity#installing-on-a-computer-that-cannot-connect-to-the-internet).

You can build project in **VS2019** (**16.4.5**+) or in **VSCode** (**1.41.1**+) with mentioned [omnisharp-vscode](https://github.com/OmniSharp/omnisharp-vscode) extension (**1.21.12**+).

![icon](OmniSharpOfflinePackager/Resources/icon.png)

Icon is made by [SimpleIcon](https://www.flaticon.com/authors/simpleicon) from [FlatIcon](https://www.flaticon.com).

[![Build status](https://ci.appveyor.com/api/projects/status/lsal8vau7s1jw1c7/branch/master?svg=true)](https://ci.appveyor.com/project/Gigas002/omnisharpofflinepackager) [![Actions Status](https://github.com/Gigas002/OmniSharpOfflinePackager/workflows/.NET%20Core/badge.svg)](https://github.com/Gigas002/OmniSharpOfflinePackager/actions)

## Current version

Current stable can be found here: [![Release](https://img.shields.io/github/release/Gigas002/OmniSharpOfflinePackager.svg)](https://github.com/Gigas002/OmniSharpOfflinePackager/releases/latest).

Information about changes since previous releases can be found in [changelog](https://github.com/Gigas002/OmniSharpOfflinePackager/blob/master/CHANGELOG.md). This project supports [SemVer 2.0.0](https://semver.org/) (template is `{MAJOR}.{MINOR}.{PATCH}.{BUILD}`).

Previous versions can be found on [releases](https://github.com/Gigas002/OmniSharpOfflinePackager/releases) and [branches](https://github.com/Gigas002/OmniSharpOfflinePackager/branches) pages.

## Requirements

- [Git](https://git-scm.com/downloads) – 2.23.0 or later
- [Node.js](https://nodejs.org/en/download/current/) – 12.10.0 or later

## Dependencies

- [CommandLineParser](https://www.nuget.org/packages/CommandLineParser/) – 2.7.82;

## Usage

| Short |       Long        |            Description             | Required? |
| :---: | :---------------: | :--------------------------------: | :-------: |
|  -p   | --package-version |     Package version to create      |    Yes    |
|  -o   |     --output      |   Full path to output directory    |    No     |
|       |     --version     |          Current version           |           |
|       |      --help       | Message about command line options |           |

`--package-version` is a `string`, representing **omnisharp-vscode** extension version to create.

`-o/--output` is a `string`, representing full path to ready **omnisharp-vscode** packages. If not set – ready packages are located in cloned repo’s directory.

Simple example looks like this: `OmniSharpOfflinePackager --package-version 1.21.2 --output "C:/OmniSharpPackage"`.

## Localization

Localizable strings are located in `Localization/Strings.resx` file. You can add your translation (e.g. added `Strings.Ru.resx` file) and create pull request.

Currently, application is available on **English** and **Russian** languages.

## Contributing

Feel free to contribute, make forks, change some code, add [issues](https://github.com/Gigas002/OmniSharpOfflinePackager/issues), etc.
