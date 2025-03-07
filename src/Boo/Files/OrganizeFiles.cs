// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Spectre.Console;

namespace Boo.Files;

internal sealed class OrganizeFiles(string installPath, (string Pattern, string Destination)[] rules) : FileHandler
{
    internal override ValueTask<Result> HandleAsync(string file)
    {
        AnsiConsole.MarkupLine("[cyan]Organizing files...[/]");

        try
        {
            DirectoryInfo source = new(file[..file.LastIndexOf('.')]);
            foreach ((string pattern, string destination) in rules)
            {
                foreach (FileInfo sourceFile in source.EnumerateFiles(pattern, SearchOption.TopDirectoryOnly))
                {
                    string targetPath = Path.Combine(installPath, destination, sourceFile.Name);

                    Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);
                    sourceFile.MoveTo(targetPath, overwrite: true);
                }
            }

            source.Delete(recursive: true);
        }
        catch (Exception ex)
        {
            return ValueTask.FromResult(Result.Failure($"Failed to organize files: {ex.Message}"));
        }

        return base.HandleAsync(file);
    }
}
