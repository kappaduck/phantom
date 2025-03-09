// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Exceptions;
using Phantom.Interop.SDL;

namespace Phantom.System;

/// <summary>
/// Represents system preferences such as the current theme, screen saver, etc.
/// </summary>
public static class SystemPreferences
{
    /// <summary>
    /// Gets or sets a value indicating whether the screen saver is enabled.
    /// </summary>
    /// <remarks>
    /// If you disable the screensaver, it is automatically re-enabled when SDL quits.
    /// The screensaver is disabled by default, but this may by changed by SDL_HINT_VIDEO_ALLOW_SCREENSAVER.
    /// </remarks>
    /// <exception cref="SDLException">Failed to enable or disable the screensaver.</exception>
    public static bool ScreenSaver
    {
        get => SDLNative.SDL_ScreenSaverEnabled();
        set
        {
            if (value)
            {
                SDLException.ThrowIfFailed(SDLNative.SDL_EnableScreenSaver());
                return;
            }

            SDLException.ThrowIfFailed(SDLNative.SDL_DisableScreenSaver());
        }
    }

    /// <summary>
    /// Gets the current system theme.
    /// </summary>
    public static SystemTheme Theme => SDLNative.SDL_GetSystemTheme();
}
