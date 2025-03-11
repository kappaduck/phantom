// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Phantom.Windows;

/// <summary>
/// Represents the options of a window.
/// </summary>
[Flags]
public enum WindowOptions : ulong
{
    /// <summary>
    /// No options to apply.
    /// </summary>
    None = 0x0000000000000000,

    /// <summary>
    /// The window is in fullscreen mode.
    /// </summary>
    Fullscreen = 0x0000000000000001,

    /// <summary>
    /// The window usable with OpenGL context.
    /// </summary>
    OpenGL = 0x0000000000000002,

    /// <summary>
    /// The window is occluded.
    /// </summary>
    Occluded = 0x0000000000000004,

    /// <summary>
    /// The window is hidden.
    /// </summary>
    /// <remarks>
    /// Window is neither mapped on the desktop nor shown in the taskbar/window list; <see cref="Window.Show"/> is required for it to become visible.
    /// </remarks>
    Hidden = 0x0000000000000008,

    /// <summary>
    /// The window has no decoration.
    /// </summary>
    Borderless = 0x0000000000000010,

    /// <summary>
    /// The window can be resized.
    /// </summary>
    Resizable = 0x0000000000000020,

    /// <summary>
    /// The window is minimized.
    /// </summary>
    Minimized = 0x0000000000000040,

    /// <summary>
    /// The window is maximized.
    /// </summary>
    Maximized = 0x0000000000000080,

    /// <summary>
    /// The window has grabbed mouse input.
    /// </summary>
    MouseGrabbed = 0x0000000000000100,

    /// <summary>
    /// The window has input focus.
    /// </summary>
    InputFocus = 0x0000000000000200,

    /// <summary>
    /// The window has mouse focus.
    /// </summary>
    MouseFocus = 0x0000000000000400,

    /// <summary>
    /// The window is not created by SDL.
    /// </summary>
    External = 0x0000000000000800,

    /// <summary>
    /// The window uses high pixel density back buffer if possible.
    /// </summary>
    HighPixelDensity = 0x0000000000002000,

    /// <summary>
    /// The window has mouse captured (unrelated to <see cref="MouseGrabbed"/>).
    /// </summary>
    MouseCapture = 0x0000000000004000,

    /// <summary>
    /// The window has relative mode enabled.
    /// </summary>
    MouseRelativeMode = 0x0000000000008000,

    /// <summary>
    /// The window should always be above others.
    /// </summary>
    AlwaysOnTop = 0x0000000000010000,

    /// <summary>
    /// The window should be treated as a utility window, not showing in the task bar and window list.
    /// </summary>
    Utility = 0x0000000000020000,

    /// <summary>
    /// The window has grabbed keyboard input.
    /// </summary>
    KeyboardGrabbed = 0x0000000000100000,

    /// <summary>
    /// The window usable for Vulkan surface.
    /// </summary>
    Vulkan = 0x0000000010000000,

    /// <summary>
    /// The window has transparent buffer.
    /// </summary>
    Transparent = 0x0000000040000000,

    /// <summary>
    /// The window should not be focusable.
    /// </summary>
    NotFocusable = 0x0000000080000000
}
