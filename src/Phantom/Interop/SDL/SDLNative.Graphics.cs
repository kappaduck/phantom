// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Graphics.Drawing.BlendModes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Phantom.Interop.SDL;

internal static partial class SDLNative
{
    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial BlendMode SDL_ComposeCustomBlendMode(BlendFactor source, BlendFactor destination, BlendOperation operation, BlendFactor alphaSource, BlendFactor alphaDestination, BlendOperation alphaOperation);
}
