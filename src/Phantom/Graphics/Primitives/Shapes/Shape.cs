// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Geometry;
using Phantom.Graphics.Drawing;
using Phantom.Graphics.Rendering;
using System.Drawing;

namespace Phantom.Graphics.Primitives.Shapes;

/// <summary>
/// Represents a shape that can be drawn.
/// </summary>
/// <remarks>
/// Initializes a shape with a specified position and color.
/// </remarks>
public abstract class Shape : IDrawable
{
    private bool _needsUpdate = true;

    /// <summary>
    /// Initializes a shape with a specified position and color.
    /// </summary>
    /// <param name="points">Defines the number of vertices in the shape.</param>
    /// <param name="position">Defines the location of the shape in a 2D space.</param>
    /// <param name="color">Sets the visual appearance of the shape.</param>
    /// <param name="filled">Determines whether the shape is filled.</param>
    /// <param name="thickness">The thickness of the shape.</param>
    protected Shape(int points, Vector2 position, Color color, bool filled = true, float thickness = 1f)
    {
        Vertices = new Vertex[points];
        Position = position;
        Color = color;
        Filled = filled;
        Thickness = thickness;
    }

    /// <summary>
    /// Gets or sets the position of the shape.
    /// </summary>
    public Vector2 Position
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                _needsUpdate = true;
            }
        }
    }

    /// <summary>
    /// Gets or sets the color of the shape.
    /// </summary>
    public Color Color
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                _needsUpdate = true;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the shape is filled.
    /// </summary>
    public bool Filled
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                _needsUpdate = true;
            }
        }
    }

    /// <summary>
    /// Gets or sets the thickness of the shape.
    /// </summary>
    public float Thickness
    {
        get;
        set
        {
            if (!field.Equals(value))
            {
                field = value;
                _needsUpdate = true;
            }
        }
    }

    /// <summary>
    /// Gets the vertices of the shape.
    /// </summary>
    protected Vertex[] Vertices { get; init; }

    /// <summary>
    /// Gets the indices to draw the vertices in the correct order.
    /// </summary>
    protected abstract int[] Indices { get; }

    /// <summary>
    /// Marks the shape as dirty and needs an update.
    /// </summary>
    protected void MarkDirty() => _needsUpdate = true;

    /// <summary>
    /// Update the shape.
    /// </summary>
    protected abstract void Update();

    /// <inheritdoc/>
    void IDrawable.Draw(IRenderTarget target)
    {
        if (_needsUpdate)
        {
            Update();
            _needsUpdate = false;
        }

        target.Draw(Vertices, Indices);
    }
}
