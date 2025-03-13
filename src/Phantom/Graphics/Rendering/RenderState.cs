// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Geometry;
using Phantom.Graphics.Drawing.BlendModes;
using System.Runtime.InteropServices;

namespace Phantom.Graphics.Rendering;

/// <summary>
/// Defines the states used for drawing to a <see cref="IRenderTarget"/>.
/// </summary>
[StructLayout(LayoutKind.Auto)]
public readonly record struct RenderState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RenderState"/> struct.
    /// </summary>
    public RenderState()
    {
    }

    /// <summary>
    /// Gets the blend mode for drawing operations (fill and line).
    /// </summary>
    /// <remarks>
    /// If the blend mode is not supported by the renderer, the closest supported mode is chosen.
    /// </remarks>
    public readonly BlendMode? BlendMode { get; init; }

    /// <summary>
    /// Gets the clipping rectangle for rendering.
    /// </summary>
    /// <remarks>
    /// Setting the clipping rectangle to <see langword="null"/> will disable clipping.
    /// </remarks>
    public readonly RectInt? Clip { get; init; }

    /// <summary>
    /// Gets the color scale used by the renderer.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The color scale is an additional scale multiplied into the pixel color value while rendering. This can be used
    /// to adjust the brightness of colors during HDR rendering, or changing HDR video brightness when playing on an SDR display.
    /// </para>
    /// <para>
    /// It does not affect the alpha channel, only the color brightness.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">The value is negative.</exception>
    public readonly float? ColorScale
    {
        get;
        init
        {
            if (!value.HasValue)
                return;

            ArgumentOutOfRangeException.ThrowIfNegative(value.Value);

            field = value;
        }
    }

    /// <summary>
    /// Gets the drawing scale for rendering on the current target.
    /// </summary>
    /// <remarks>
    /// The drawing coordinates are scaled by the x/y scaling factors before they are used by the renderer.
    /// This allows resolution independent drawing with a single coordinate system.
    /// If this results in scaling or subpixel drawing by the rendering backend,
    /// it will be handled using the appropriate quality hints.For best results use integer scaling factors.
    /// </remarks>
    public readonly Vector2? Scale { get; init; }

    internal static RenderState Default { get; } = new RenderState();
}
