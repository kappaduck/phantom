// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Exceptions;
using Phantom.Interop.SDL;

namespace Phantom.Core;

internal sealed class Properties : IDisposable
{
    private readonly uint _id;

    internal Properties()
    {
        _id = SDLNative.SDL_CreateProperties();

        SDLException.ThrowIfZero(_id);
    }

    public void Dispose() => SDLNative.SDL_DestroyProperties(_id);

    internal T Get<T>(string name, T defaultValue) => Get(_id, name, defaultValue);

    internal void Set<T>(string name, T value) => Set(_id, name, value);

    internal static T Get<T>(uint propertiesId, string name, T defaultValue)
    {
        return defaultValue switch
        {
            bool boolean => (T)(object)SDLNative.SDL_GetBooleanProperty(propertiesId, name, boolean),
            float floating => (T)(object)SDLNative.SDL_GetFloatProperty(propertiesId, name, floating),
            long number => (T)(object)SDLNative.SDL_GetNumberProperty(propertiesId, name, number),
            string str => (T)(object)SDLNative.SDL_GetStringProperty(propertiesId, name, str),
            nint pointer => (T)(object)SDLNative.SDL_GetPointerProperty(propertiesId, name, pointer),
            _ => throw new NotSupportedException($"The type {typeof(T)} is not supported.")
        };
    }

    internal static void Set<T>(uint propertiesId, string name, T value)
    {
        bool isSet = value switch
        {
            bool boolean => SDLNative.SDL_SetBooleanProperty(propertiesId, name, boolean),
            float floating => SDLNative.SDL_SetFloatProperty(propertiesId, name, floating),
            long number => SDLNative.SDL_SetNumberProperty(propertiesId, name, number),
            string str => SDLNative.SDL_SetStringProperty(propertiesId, name, str),
            _ => throw new NotSupportedException($"The type {typeof(T)} is not supported.")
        };

        SDLException.ThrowIf(!isSet);
    }
}
