// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Exceptions;
using Phantom.Interop.SDL;

namespace Phantom.Core;

/// <summary>
/// The engine to initialize and shutdown SDL <see cref="SubSystem"/>.
/// </summary>
public sealed class PhantomEngine : IDisposable
{
    internal const string IdentifierProperty = "SDL.app.metadata.identifier";
    internal const string NameProperty = "SDL.app.metadata.name";
    internal const string VersionProperty = "SDL.app.metadata.version";
    internal const string AuthorProperty = "SDL.app.metadata.creator";
    internal const string CopyrightProperty = "SDL.app.metadata.copyright";
    internal const string UrlProperty = "SDL.app.metadata.url";
    internal const string TypeProperty = "SDL.app.metadata.type";

    private static readonly Lock _lock = new();

    private static PhantomEngine? _instance;
    private static SubSystem _subSystems = SubSystem.None;
    private static int _refCount;

    private PhantomEngine()
    {
    }

    /// <summary>
    /// Gets the version of the SDL3 library that Phantom is using.
    /// </summary>
    public static string Version
    {
        get
        {
            int version = SDLNative.SDL_GetVersion();

            int major = version / 1000000;
            int minor = version / 1000 % 1000;
            int patch = version % 1000;

            return $"{major}.{minor}.{patch}";
        }
    }

    /// <summary>
    /// Shutdown the engine and all subsystems.
    /// </summary>
    public void Dispose()
    {
        lock (_lock)
        {
            if (Interlocked.Decrement(ref _refCount) > 0)
                return;

            SDLNative.SDL_QuitSubSystem(_subSystems);
            SDLNative.SDL_Quit();

            Cleanup();
        }
    }

    /// <summary>
    /// Gets the number of milliseconds since the engine initialization.
    /// </summary>
    /// <returns>The timespan in milliseconds.</returns>
    public static TimeSpan GetTicks()
    {
        ulong ticks = SDLNative.SDL_GetTicks();
        return TimeSpan.FromMilliseconds(ticks);
    }

    /// <summary>
    /// Determines whether the specified <see cref="SubSystem"/> is initialized.
    /// </summary>
    /// <param name="subSystem">The subsystem to check.</param>
    /// <returns><see langword="true"/> if the subsystem is initialized; otherwise, <see langword="false"/>.</returns>
    public static bool Has(SubSystem subSystem)
        => (_subSystems & subSystem) == subSystem;

    /// <summary>
    /// Initialize the engine with the specified <see cref="SubSystem"/>.
    /// </summary>
    /// <remarks>
    /// Initialized subsystems are stored and will be uninitialized on <see cref="Dispose" />
    /// or call directly <see cref="QuitSubSystem(SubSystem)"/> to shut down specific subsystems.
    /// You can initialize the same subsystem multiple times. It will only initializes once.
    /// </remarks>
    /// <param name="subSystem">The subsystem to initialize.</param>
    /// <param name="metadata">The metadata of the application.</param>
    /// <returns>An instance of <see cref="PhantomEngine"/>.</returns>
    /// <exception cref="PhantomException">Failed to initialize the subsystem.</exception>
    public static PhantomEngine Init(SubSystem subSystem, AppMetadata? metadata = null)
    {
        lock (_lock)
        {
            _instance ??= new PhantomEngine();

            SetAppMetadata(metadata);
            InternalInit(subSystem);

            return _instance;
        }
    }

    /// <summary>
    /// Initialize specific subsystems.
    /// </summary>
    /// <remarks>
    /// You should call <see cref="Init(SubSystem, AppMetadata)"/> before using this method to initialize the engine.
    /// </remarks>
    /// <param name="subSystem">The subsystem to initialize.</param>
    /// <exception cref="PhantomEngine">Failed to initialize the subsystem.</exception>
    /// <exception cref="InvalidOperationException">The engine is not initialized.</exception>
    public static void InitSubsystem(SubSystem subSystem)
    {
        ThrowIfInstanceNull();

        lock (_lock)
        {
            if (Has(subSystem))
                return;

            SDLNative.SDL_InitSubSystem(subSystem);

            _subSystems |= subSystem;
        }
    }

    /// <summary>
    /// Quit the specified <see cref="SubSystem"/>.
    /// </summary>
    /// <remarks>
    /// <para>You should call <see cref = "Init(SubSystem, AppMetadata)" /> before calling this method to make sure the SDL is initialized.</para>
    /// <para>You can shut down the same subsystem multiple times. It will only shut down once.</para>
    /// You still need to call <see cref="Dispose" /> or <see langword="using"/> even if you close all subsystems.
    /// </remarks>
    /// <param name="subSystem">The subsystem to quit.</param>
    /// <exception cref="InvalidOperationException">The engine is not initialized.</exception>
    public static void QuitSubSystem(SubSystem subSystem)
    {
        ThrowIfInstanceNull();

        lock (_lock)
        {
            if (!Has(subSystem))
                return;

            SDLNative.SDL_QuitSubSystem(subSystem);

            _subSystems &= ~subSystem;
        }
    }

    private static void SetAppMetadata(AppMetadata? metadata)
    {
        if (metadata is null)
            return;

        if (!string.IsNullOrEmpty(metadata.Id))
            SDLNative.SDL_SetAppMetadataProperty(IdentifierProperty, metadata.Id);

        if (!string.IsNullOrEmpty(metadata.Name))
            SDLNative.SDL_SetAppMetadataProperty(NameProperty, metadata.Name);

        if (!string.IsNullOrEmpty(metadata.Version))
            SDLNative.SDL_SetAppMetadataProperty(VersionProperty, metadata.Version);

        if (!string.IsNullOrEmpty(metadata.Author))
            SDLNative.SDL_SetAppMetadataProperty(AuthorProperty, metadata.Author);

        if (metadata.Url is not null)
            SDLNative.SDL_SetAppMetadataProperty(UrlProperty, metadata.Url.ToString());

        if (!string.IsNullOrEmpty(metadata.Type))
            SDLNative.SDL_SetAppMetadataProperty(TypeProperty, metadata.Type);
    }

    private static void InternalInit(SubSystem subSystem)
    {
        if (Has(subSystem))
        {
            Interlocked.Increment(ref _refCount);
            return;
        }

        SDLException.ThrowIf(!SDLNative.SDL_InitSubSystem(subSystem));

        Interlocked.Increment(ref _refCount);

        _subSystems |= subSystem;
    }

    private static void Cleanup()
    {
        _instance = null;
        _subSystems = SubSystem.None;
    }

    private static void ThrowIfInstanceNull()
        => PhantomException.ThrowIfNull(_instance, "The engine is not initialized.");
}
