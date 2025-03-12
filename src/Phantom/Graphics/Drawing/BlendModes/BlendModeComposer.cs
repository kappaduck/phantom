// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Interop.SDL;

namespace Phantom.Graphics.Drawing.BlendModes;

/// <summary>
/// Used to compose a custom blend mode.
/// </summary>
public static class BlendModeComposer
{
    /// <summary>
    /// Composes a custom blend mode.
    /// </summary>
    /// <param name="source">The factor applied to the red, green, and blue components of the source pixel.</param>
    /// <param name="destination">The factor applied to the red, green, and blue components of the destination pixel.</param>
    /// <param name="operation">The operation used to combine the source and destination pixel components.</param>
    /// <param name="sourceAlpha">The factor applied to the alpha component of the source pixel.</param>
    /// <param name="destinationAlpha">The factor applied to the alpha component of the destination pixel.</param>
    /// <param name="alphaOperation">The operation used to combine the source and destination alpha components.</param>
    /// <returns>The composed blend mode.</returns>
    public static BlendMode Compose(BlendFactor source, BlendFactor destination, BlendOperation operation, BlendFactor sourceAlpha, BlendFactor destinationAlpha, BlendOperation alphaOperation)
        => SDLNative.SDL_ComposeCustomBlendMode(source, destination, operation, sourceAlpha, destinationAlpha, alphaOperation);
}
