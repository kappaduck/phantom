// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Phantom.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an Phantom error occurs.
/// </summary>
public sealed class PhantomException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PhantomException"/> class.
    /// </summary>
    public PhantomException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PhantomException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message from SDL or a custom message.</param>
    public PhantomException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PhantomException"/> class
    /// with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message from SDL or a custom message.</param>
    /// <param name="innerException">The inner exception.</param>
    public PhantomException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    internal static void ThrowIf([DoesNotReturnIf(true)] bool condition, string? message)
    {
        if (condition)
            throw new PhantomException(message);
    }

    internal static void ThrowIfNull<T>([NotNull] T? value, string? message) => ThrowIf(value is null, message);
}
