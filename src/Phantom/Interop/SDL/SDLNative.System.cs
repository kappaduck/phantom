// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Phantom.Interop.SDL;

internal static partial class SDLNative
{
    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial DevicePower.PowerState SDL_GetPowerInfo(out int seconds, out int percent);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_OpenURL(string url);
}
