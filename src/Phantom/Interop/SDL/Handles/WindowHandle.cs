// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Interop.Handles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Phantom.Interop.SDL.Handles;

internal sealed partial class WindowHandle : SafeHandleZeroInvalid
{
    /// <summary>
    /// Marshaller needs a public parameterless constructor.
    /// </summary>
    public WindowHandle() : base(ownsHandle: true)
    {
    }

    internal WindowHandle(WindowHandle window) : base(ownsHandle: false)
        => SetHandle(window.handle);

    internal static WindowHandle Zero { get; } = new();

    protected override bool ReleaseHandle()
    {
        if (!IsInvalid)
        {
            SDL_DestroyWindow(handle);

            SetHandle(nint.Zero);
            SetHandleAsInvalid();
        }

        return true;
    }

    [LibraryImport(SDLNative.LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_DestroyWindow(nint window);
}
