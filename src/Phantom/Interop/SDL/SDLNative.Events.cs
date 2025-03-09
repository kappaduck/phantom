// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Exceptions;
using Phantom.Input.Events;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Phantom.Interop.SDL;

internal static partial class SDLNative
{
    internal static int Peek(Span<Event> events, EventType minType, EventType? maxType = null)
    {
        int peekedEvents = SDL_PeepEvents(events, events.Length, EventAction.Peek, minType, maxType ?? minType);

        SDLException.ThrowIfNegative(peekedEvents);

        return peekedEvents;
    }

    internal static int Push(Span<Event> events)
    {
        int added = SDL_PeepEvents(events, events.Length, EventAction.Add, EventType.None, EventType.LastEvent);

        SDLException.ThrowIfNegative(added);

        return added;
    }

    internal static int Retrieve(Span<Event> events, EventType minType, EventType? maxType = null)
    {
        int retrievedEvents = SDL_PeepEvents(events, events.Length, EventAction.Get, minType, maxType ?? minType);

        SDLException.ThrowIfNegative(retrievedEvents);

        return retrievedEvents;
    }

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_EventEnabled(EventType type);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_FlushEvents(EventType minType, EventType maxType);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_HasEvent(EventType type);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_HasEvents(EventType minType, EventType maxType);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int SDL_PeepEvents(Span<Event> events, int count, EventAction action, EventType minType, EventType maxType);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_PollEvent(out Event e);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_PumpEvents();

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_PushEvent(ref Event e);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_SetEventEnabled(EventType type, [MarshalAs(UnmanagedType.I1)] bool enabled);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_WaitEvent(out Event e);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_WaitEventTimeout(out Event e, int timeout);

    private enum EventAction
    {
        Add = 0,
        Peek = 1,
        Get = 2,
    }
}
