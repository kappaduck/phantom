# Phantom

Phantom is a high-performance game engine built on [SDL3] and its extensions ([SDL_image], [SDL_ttf], and [SDL_mixer]), designed for modern .NET 9.0+ applications.

With a clean, flexible, and intuitive API, Phantom eliminates the need for low-level coding, 
allowing developers to focus entirely on building their games and multimedia applications. 
Under the hood, it leverages SDL3’s power while maintaining a fully managed, high-level .NET experience.

## Phantom & SDL3 Compatibility

Below is a list of Phantom versions and the corresponding SDL3 versions they support:

| Phantom Version | SDL3 Version | SDL_image Version | SDL_ttf Version | SDL_mixer Version |
| :-------------: | :----------: | :---------------: | :-------------: | :---------------: |
|   `>= 0.1.0`    |   `3.2.8`    |   `unsupported`   |  `unsupported`  |   `unsupported`   |

> Support for SDL_image, SDL_ttf, and SDL_mixer is planned for future releases. Stay tuned!

## Cross-Platform

Phantom currently supports Windows, with Linux and WebAssembly support planned for future releases.

> Other platforms may be considered in the future, but there are no plans for them at this time.

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
