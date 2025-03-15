// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Core;
using Phantom.Graphics.Rendering;
using Phantom.Input;
using Phantom.Input.Events;

namespace Phantom.Demo.Examples.Inputs;

/// <summary>
/// Demonstrates how to handle mouse input.
/// </summary>
internal static class MouseInput
{
    internal static void Run()
    {
        // Initialize the engine with the video subsystem.
        using PhantomEngine engine = PhantomEngine.Init(SubSystem.Video);

        // Create the window
        using RenderWindow window = new("Minimal Window", 1080, 720);

        // Run the window loop
        while (window.IsOpen)
        {
            // Poll for events
            while (window.Poll(out Event e))
            {
                if (e.RequestQuit())
                {
                    window.Close();
                    return;
                }

                // Detect if the mouse button is pressed
                if (e.Type is EventType.MouseButtonDown && e.Mouse.Button == Mouse.Button.Left)
                {
                    Console.WriteLine("Mouse button is pressed");
                }

                // You can use the extension methods to check if the mouse button is pressed
                if (e.IsMouseButtonDown(Mouse.Button.Right))
                {
                    Console.WriteLine("Right mouse button is pressed");
                }

                // Detect if the mouse button is released
                if (e.IsMouseButtonUp(Mouse.Button.Left))
                {
                    Console.WriteLine("Mouse button is released");
                }

                // Detect if the mouse is moved
                if (e.Type is EventType.MouseMotion)
                {
                    Console.WriteLine($"Mouse moved to {e.Mouse.Position}");
                }

                // Detect if the mouse wheel is scrolled
                if (e.Type is EventType.MouseWheel)
                {
                    Console.WriteLine($"Mouse wheel scrolled {e.Wheel.Direction} ({e.Wheel.X}, {e.Wheel.Y})");
                }
            }

            // You can also use the Mouse class to check if the mouse button is pressed outside the event loop
            if (Mouse.CachedState.IsButtonDown(Mouse.Button.Middle))
            {
                Console.WriteLine("Middle mouse button is pressed");
            }
        }
    }
}
