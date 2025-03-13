// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Interop.Handles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Phantom.Interop.SDL.Handles;

internal sealed partial class RendererHandle() : SafeHandleZeroInvalid(ownsHandle: true)
{
    internal static RendererHandle Zero { get; } = new();

    protected override bool ReleaseHandle()
    {
        SDL_DestroyRenderer(handle);

        SetHandle(nint.Zero);
        SetHandleAsInvalid();

        return true;
    }

    [LibraryImport(SDLNative.LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_DestroyRenderer(nint renderer);
}
