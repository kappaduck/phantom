// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Phantom.Windows;

/// <summary>
/// The operation to perform on a window flash state.
/// </summary>
public enum FlashOperation
{
    /// <summary>
    /// Cancel any window flash state.
    /// </summary>
    Cancel = 0,

    /// <summary>
    /// Flash the window briefly.
    /// </summary>
    Briefly = 1,

    /// <summary>
    /// Flash the window until it receives focus.
    /// </summary>
    UntilFocused = 2,
}
