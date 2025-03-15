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
internal static class KeyboardInput
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

                // Detect if a key is pressed
                if (e.Type is EventType.KeyDown && e.Keyboard.Code is Keyboard.Scancode.A)
                {
                    Console.WriteLine("A key is pressed");
                }

                // You can use the extension methods to check if a key is pressed
                if (e.IsKeyDown(Keyboard.Scancode.D))
                {
                    Console.WriteLine("D key is pressed");
                }

                // Detect if a key is released
                if (e.IsKeyUp(Keyboard.Scancode.A))
                {
                    Console.WriteLine("A key is released");
                }

                // Detect if a modifier key is pressed
                if (e.Type == EventType.KeyDown && e.Keyboard.Modifiers == Keyboard.Modifier.LeftShift)
                {
                    Console.WriteLine("Shift key is pressed");
                }
            }

            // You can also check if a key is pressed outside of the event loop
            if (Keyboard.IsPressed(Keyboard.Scancode.S))
            {
                Console.WriteLine("S key is pressed");
            }
        }
    }
}
