// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Geometry;
using Phantom.Graphics.Drawing;
using Phantom.Graphics.Drawing.BlendModes;
using Phantom.Graphics.Primitives;
using Phantom.Input.Events;
using Phantom.Interop.SDL.Handles;
using Phantom.Interop.SDL.Marshallers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Phantom.Interop.SDL;

internal static partial class SDLNative
{
    internal static unsafe void SetRenderClip(RendererHandle renderer, RectInt? value)
    {
        if (value is null)
        {
            SDL_SetRenderClipRect(renderer, rect: null);
            return;
        }

        RectInt rect = value.Value;
        SDL_SetRenderClipRect(renderer, &rect);
    }

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_ConvertEventToRenderCoordinates(RendererHandle renderer, ref Event e);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial RendererHandle SDL_CreateRenderer(SafeHandle window, string? name = null);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_GetCurrentRenderOutputSize(RendererHandle renderer, out int w, out int h);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial int SDL_GetNumRenderDrivers();

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(SDLOwnedStringMarshaller))]
    internal static partial string SDL_GetRenderDriver(int index);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_GetRenderLogicalPresentationRect(RendererHandle renderer, out Rect rect);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_GetRenderOutputSize(RendererHandle renderer, out int w, out int h);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(SDLOwnedStringMarshaller))]
    internal static partial string SDL_GetRendererName(RendererHandle renderer);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_RenderClear(RendererHandle renderer);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_RenderCoordinatesFromWindow(RendererHandle renderer, float windowX, float windowY, out float x, out float y);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_RenderCoordinatesToWindow(RendererHandle renderer, float x, float y, out float windowX, out float windowY);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_RenderGeometry(RendererHandle renderer, nint texture, ReadOnlySpan<Vertex> vertices, int numVertices, ReadOnlySpan<int> indices, int numIndices);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_RenderPresent(RendererHandle renderer);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void SDL_SetRenderClipRect(RendererHandle renderer, RectInt* rect);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_SetRenderDrawColor(RendererHandle renderer, byte r, byte g, byte b, byte a);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_SetRenderLogicalPresentation(RendererHandle renderer, int w, int h, LogicalPresentation mode);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_SetRenderVSync(RendererHandle renderer, int vsync);
}
