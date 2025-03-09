// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Interop.SDL;
using System.Diagnostics.CodeAnalysis;

namespace Phantom.Exceptions;

/// <summary>
/// Represents an exception that is thrown when SDL error occurs.
/// </summary>
/// <remarks>
/// It will uses <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see> to get the error message
/// then use <see href="https://wiki.libsdl.org/SDL3/SDL_ClearError">SDL_ClearError</see> to clear the message.
/// </remarks>
public sealed class SDLException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SDLException"/> class.
    /// </summary>
    public SDLException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SDLException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message from SDL or a custom message.</param>
    public SDLException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SDLException"/> class
    /// with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message from SDL or a custom message.</param>
    /// <param name="innerException">The inner exception.</param>
    public SDLException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    internal static void ThrowIf([DoesNotReturnIf(true)] bool condition)
    {
        if (condition)
            Throw();
    }

    internal static void ThrowIfFailed(bool result) => ThrowIf(!result);

    internal static void ThrowIfNegative(int value) => ThrowIf(int.IsNegative(value));

    internal static void ThrowIfZero(uint value) => ThrowIf(value == 0);

    [DoesNotReturn]
    internal static void Throw()
    {
        string message = SDLNative.SDL_GetError();
        SDLNative.SDL_ClearError();

        throw new SDLException(message);
    }
}
