// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Exceptions;
using Phantom.Interop.SDL;

namespace Phantom.Input.Events;

/// <summary>
/// Global event functions.
/// </summary>
public static class EventManager
{
    /// <summary>
    /// Disable events of a specific type.
    /// </summary>
    /// <param name="type">The event type to disable.</param>
    public static void Disable(EventType type) => SDLNative.SDL_SetEventEnabled(type, enabled: false);

    /// <summary>
    /// enable events of a specific type.
    /// </summary>
    /// <param name="type">The event type to enable.</param>
    public static void Enable(EventType type) => SDLNative.SDL_SetEventEnabled(type, enabled: true);

    /// <summary>
    /// Clear events of a range of types from the event queue.
    /// </summary>
    /// <param name="type">The low end of event type to be cleared, inclusive.</param>
    /// <param name="maxType">the high end of event type to be cleared, inclusive. If <see langword="null"/> then minType is used.</param>
    /// <remarks>
    /// This will unconditionally remove any events from the queue that are in the range of <paramref name="type"/> to <paramref name="maxType"/>, inclusive.
    /// It's also normal to just ignore events you don't care about in your event loop without calling this function.
    /// This function only affects currently queued events. If you want to make sure that all pending OS events are flushed,
    /// you can call <see cref="Pump"/> on the main thread immediately before the flush call.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="type"/> is greater than <paramref name="maxType"/>.</exception>
    public static void Flush(EventType type, EventType? maxType = null)
    {
        ThrowIfGreaterThan(type, maxType);

        SDLNative.SDL_FlushEvents(type, maxType ?? type);
    }

    /// <summary>
    /// Check for the existence of a certain event types in the event queue.
    /// </summary>
    /// <param name="type">The type to check if exists.</param>
    /// <returns><see langword="true"/> if events matching type are present, or <see langword="false"/> if events matching type are not present.</returns>
    public static bool Has(EventType type) => SDLNative.SDL_HasEvent(type);

    /// <summary>
    /// Check for the existence of certain event types in the event queue.
    /// </summary>
    /// <param name="minType">The low end of event type to be cleared, inclusive.</param>
    /// <param name="maxType">the high end of event type to be cleared, inclusive.</param>
    /// <returns><see langword="true"/> if events with type >= <paramref name="minType"/> and &lt;= <paramref name="maxType"/> are present, or <see langword="false"/> if not.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minType"/> is greater than <paramref name="maxType"/>.</exception>
    public static bool Has(EventType minType, EventType maxType)
    {
        ThrowIfGreaterThan(minType, maxType);

        return SDLNative.SDL_HasEvents(minType, maxType);
    }

    /// <summary>
    /// Query the state of processing events by type.
    /// </summary>
    /// <param name="type">The event type to check.</param>
    /// <returns><see langword="true"/> if the event is being processed, false otherwise.</returns>
    public static bool IsEnabled(EventType type) => SDLNative.SDL_EventEnabled(type);

    /// <summary>
    /// Retrieve events from the event queue without removing them.
    /// </summary>
    /// <param name="events">Destination buffer for the events at the front of the event queue.</param>
    /// <param name="minType">The low end of event type to be cleared, inclusive.</param>
    /// <param name="maxType">the high end of event type to be cleared, inclusive. If <see langword="null"/> then minType is used.</param>
    /// <returns>The number of events actually stored.</returns>
    /// <remarks>
    /// You may have to call <see cref="Pump"/> before calling this function. Otherwise, the events may not be ready to be filtered.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minType"/> is greater than <paramref name="maxType"/>.</exception>
    /// <exception cref="SDLException">Failed while peeping events.</exception>
    public static int Peek(Span<Event> events, EventType minType, EventType? maxType = null)
    {
        ThrowIfGreaterThan(minType, maxType);
        return SDLNative.Peek(events, minType, maxType);
    }

    /// <summary>
    /// Polls for currently pending events.
    /// </summary>
    /// <param name="e">The next filled event from the queue.</param>
    /// <returns><see langword="true"/> if this got an event or <see langword="false"/> if there are none available.</returns>
    public static bool Poll(out Event e) => SDLNative.SDL_PollEvent(out e);

    /// <summary>
    /// Pump the event loop, gathering events from the input devices.
    /// </summary>
    /// <remarks>
    /// This function updates the event queue and internal input device state.
    /// Gathers all the pending input information from devices and places it in the event queue.
    /// Without calls to <see cref="Pump"/> no events would ever be placed on the queue.
    /// Often the need for calls to <see cref="Pump"/> is hidden from the user since <see cref="Poll(out Event)"/> and <see cref="Wait(out Event, TimeSpan?)"/>
    /// implicitly call <see cref="Pump"/>. However, if you are not polling or waiting for events(e.g.you are filtering them),
    /// then you must call <see cref="Pump"/> to force an event queue update.
    /// </remarks>
    public static void Pump() => SDLNative.SDL_PumpEvents();

    /// <summary>
    /// Add an event to the event queue.
    /// </summary>
    /// <param name="e">The event to push to the event queue.</param>
    /// <returns><see langword="true"/> on success, <see langword="false"/> if the event was filtered or on failure.
    /// A common reason for error is the event queue being full.
    /// </returns>
    public static bool Push(ref Event e) => SDLNative.SDL_PushEvent(ref e);

    /// <summary>
    /// Add events to the back of the event queue.
    /// </summary>
    /// <param name="events">Added events to the event queue.</param>
    /// <returns>The number of events actually added.</returns>
    /// <exception cref="SDLException">Failed while added events.</exception>"
    public static int Push(Span<Event> events) => SDLNative.Push(events);

    /// <summary>
    /// Retrieve events from the event queue by removing them.
    /// </summary>
    /// <param name="events">Destination buffer for the retrieved events.</param>
    /// <param name="minType">The low end of event type to be cleared, inclusive.</param>
    /// <param name="maxType">the high end of event type to be cleared, inclusive. If <see langword="null"/> then minType is used.</param>
    /// <returns>The number of events actually stored.</returns>
    /// <remarks>
    /// You may have to call <see cref="Pump"/> before calling this function. Otherwise, the events may not be ready to be filtered.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minType"/> is greater than <paramref name="maxType"/>.</exception>
    /// <exception cref="SDLException">Failed while retrieving events.</exception>
    public static int? Retrieve(Span<Event> events, EventType minType, EventType? maxType = null)
    {
        ThrowIfGreaterThan(minType, maxType);
        return SDLNative.Retrieve(events, minType, maxType);
    }

    /// <summary>
    /// Wait until the specified timeout (in milliseconds) for the next available event.
    /// </summary>
    /// <param name="e">The next filled event from the queue.</param>
    /// <param name="timeSpan">The maximum time (milliseconds) to wait for the next available event.</param>
    /// <returns><see langword="true"/> if this got an event or <see langword="false"/> if the timeout elapsed without any events available.</returns>
    /// <remarks>
    /// The timeout is not guaranteed, the actual wait time could be longer due to system scheduling.
    /// </remarks>
    public static bool Wait(out Event e, TimeSpan? timeSpan = null)
    {
        if (timeSpan is null || timeSpan == Timeout.InfiniteTimeSpan)
            return SDLNative.SDL_WaitEvent(out e);

        return SDLNative.SDL_WaitEventTimeout(out e, (int)timeSpan.Value.TotalMilliseconds);
    }

    private static void ThrowIfGreaterThan(EventType minType, EventType? maxType)
    {
        if (minType > maxType)
            throw new ArgumentOutOfRangeException(nameof(minType), "The minimum type must be less than or equal to the maximum type.");
    }
}
