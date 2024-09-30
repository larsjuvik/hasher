# Hasher

[![CI](https://github.com/larsjuvik/hasher/actions/workflows/CI.yml/badge.svg)](https://github.com/larsjuvik/hasher/actions/workflows/CI.yml)
![GitHub License](https://img.shields.io/github/license/larsjuvik/hasher)
![Static Badge](https://img.shields.io/badge/made_with-C%23-blue)

A cross-platform, simple-to-use application for computing and verifying file-hashes.
Comes as a GUI- and console-application. Built on .NET 8.


<p align="center">
  <img src="docs/res/Hasher_GUI.png" style="width: 700px" />
</p>
<p align="center" >
  <img src="docs/res/Hasher_Console.png" style="width: 668px" />
</p>


## Build project

In order to build this project, you need to install MAUI:

```shell
dotnet workload install maui
```

### macOS

GUI:
```bash
dotnet publish -c Release -f net8.0-maccatalyst -p:CreatePackage=false Hasher/Hasher.csproj
```

CLI:
```bash
dotnet publish -c Release -o MY_OUTPUT_DIR Console/Console.csproj
```

## Console Examples

In UNIX-shells:
```shell
./hasher --help  # show help screen
./hasher -a sha256 -f /path/to/file  # hash file
./hasher -a sha256 -f /path/to/file -v enter_your_hash  # hash and then verify
./hasher -l  # list available algorithms
```

## Supported algorithms

- MD5
- SHA1
- SHA256
- SHA384
- SHA512

## Remaining work

1. More testing of the application itself, and more test-cases in unit tests
2. ~~Add an app logo, s.t. it shows a different logo than the purple ".NET" in the dock.~~
3. ~~CI pipeline~~
4. Testing on Windows
5. Build instructions for Windows
6. ~~Add console application for hasher~~

## Thanks :heart:

Thanks to the following libraries and frameworks for helping build hasher:
* [.NET MAUI](https://github.com/dotnet/maui)
* [ShellProgressBar](https://github.com/Mpdreamz/shellprogressbar)
* [Command Line Parser](https://github.com/commandlineparser/commandline)
