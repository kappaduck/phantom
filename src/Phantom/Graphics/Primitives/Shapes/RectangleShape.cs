// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Geometry;
using System.Drawing;

namespace Phantom.Graphics.Primitives.Shapes;

/// <summary>
/// Represents a rectangle shape.
/// </summary>
public sealed class RectangleShape : Shape
{
    private const int Points = 8;
    private static readonly int[] _outerIndices = [0, 1, 2, 2, 3, 0];
    private static readonly int[] _borderIndices =
    [
        0, 1, 5, 5, 4, 0,
        1, 2, 6, 6, 5, 1,
        2, 3, 7, 7, 6, 2,
        3, 0, 4, 4, 7, 3
    ];

    /// <summary>
    /// Initializes a rectangle shape with a specified position, size, and color.
    /// </summary>
    /// <param name="position">Defines the location of the rectangle in the 2D space.</param>
    /// <param name="size">Specifies the dimensions of the rectangle.</param>
    /// <param name="color">Sets the fill color of the rectangle.</param>
    /// <param name="filled">Determines whether the rectangle is filled.</param>
    /// <param name="thickness">The thickness of the rectangle.</param>
    public RectangleShape(Vector2 position, Vector2 size, Color color, bool filled = true, float thickness = 1f) : base(Points, position, color, filled, thickness)
    {
        Size = size;
        Update();
    }

    /// <summary>
    /// Initializes a new instance of an empty RectangleShape.
    /// </summary>
    public RectangleShape() : this(Vector2.Zero, Vector2.Zero, Color.White)
    {
    }

    /// <summary>
    /// Constructs a RectangleShape using the position and size from a given rectangle and a specified color.
    /// </summary>
    /// <param name="rectangle">Defines the position and size of the shape to be created.</param>
    /// <param name="color">Specifies the color of the shape.</param>
    public RectangleShape(Rect rectangle, Color color) : this(rectangle.Position, rectangle.Size, color)
    {
    }

    /// <summary>
    /// Gets or sets the rectangle size.
    /// </summary>
    public Vector2 Size
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                MarkDirty();
            }
        }
    }

    /// <inheritdoc/>
    protected override int[] Indices => Filled ? _outerIndices : _borderIndices;

    /// <inheritdoc/>
    protected override void Update()
    {
        if (Filled)
            UpdateOuterRectangle();
        else
            UpdateBorderRectangle();
    }

    private void UpdateOuterRectangle()
    {
        Vertices[0].Position = Position;
        Vertices[0].Color = Color;

        Vertices[1].Position = Position + new Vector2(Size.X, 0);
        Vertices[1].Color = Color;

        Vertices[2].Position = Position + Size;
        Vertices[2].Color = Color;

        Vertices[3].Position = Position + new Vector2(0, Size.Y);
        Vertices[3].Color = Color;
    }

    private void UpdateBorderRectangle()
    {
        Vector2 thickness = new(Thickness, Thickness);

        UpdateOuterRectangle();

        Vertices[4].Position = Position + thickness;
        Vertices[4].Color = Color;

        Vertices[5].Position = Position + new Vector2(Size.X - Thickness, Thickness);
        Vertices[5].Color = Color;

        Vertices[6].Position = Position + Size - thickness;
        Vertices[6].Color = Color;

        Vertices[7].Position = Position + new Vector2(Thickness, Size.Y - Thickness);
        Vertices[7].Color = Color;
    }
}
