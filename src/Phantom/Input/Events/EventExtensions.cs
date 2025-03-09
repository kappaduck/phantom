// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Phantom.Input.Events;

/// <summary>
/// Provides extension methods for <see cref="Event"/>.
/// </summary>
public static class EventExtensions
{
    /// <summary>
    /// Determines whether the event is a mouse button down event with the specified button.
    /// </summary>
    /// <param name="e">The SDL event.</param>
    /// <param name="button">The button to compare.</param>
    /// <returns><see langword="true"/> if the event is a mouse button down event with the specified button; otherwise, <see langword="false"/>.</returns>
    public static bool IsMouseButtonDown(this Event e, Mouse.Button button)
        => e.Type is EventType.MouseButtonDown && e.Mouse.Button == button;

    /// <summary>
    /// Determines whether the event is a mouse button up event with the specified button.
    /// </summary>
    /// <param name="e">The SDL event.</param>
    /// <param name="button">The button to compare.</param>
    /// <returns><see langword="true"/> if the event is a mouse button up event with the specified button; otherwise, <see langword="false"/>.</returns>
    public static bool IsMouseButtonUp(this Event e, Mouse.Button button)
        => e.Type is EventType.MouseButtonUp && e.Mouse.Button == button;
}
