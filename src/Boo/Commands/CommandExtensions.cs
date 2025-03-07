// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Spectre.Console.Cli;

namespace Boo.Commands;

internal static class CommandExtensions
{
    internal static void AddSDLCommand(this IConfigurator config)
    {
        config.AddCommand<SDLCommand>(SDLCommand.Name)
              .WithDescription(SDLCommand.Description)
              .WithExample("sdl")
              .WithExample("sdl --force");
    }
}
