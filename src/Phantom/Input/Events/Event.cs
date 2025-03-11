// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.InteropServices;

namespace Phantom.Input.Events;

/// <summary>
/// Represents an event.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct Event
{
    /// <summary>
    /// The type of the event.
    /// </summary>
    [FieldOffset(0)]
    public readonly EventType Type;

    /// <summary>
    /// The display event data.
    /// </summary>
    [FieldOffset(0)]
    public readonly DisplayEvent Display;

    /// <summary>
    /// The keyboard device event data.
    /// </summary>
    [FieldOffset(0)]
    public readonly KeyboardDeviceEvent KeyboardDevice;

    /// <summary>
    /// The keyboard event data.
    /// </summary>
    [FieldOffset(0)]
    public readonly KeyboardEvent Keyboard;

    /// <summary>
    /// The mouse button event data.
    /// </summary>
    [FieldOffset(0)]
    public readonly MouseButtonEvent Mouse;

    /// <summary>
    /// The mouse device event data.
    /// </summary>
    [FieldOffset(0)]
    public readonly MouseDeviceEvent MouseDevice;

    /// <summary>
    /// The mouse motion event data.
    /// </summary>
    [FieldOffset(0)]
    public readonly MouseMotionEvent Motion;

    /// <summary>
    /// The mouse wheel event data.
    /// </summary>
    [FieldOffset(0)]
    public readonly MouseWheelEvent Wheel;

    /// <summary>
    /// The window event data.
    /// </summary>
    [FieldOffset(0)]
    public readonly WindowEvent Window;

    [FieldOffset(0)]
    private unsafe fixed byte _padding[128];
}
