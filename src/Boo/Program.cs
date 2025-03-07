// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Boo.Commands;
using Spectre.Console.Cli;
using System.Reflection;

CommandApp app = new();

app.Configure(config =>
{
    Version version = Assembly.GetExecutingAssembly().GetName().Version!;

    config.SetApplicationName("boo")
          .SetApplicationVersion($"{version.Major}.{version.Minor}.{version.Revision}");

    config.AddSDLCommand();
});

return await app.RunAsync(args)
                .ConfigureAwait(false);
