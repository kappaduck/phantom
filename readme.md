# Phantom

Phantom is a high-performance game engine built on top of the [SDL3] library and his extensions ([SDL_image], [SDL_ttf] and [SDL_mixer]),
designed for modern .NET 9.0+ applications.

It provides a clean, flexible, and intuitive API, making it easy to create games and multimedia applications. By abstracting the underlying SDL3 API, Phantom allows developers to focus on creating their games without worrying about the low-level details.

## Compability

Here is a list of Phantom versions and the SDL3 versions they support:

| Phantom Version | SDL3 Version | SDL_image Version | SDL_ttf Version | SDL_mixer Version |
| :-------------: | :----------: | :---------------: | :-------------: | :---------------: |
|   `>= 0.1.0`    |   `3.2.8`    |   `unsupported`   |  `unsupported`  |   `unsupported`   |

## Cross-Platform

At the moment, Phantom is only available for Windows, but it will be available for Linux and WebAssembly in the future.

> It is possible that Phantom will support other platforms but it is not planned at the moment.

## Installation

*Work in progress...*

## Usage

*Work in progress...*

## Development

To build Phantom from source, you will need the following tools installed:

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

> The SDK includes everything you need to build and run .NET applications on your machine.

### Boo!

`Boo` is a simple dotnet tool that allows you to setup Phantom and its dependencies. 
You can install it by running the setup script in the root directory:

> The script will restore/pack Boo project and install as local dotnet tool.

> windows
```sh
./setup.bat
```

> linux
```sh
./setup.sh
```
#### Usage

To install latest SDL3 version
```bash
dotnet boo sdl
```

To install a specific version of SDL3
```bash
dotnet boo sdl --version 3.2.8
```

## Credits

Phantom leverages and draws inspiration from the following projects:

- [SDL3]
- [SDL_image]
- [SDL_ttf]
- [SDL_mixer]
- [LazyFoo](https://lazyfoo.net/index.php)
- [Sayers.SDL2.Core](https://github.com/JeremySayers/Sayers.SDL2.Core)
- [SDL3-CS](https://github.com/flibitijibibo/SDL3-CS)
- [SFML](https://www.sfml-dev.org/)

[SDL3]: https://www.libsdl.org/
[SDL_image]: https://www.libsdl.org/projects/SDL_image/
[SDL_ttf]: https://www.libsdl.org/projects/SDL_ttf/
[SDL_mixer]: https://www.libsdl.org/projects/SDL_mixer/
