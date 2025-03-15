// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Core;
using Phantom.Graphics.Rendering;
using Phantom.Input.Events;
using Phantom.Windows;

namespace Phantom.Demo.Examples.Windows;

/// <summary>
/// Demonstrates how to create a minimal window.
/// </summary>
internal static class MinimalWindow
{
    internal static void Run()
    {
        // Initialize the engine with the video subsystem.
        using PhantomEngine engine = PhantomEngine.Init(SubSystem.Video);

        // Create a resizable window with the title "Minimal Window" and size 1080x720
        using RenderWindow window = new("Minimal Window", 1080, 720, WindowOptions.Resizable);

        // Run the window loop
        while (window.IsOpen)
        {
            while (window.Poll(out Event e))
            {
                if (e.RequestQuit())
                {
                    window.Close();
                    return;
                }
            }
        }
    }
}
