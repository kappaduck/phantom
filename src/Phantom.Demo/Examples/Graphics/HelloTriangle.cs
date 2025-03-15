// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Core;
using Phantom.Geometry;
using Phantom.Graphics.Primitives;
using Phantom.Graphics.Rendering;
using Phantom.Input.Events;
using Phantom.Windows;
using System.Drawing;

namespace Phantom.Demo.Examples.Graphics;

/// <summary>
/// Demonstrates how to render a simple 2D triangle.
/// </summary>
internal static class HelloTriangle
{
    private const int Width = 1080;
    private const int Height = 720;

    internal static void Run()
    {
        // Initialize the engine with the video subsystem
        using PhantomEngine engine = PhantomEngine.Init(SubSystem.Video);

        Vertex[] vertices = CreateTriangle();

        // Create the window
        using RenderWindow window = new("Hello Triangle!", Width, Height, WindowOptions.Resizable);

        // Run the main loop
        while (window.IsOpen)
        {
            // Poll for events
            while (window.Poll(out Event e))
            {
                // Close the window if the user requests to quit
                if (e.RequestQuit())
                {
                    window.Close();
                    return;
                }
            }

            // Clear the window with black color
            window.Clear();

            // Draw the triangle
            window.Draw(vertices);

            // Render the graphics to the window since the last call
            window.Render();
        }
    }

    /// <summary>
    /// Creates a triangle with red, green and blue vertices.
    /// </summary>
    /// <returns>The vertices of the triangle.</returns>
    private static Vertex[] CreateTriangle()
    {
        // Create the vertices with red, green and blue colors
        Vertex[] vertices =
        [
            new Vertex(Color.Red),
            new Vertex(Color.Green),
            new Vertex(Color.Blue)
        ];

        const float radius = 200.0f;
        const float start = -MathF.PI / 2;

        // Calculate the position of the vertices
        for (int i = 0; i < vertices.Length; i++)
        {
            float angle = start + (i * (2 * MathF.PI / vertices.Length));

            float x = (Width / 2) + (MathF.Cos(angle) * radius);
            float y = (Height / 2) + (MathF.Sin(angle) * radius);

            vertices[i].Position = new Vector2(x, y);
        }

        return vertices;
    }
}
