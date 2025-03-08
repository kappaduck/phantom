// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Interop.SDL;

namespace Phantom.System;

/// <summary>
/// Provides information about a device's power status, e.g laptop.
/// </summary>
public sealed class DevicePower
{
    private DevicePower(int? seconds, int? percentage, PowerState state)
    {
        Seconds = seconds;
        Percentage = percentage;
        State = state;
    }

    /// <summary>
    /// Gets the remaining seconds of power left or <see langword="null"/> if the platform cannot determine it.
    /// </summary>
    public int? Seconds { get; }

    /// <summary>
    /// Gets the remaining percentage of power left or <see langword="null"/> if the platform cannot determine it.
    /// </summary>
    public int? Percentage { get; }

    /// <summary>
    /// Gets the current power state.
    /// </summary>
    public PowerState State { get; }

    /// <summary>
    /// Get the current device power information.
    /// </summary>
    /// <remarks>
    /// You should never take a battery status as absolute truth.
    /// Batteries (especially failing batteries) are delicate hardware,
    /// and the values reported here are best estimates based on what that hardware reports.
    /// It's not uncommon for older batteries to lose stored power much faster than it reports,
    /// or completely drain when reporting it has 20 percent left, etc.
    /// Battery status can change at any time; if you are concerned with power state,
    /// you should call this function frequently, and perhaps ignore changes until they seem to be stable for a few seconds.
    /// It's possible a platform can only report battery percentage or time left but not both.
    /// </remarks>
    /// <returns>The current device power information.</returns>
    public static DevicePower Get()
    {
        PowerState state = SDLNative.SDL_GetPowerInfo(out int seconds, out int percent);

        return new DevicePower(seconds == -1 ? null : seconds, percent == -1 ? null : percent, state);
    }

    /// <summary>
    /// Represents the power state of a device.
    /// </summary>
    public enum PowerState
    {
        /// <summary>
        /// An error occurred while determining the power state.
        /// </summary>
        Error = -1,

        /// <summary>
        /// Cannot determine power state.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Not plugged in, running on the battery.
        /// </summary>
        OnBattery = 1,

        /// <summary>
        /// Plugged in, no battery available.
        /// </summary>
        NoBattery = 2,

        /// <summary>
        /// Plugged in, charging the battery.
        /// </summary>
        Charging = 3,

        /// <summary>
        /// Plugged in, battery is charged.
        /// </summary>
        Charged = 4
    }
}
