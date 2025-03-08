// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Interop.SDL.Marshallers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Phantom.Interop.SDL;

internal static partial class SDLNative
{
    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial uint SDL_CreateProperties();

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_DestroyProperties(uint propertiesId);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_GetBooleanProperty(uint propertiesId, string name, [MarshalAs(UnmanagedType.I1)] bool defaultValue);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial float SDL_GetFloatProperty(uint propertiesId, string name, float defaultValue);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial long SDL_GetNumberProperty(uint propertiesId, string name, long defaultValue);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial nint SDL_GetPointerProperty(uint propertiesId, string name, nint defaultValue);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(SDLOwnedStringMarshaller))]
    internal static partial string SDL_GetStringProperty(uint propertiesId, string name, string defaultValue);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_SetAppMetadataProperty(string name, string value);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_SetBooleanProperty(uint propertiesId, string name, [MarshalAs(UnmanagedType.I1)] bool value);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_SetFloatProperty(uint propertiesId, string name, float value);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_SetNumberProperty(uint propertiesId, string name, long value);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_SetStringProperty(uint propertiesId, string name, string value);
}
