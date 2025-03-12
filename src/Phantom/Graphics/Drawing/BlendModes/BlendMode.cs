// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Phantom.Graphics.Drawing.BlendModes;

/// <summary>
/// A set of blend modes used in drawing operations.
/// </summary>
public enum BlendMode : uint
{
    /// <summary>
    /// No blending: destination RGBA = source RGBA.
    /// </summary>
    None = 0x00000000u,

    /// <summary>
    /// Alpha blending: destination RGB = (source RGB * source Alpha) + (destination RGB * (1 - source Alpha)),
    /// destination Alpha = source Alpha + (destination Alpha * (1 - source Alpha)).
    /// </summary>
    Blend = 0x00000001u,

    /// <summary>
    /// Additive blending: destination RGB = (source RGB * source Alpha) + destination RGB, destination Alpha = destination Alpha.
    /// </summary>
    Add = 0x00000002u,

    /// <summary>
    /// Color modulate: destination RGB = source RGB * destination RGB, destination Alpha = destination Alpha.
    /// </summary>
    ColorModulate = 0x00000004u,

    /// <summary>
    /// Color multiply: destination RGB = (source RGB * destination RGB) + (destination RGB * (1 - source Alpha)), destination Alpha = destination Alpha.
    /// </summary>
    ColorMultiply = 0x00000008u,

    /// <summary>
    /// Pre-multiplied alpha blending: destination RGBA = source RGBA + (destination RGBA * (1 - source Alpha)).
    /// </summary>
    BlendPreMultiplied = 0x00000010u,

    /// <summary>
    /// Pre-multiplied additive blending: destination RGB = source RGB + destination RGB, destination Alpha = destination Alpha.
    /// </summary>
    AddPreMultiplied = 0x00000020u,

    /// <summary>
    /// Invalid blend mode.
    /// </summary>
    Invalid = 0x7FFFFFFFu
}
