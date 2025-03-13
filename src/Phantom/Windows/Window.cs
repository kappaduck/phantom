// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Exceptions;
using Phantom.Geometry;
using Phantom.Graphics.Pixels;
using Phantom.Input.Events;
using Phantom.Interop.SDL;
using Phantom.Interop.SDL.Handles;
using Phantom.Video.Displays;
using System.Runtime.InteropServices;

namespace Phantom.Windows;

/// <summary>
/// Represents a simple window with no graphics context.
/// </summary>
/// <remarks>
/// You can inherits this class to implement a graphic window using graphics APIs like OpenGL or Vulkan.
/// </remarks>
public abstract class Window : IDisposable
{
    private WindowHandle _handle;
    private WindowOptions _options;

    private string _title = string.Empty;
    private Vector2Int _position;
    private int _height;
    private int _width;

    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of <see cref="Window"/>.
    /// </summary>
    protected Window()
    {
        _handle = WindowHandle.Zero;
        Opacity = 1.0f;

        Handle = new WindowHandle(_handle);
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Window"/>.
    /// </summary>
    /// <param name="title">The title of the window.</param>
    /// <param name="width">The width of the window.</param>
    /// <param name="height">The height of the window.</param>
    /// <param name="options">The initial options of the window.</param>
    protected Window(string title, int width, int height, WindowOptions options = WindowOptions.None)
    {
        _handle = InternalCreate(title, width, height, options);
        IsOpen = true;

        Handle = new WindowHandle(_handle);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is always on top.
    /// </summary>
    /// <remarks>
    /// If you set this property during the creation of an empty <see cref="Window"/> then using <see cref="Create(string, int, int, WindowOptions)"/>, it will be ignored.
    /// Use <see cref="WindowOptions"/> parameter instead.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window always on top.</exception>
    public bool AlwaysOnTop
    {
        get => (_options & WindowOptions.AlwaysOnTop) == WindowOptions.AlwaysOnTop;
        set
        {
            if (!IsOpen)
                return;

            _options = value ? _options | WindowOptions.AlwaysOnTop : _options & ~WindowOptions.AlwaysOnTop;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowAlwaysOnTop(_handle, value));
        }
    }

    /// <summary>
    /// Gets or sets the aspect ratio of the window's client area.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Minimum or maximum is negative.</exception>
    /// <exception cref="SDLException">An error occurred while setting the window aspect ratio.</exception>
    public (float Minimum, float Maximum) AspectRatio
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value.Minimum);
            ArgumentOutOfRangeException.ThrowIfNegative(value.Maximum);

            field = value;

            if (!IsOpen)
                return;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowAspectRatio(_handle, value.Minimum, value.Maximum));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is borderless.
    /// </summary>
    /// <remarks>
    /// If you set this property during the creation of an empty <see cref="Window"/> then using <see cref="Create(string, int, int, WindowOptions)"/>, it will be ignored.
    /// Use <see cref="WindowOptions"/> parameter instead.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window borderless.</exception>
    public bool Borderless
    {
        get => (_options & WindowOptions.Borderless) == WindowOptions.Borderless;
        set
        {
            if (!IsOpen)
                return;

            _options = value ? _options | WindowOptions.Borderless : _options & ~WindowOptions.Borderless;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowBordered(_handle, !value));
        }
    }

    /// <summary>
    /// Gets the size of the window's borders (decorations) around the client area.
    /// </summary>
    /// <remarks>
    /// <para>If the window is not open or borderless, it will return (0, 0, 0, 0).</para>
    /// <para>
    /// It is possible that failed to get the border size because the window has not yet been decorated by the display server
    /// or the information is not supported.
    /// </para>
    /// </remarks>
    public (int Top, int Left, int Bottom, int Right) BordersSize
    {
        get
        {
            if (!IsOpen || Borderless)
                return default;

            SDLNative.SDL_GetWindowBordersSize(_handle, out int top, out int left, out int bottom, out int right);
            return (top, left, bottom, right);
        }
    }

    /// <summary>
    /// Gets the display associated with the window.
    /// </summary>
    /// <remarks>
    /// If the window is not open, it will return the primary display.
    /// </remarks>
    public Display Display
    {
        get
        {
            if (!IsOpen)
                return Display.GetPrimaryDisplay();

            return Display.GetDisplay(SDLNative.SDL_GetDisplayForWindow(_handle));
        }
    }

    /// <summary>
    /// Gets the content display scale relative to the window's pixel size.
    /// </summary>
    /// <remarks>
    /// <para>If the window is not open, it will return 0.0f.</para>
    /// <para>
    /// This is a combination of the window pixel density and the display content scale,
    /// and is the expected scale for displaying content in this window.
    /// For example, if a 3840x2160 window had a display scale of 2.0,
    /// the user expects the content to take twice as many pixels and be the same physical size
    /// as if it were being displayed in a 1920x1080 window with a display scale of 1.0.
    /// </para>
    /// <para>Conceptually this value corresponds to the scale display setting, and is updated when that setting is changed, or the window moves to a display with a different scale setting.</para>
    /// </remarks>
    public float DisplayScale
    {
        get
        {
            if (!IsOpen)
                return 0.0f;

            return SDLNative.SDL_GetWindowDisplayScale(_handle);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is focusable.
    /// </summary>
    /// <remarks>
    /// If you set this property during the creation of an empty <see cref="WindowOptions"/> then using <see cref="Create(string, int, int, WindowOptions)"/>, it will be ignored.
    /// Use <see cref="WindowOptions"/> parameter instead.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window focusable.</exception>
    public bool Focusable
    {
        get => (_options & ~WindowOptions.NotFocusable) == WindowOptions.NotFocusable;
        set
        {
            if (!IsOpen)
                return;

            _options = value ? _options & ~WindowOptions.NotFocusable : _options | WindowOptions.NotFocusable;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowFocusable(_handle, value));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is fullscreen.
    /// </summary>
    /// <remarks>
    /// If you set this property during the creation of an empty <see cref="Window"/> then using <see cref="Create(string, int, int, WindowOptions)"/>, it will be ignored.
    /// Use <see cref="WindowOptions"/> parameter instead.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while settings the window fullscreen.</exception>
    public bool Fullscreen
    {
        get => (_options & WindowOptions.Fullscreen) == WindowOptions.Fullscreen;
        set
        {
            if (!IsOpen)
                return;

            _options = value ? _options | WindowOptions.Fullscreen : _options & ~WindowOptions.Fullscreen;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowFullscreen(_handle, value));
        }
    }

    /// <summary>
    /// Gets or sets the fullscreen display mode to use when
    /// the window is in <see cref="WindowOptions.Fullscreen"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Setting to <see langword="null"/> will use borderless fullscreen desktop mode,
    /// or one of the fullscreen modes from <see cref="Display.GetFullScreenModes"/> to set an exclusive fullscreen mode.
    /// </para>
    /// <para>
    /// If the window is currently in <see cref="WindowOptions.Fullscreen"/> state, this request is asynchronous on some windowing
    /// systems and the new mode dimensions may not be applied immediately. If an immediate change is needed, call <see cref="Sync"/> to block
    /// until the changes have taken effect.
    /// </para>
    /// <para>
    /// When the new mode takes effect, an <see cref="EventType.WindowResized"/> and/or
    /// an <see cref="EventType.WindowPixelSizeChanged"/> event will be emitted with the new mode dimensions.
    /// </para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window fullscreen mode.</exception>
    public DisplayMode? FullscreenMode
    {
        get;
        set
        {
            field = value;

            if (!IsOpen)
                return;

            SDLException.ThrowIfFailed(SDLNative.SetWindowFullscreenMode(_handle, value));
        }
    }

    /// <summary>
    /// Gets the handle of the window.
    /// </summary>
    /// <remarks>
    /// <para>
    /// You can use this handle to interact with the window using platform-specific APIs.
    /// </para>
    /// <para>
    /// You do not need to dispose of this handle as it is owned by the window.
    /// </para>
    /// </remarks>
    public SafeHandle Handle { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the window has keyboard focus.
    /// </summary>
    public bool HasKeyboardFocus => (_options & WindowOptions.InputFocus) == WindowOptions.InputFocus;

    /// <summary>
    /// Gets a value indicating whether the window has mouse focus.
    /// </summary>
    public bool HasMouseFocus => (_options & WindowOptions.MouseFocus) == WindowOptions.MouseFocus;

    /// <summary>
    /// Gets or sets the height of the window.
    /// </summary>
    /// <remarks>
    /// <para>Setting the height if the window is in <see cref="WindowOptions.Fullscreen"/> or <see cref="WindowOptions.Maximized"/> state will be ignored.</para>
    /// <para>To change the exclusive fullscreen mode dimensions, use <see cref="FullscreenMode"/>.</para>
    /// <para>It will be restricted by <see cref="MinimumSize"/> and <see cref="MaximumSize"/>.</para>
    /// <para>
    /// On some windowing systems this request is asynchronous and the new window state may not have been applied immediately.
    /// If an immediate change is required, call <see cref="Sync"/> to block until the changes have taken effect.
    /// </para>
    /// <para>
    /// When the window size changes, an <see cref="EventType.WindowResized"/> event will be emitted with the new dimensions.
    /// Note that the new dimensions may not be the same as those requested, as the windowing system may impose its own constraints.
    /// (e.g constraining the size of the content area to remain within the usable desktop bounds). Additionally,
    /// as this is just a request, the windowing system can deny the state change.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">Height is less than or equal to 0.</exception>
    /// <exception cref="SDLException">An error occurred while setting the window height.</exception>
    public int Height
    {
        get => _height;
        set
        {
            if (Fullscreen || Maximized)
                return;

            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, 0);

            _height = value;

            if (!IsOpen)
                return;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowSize(_handle, _width, value));
        }
    }

    /// <summary>
    /// Gets the height of the window in pixels.
    /// </summary>
    public int HeightInPixel { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the window is hidden.
    /// </summary>
    public bool Hidden => (_options & WindowOptions.Hidden) == WindowOptions.Hidden;

    /// <summary>
    /// Gets the window's identifier.
    /// </summary>
    /// <remarks>
    /// The identifier is what <see cref="WindowEvent"/> references.
    /// </remarks>
    public uint Id { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the window uses high pixel density.
    /// </summary>
    public bool HighPixelDensity => (_options & WindowOptions.HighPixelDensity) == WindowOptions.HighPixelDensity;

    /// <summary>
    /// Gets a value indicating whether the window is open.
    /// </summary>
    public bool IsOpen
    {
        get => !_handle.IsInvalid && field;
        private set;
    }

    /// <summary>
    /// Gets a value indicating whether the screen keyboard is visible.
    /// </summary>
    public bool IsScreenKeyboardVisible
    {
        get
        {
            if (!IsOpen)
                return false;

            return SDLNative.SDL_ScreenKeyboardShown(_handle);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window has grabbed keyboard input.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If you set this property during the creation of an empty <see cref="Window"/> then using <see cref="Create(string, int, int, WindowOptions)"/>, it will be ignored.
    /// Use <see cref="WindowOptions"/> parameter instead.
    /// </para>
    /// <para>
    /// Keyboard grab enables capture of system keyboard shortcuts like Alt+Tab or the Meta/Super key.
    /// Important to note that not all system keyboard shortcuts can be captured by applications (one example is Ctrl+Alt+Del on Windows).
    /// </para>
    /// <para>
    /// This is primarily intended for specialized applications such as VNC clients or VM frontends. Normal games should not use keyboard grab.
    /// </para>
    /// <para>
    /// When keyboard is enabled, SDL will continue to handle Alt+Tab when
    /// the window is fullscreen to ensure the user is not trapped in your application.
    /// </para>
    /// <para>If the caller enables a grab while another window is currently grabbed, the other window loses its grab in favor of the caller's window.</para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window keyboard grab.</exception>
    public bool KeyboardGrabbed
    {
        get => (_options & WindowOptions.KeyboardGrabbed) == WindowOptions.KeyboardGrabbed;
        set
        {
            if (!IsOpen)
                return;

            _options = value ? _options | WindowOptions.KeyboardGrabbed : _options & ~WindowOptions.KeyboardGrabbed;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowKeyboardGrab(_handle, value));
        }
    }

    /// <summary>
    /// Gets a value indicating whether the window is maximized.
    /// </summary>
    public bool Maximized => (_options & WindowOptions.Maximized) == WindowOptions.Maximized;

    /// <summary>
    /// Gets or sets the maximum size of the window's client area.
    /// </summary>
    /// <remarks>
    /// <para>Setting to (0, 0) removes the maximum size limit.</para>
    /// <para>It will influence the window's size when resizing or using <see cref="Maximize"/>.</para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">Width or height is negative.</exception>
    /// <exception cref="SDLException">An error occurred while setting the window maximum size.</exception>
    public (int Width, int Height) MaximumSize
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value.Width);
            ArgumentOutOfRangeException.ThrowIfNegative(value.Height);

            field = value;

            if (!IsOpen)
                return;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowMaximumSize(_handle, value.Width, value.Height));
        }
    }

    /// <summary>
    /// Gets a value indicating whether the window is minimized.
    /// </summary>
    public bool Minimized => (_options & WindowOptions.Minimized) == WindowOptions.Minimized;

    /// <summary>
    /// Gets or sets the minimum size of the window's client area.
    /// </summary>
    /// <remarks>
    /// <para>Setting to (0, 0) removes the maximum size limit.</para>
    /// <para>It will influence the window's size when resizing.</para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">Width or height is negative.</exception>
    /// <exception cref="SDLException">An error occurred while setting the window minimum size.</exception>
    public (int Width, int Height) MinimumSize
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value.Width);
            ArgumentOutOfRangeException.ThrowIfNegative(value.Height);

            field = value;

            if (!IsOpen)
                return;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowMinimumSize(_handle, value.Width, value.Height));
        }
    }

    /// <summary>
    /// Gets a value indicating whether the window has captured mouse input.
    /// </summary>
    /// <remarks>
    /// It is not related to <see cref="MouseGrabbed"/> (<see cref="WindowOptions.MouseGrabbed"/>).
    /// </remarks>
    public bool MouseCaptured => (_options & WindowOptions.MouseCapture) == WindowOptions.MouseCapture;

    /// <summary>
    /// Gets or sets the confined area of the mouse in the window.
    /// </summary>
    /// <remarks>
    /// <para>Setting to <see langword="null"/> or an empty <see cref="RectInt"/> removes the confined area.</para>
    /// <para>This will not grab the cursor, it only defines the area a cursor is restricted to when the window has mouse focus.</para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window mouse clip.</exception>
    public RectInt? MouseClip
    {
        get;
        set
        {
            field = value;

            if (!IsOpen)
                return;

            SDLException.ThrowIfFailed(SDLNative.SetWindowMouseRect(_handle, value));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window has mouse input grabbed.
    /// </summary>
    /// <remarks>
    /// If you set this property during the creation of an empty <see cref="Window"/> then using <see cref="Create(string, int, int, WindowOptions)"/>, it will be ignored.
    /// Use <see cref="WindowOptions"/> parameter instead.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window mouse grab.</exception>
    public bool MouseGrabbed
    {
        get => (_options & WindowOptions.MouseGrabbed) == WindowOptions.MouseGrabbed;
        set
        {
            if (!IsOpen)
                return;

            _options = value ? _options | WindowOptions.MouseGrabbed : _options & ~WindowOptions.MouseGrabbed;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowMouseGrab(_handle, value));
        }
    }

    /// <summary>
    /// Gets a value indicating whether the window has relative mouse mode enabled.
    /// </summary>
    public bool MouseRelativeMode => (_options & WindowOptions.MouseRelativeMode) == WindowOptions.MouseRelativeMode;

    /// <summary>
    /// Gets a value indicating whether the window is occluded.
    /// </summary>
    public bool Occluded => (_options & WindowOptions.Occluded) == WindowOptions.Occluded;

    /// <summary>
    /// Gets or sets the opacity of the window.
    /// </summary>
    /// <remarks>
    /// <para>The default value is 1.0f.</para>
    /// <para>The opacity value should be in the range 0.0f - 1.0f.</para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">Opacity is negative or greater than 1.0f.</exception>
    /// <exception cref="SDLException">An error occurred while setting the window opacity.</exception>
    public float Opacity
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 1.0f);

            field = value;

            if (!IsOpen)
                return;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowOpacity(_handle, value));
        }
    }

    /// <summary>
    /// Gets the pixel density of the window.
    /// </summary>
    /// <remarks>
    /// <para>If the window is not open, it will return 0.0f.</para>
    /// <para>
    /// This is a ratio of pixel size to window size. For example, if the window is 1920x1080 and it has a
    /// high density back buffer of 3840x2160 pixels, it would have a pixel density of 2.0.
    /// </para>
    /// </remarks>
    public float PixelDensity
    {
        get
        {
            if (!IsOpen)
                return 0.0f;

            return SDLNative.SDL_GetWindowPixelDensity(_handle);
        }
    }

    /// <summary>
    /// Gets the pixel format associated with the window.
    /// </summary>
    public PixelFormat PixelFormat
    {
        get
        {
            if (!IsOpen)
                return PixelFormat.Unknown;

            return SDLNative.SDL_GetWindowPixelFormat(_handle);
        }
    }

    /// <summary>
    /// Gets or sets the position of the window.
    /// </summary>
    /// <remarks>
    /// <para>Setting the position if the window is in <see cref="WindowOptions.Fullscreen"/> or <see cref="WindowOptions.Maximized"/> state will be ignored.</para>
    /// <para>
    /// This can be used to reposition fullscreen desktop windows onto a different display,
    /// however, as exclusive fullscreen windows are locked to a specific display, they can only be repositioned via <see cref="FullscreenMode"/>.
    /// </para>
    /// <para>
    /// On some windowing systems this request is asynchronous and the new window state may not have been applied immediately.
    /// If an immediate change is required, call <see cref="Sync"/> to block until the changes have taken effect.
    /// </para>
    /// <para>
    /// When the window position changes, an <see cref="EventType.WindowMoved"/> event will be emitted with the new coordinates.
    /// Note that the new coordinates may not be the same as those requested, as the windowing system may impose its own constraints.
    /// (e.g constraining the size of the content area to remain within the usable desktop bounds). Additionally,
    /// as this is just a request, the windowing system can deny the state change.
    /// </para>
    /// <para>This is the current position of the window as last reported by the windowing system.</para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window position.</exception>
    public Vector2Int Position
    {
        get => _position;
        set
        {
            if (Fullscreen || Maximized)
                return;

            _position = value;

            if (!IsOpen)
                return;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowPosition(_handle, value.X, value.Y));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is resizable.
    /// </summary>
    /// <remarks>
    /// If you set this property during the creation of an empty <see cref="Window"/> then using <see cref="Create(string, int, int, WindowOptions)"/>, it will be ignored.
    /// Use <see cref="WindowOptions"/> parameter instead.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window resizable.</exception>
    public bool Resizable
    {
        get => (_options & WindowOptions.Resizable) == WindowOptions.Resizable;
        set
        {
            if (!IsOpen)
                return;

            _options = value ? _options | WindowOptions.Resizable : _options & ~WindowOptions.Resizable;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowResizable(_handle, value));
        }
    }

    /// <summary>
    /// Gets the safe area for the window.
    /// </summary>
    /// <remarks>
    /// Some devices have portions of the screen which are partially obscured or not interactive,
    /// possibly due to on-screen controls, curved edges, camera notches, TV over scan, etc.
    /// This provides the area of the window which is safe to have interactable content.
    /// You should continue rendering into the rest of the window,
    /// but it should not contain visually important or interactable content.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while getting the window safe area.</exception>
    public RectInt SafeArea
    {
        get
        {
            if (!IsOpen)
                return default;

            SDLException.ThrowIfFailed(SDLNative.SDL_GetWindowSafeArea(_handle, out RectInt area));
            return area;
        }
    }

    /// <summary>
    /// Gets or sets the title of the window.
    /// </summary>
    /// <exception cref="SDLException">An error occurred while setting the window title.</exception>
    public string Title
    {
        get => _title;
        set
        {
            _title = value;

            if (!IsOpen)
                return;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowTitle(_handle, value));
        }
    }

    /// <summary>
    /// Gets or sets the width of the window.
    /// </summary>
    /// <remarks>
    /// <para>Setting the width if the window is in <see cref="WindowOptions.Fullscreen"/> or <see cref="WindowOptions.Maximized"/> state will be ignored.</para>
    /// <para>To change the exclusive fullscreen mode dimensions, use <see cref="FullscreenMode"/>.</para>
    /// <para>It will be restricted by <see cref="MinimumSize"/> and <see cref="MaximumSize"/>.</para>
    /// <para>
    /// On some windowing systems this request is asynchronous and the new window state may not have been applied immediately.
    /// If an immediate change is required, call <see cref="Sync"/> to block until the changes have taken effect.
    /// </para>
    /// <para>
    /// When the window size changes, an <see cref="EventType.WindowResized"/> event will be emitted with the new dimensions.
    /// Note that the new dimensions may not be the same as those requested, as the windowing system may impose its own constraints.
    /// (e.g constraining the size of the content area to remain within the usable desktop bounds). Additionally,
    /// as this is just a request, the windowing system can deny the state change.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">Width is less than or equal to 0.</exception>
    /// <exception cref="SDLException">An error occurred while setting the window width.</exception>
    public int Width
    {
        get => _width;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, 0);

            _width = value;

            if (!IsOpen)
                return;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowSize(_handle, value, _height));
        }
    }

    /// <summary>
    /// Gets the width of the window in pixels.
    /// </summary>
    public int WidthInPixel { get; private set; }

    /// <summary>
    /// Closes the window.
    /// </summary>
    /// <remarks>
    /// If the window is already closed or not created, It does nothing.
    /// </remarks>
    public void Close()
    {
        if (!IsOpen)
            return;

        IsOpen = false;
    }

    /// <summary>
    /// Creates the window.
    /// </summary>
    /// <remarks>
    /// If the window is already open, it does nothing.
    /// </remarks>
    /// <param name="title">The title of the window.</param>
    /// <param name="width">The width of the window.</param>
    /// <param name="height">The height of the window.</param>
    /// <param name="options">The initial state of the window.</param>
    /// <exception cref="SDLException">An error occurred while creating the window.</exception>
    public void Create(string title, int width, int height, WindowOptions options = WindowOptions.None)
    {
        if (IsOpen)
            return;

        _handle = InternalCreate(title, width, height, options);
        Handle = new WindowHandle(_handle);

        SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowAspectRatio(_handle, AspectRatio.Minimum, AspectRatio.Maximum));
        SDLException.ThrowIfFailed(SDLNative.SetWindowFullscreenMode(_handle, FullscreenMode));
        SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowMaximumSize(_handle, MaximumSize.Width, MaximumSize.Height));
        SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowMinimumSize(_handle, MinimumSize.Width, MinimumSize.Height));
        SDLException.ThrowIfFailed(SDLNative.SetWindowMouseRect(_handle, MouseClip));
        SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowOpacity(_handle, Opacity));

        IsOpen = true;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Request the window to demand attention from the user.
    /// </summary>
    /// <param name="operation">The operation to perform.</param>
    /// <exception cref="SDLException">An error occurred while flashing the window.</exception>
    public void Flash(FlashOperation operation)
    {
        if (!IsOpen)
            return;

        SDLException.ThrowIfFailed(SDLNative.SDL_FlashWindow(_handle, operation));
    }

    /// <summary>
    /// Hides the window. It can be shown again with <see cref="Show"/>.
    /// </summary>
    /// <exception cref="SDLException">An error occurred while hiding the window.</exception>
    public void Hide()
    {
        if (!IsOpen)
            return;

        SDLException.ThrowIfFailed(SDLNative.SDL_HideWindow(_handle));

        _options |= WindowOptions.Hidden;
    }

    /// <summary>
    /// Request that the window be made as large as possible.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Non-resizable windows can't be maximized. The window must have the <see cref="WindowOptions.Resizable"/> state set.
    /// </para>
    /// <para>
    /// On some windowing systems this request is asynchronous and the new window state may not have been applied immediately.
    /// If an immediate change is required, call <see cref="Sync"/> to block until the changes have taken effect.
    /// </para>
    /// <para>
    /// When the window state changes, an <see cref="EventType.WindowMaximized"/> event will be emitted.
    /// Note that, as this is just a request, the windowing system can deny the state change.
    /// </para>
    /// <para>
    /// When maximizing a window, whether the constraints set via <see cref="MaximumSize"/> are honored depends on the policy of the window manager.
    /// Win32 enforce the constraints when maximizing, while X11 and Wayland window managers may vary.
    /// </para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while maximizing the window.</exception>
    public void Maximize()
    {
        if (!IsOpen || Maximized || !Resizable)
            return;

        SDLException.ThrowIfFailed(SDLNative.SDL_MaximizeWindow(_handle));

        _options &= ~WindowOptions.Minimized;
        _options |= WindowOptions.Maximized;
    }

    /// <summary>
    /// Request that the window be minimized to an iconic representation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the window is in <see cref="WindowOptions.Fullscreen"/> state, it will has no direct effect.
    /// It may alter the state the window is restored to when leaving fullscreen.
    /// </para>
    /// <para>
    /// On some windowing systems this request is asynchronous and the new window state may not have been applied immediately.
    /// If an immediate change is required, call <see cref="Sync"/> to block until the changes have taken effect.
    /// </para>
    /// <para>
    /// When the window state changes, an <see cref="EventType.WindowMinimized"/> event will be emitted.
    /// Note that, as this is just a request, the windowing system can deny the state change.
    /// </para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while minimizing the window.</exception>
    public void Minimize()
    {
        if (!IsOpen || Minimized)
            return;

        SDLException.ThrowIfFailed(SDLNative.SDL_MinimizeWindow(_handle));

        _options &= ~WindowOptions.Maximized;
        _options |= WindowOptions.Minimized;
    }

    /// <summary>
    /// Polls for currently pending events.
    /// </summary>
    /// <remarks>
    /// <para>Some events are processed internally by the window.</para>
    /// Will return <see langword="false"/> and empty <see cref="Event"/> if the window is not open.
    /// </remarks>
    /// <param name="e">The next filled event from the queue.</param>
    /// <returns><see langword="true"/> if this got an event or <see langword="false"/> if there are none available.</returns>
    public bool Poll(out Event e)
    {
        if (!IsOpen)
        {
            e = default;
            return false;
        }

        bool hasEvent = EventManager.Poll(out e);

        if (!IsOpen || e.Window.Id != Id)
            return hasEvent;

        if (e.Type == EventType.WindowExposed)
            _options &= ~WindowOptions.Occluded;

        if (e.Type == EventType.WindowOccluded)
            _options |= WindowOptions.Occluded;

        if (e.Type == EventType.WindowResized)
        {
            _width = e.Window.Data1;
            _height = e.Window.Data2;
        }

        if (e.Type == EventType.WindowPixelSizeChanged)
        {
            WidthInPixel = e.Window.Data1;
            HeightInPixel = e.Window.Data2;
        }

        if (e.Type == EventType.WindowMoved)
        {
            _position.X = e.Window.Data1;
            _position.Y = e.Window.Data2;
        }

        if (e.Type == EventType.MouseEnter)
            _options |= WindowOptions.MouseFocus;

        if (e.Type == EventType.MouseLeave)
            _options &= ~WindowOptions.MouseFocus;

        if (e.Type == EventType.FocusGained)
            _options |= WindowOptions.InputFocus;

        if (e.Type == EventType.FocusLost)
            _options &= ~WindowOptions.InputFocus;

        if (e.Type == EventType.WindowRestored)
            _options &= ~(WindowOptions.Minimized | WindowOptions.Maximized);

        return hasEvent;
    }

    /// <summary>
    /// Request that the window be raised above other windows and gain the input focus.
    /// </summary>
    /// <remarks>
    /// The result of this request is subject to desktop window manager policy, particularly if raising
    /// the requested window would result in stealing focus from another application.
    /// If the window is successfully raised and gains input focus,
    /// an <see cref="EventType.FocusGained"/> event will be emitted,
    /// and the window will have <see cref="WindowOptions.InputFocus"/> state set.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while raising the window.</exception>
    public void Raise()
    {
        if (!IsOpen)
            return;

        SDLException.ThrowIfFailed(SDLNative.SDL_RaiseWindow(_handle));

        _options |= WindowOptions.InputFocus;
    }

    /// <summary>
    /// Request that the size and position of a minimized or maximized window be restored.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the window is in <see cref="WindowOptions.Fullscreen"/> state, it will has no direct effect.
    /// It may alter the state the window is restored to when leaving fullscreen.
    /// </para>
    /// <para>
    /// On some windowing systems this request is asynchronous and the new window state may not have been applied immediately.
    /// If an immediate change is required, call <see cref="Sync"/> to block until the changes have taken effect.
    /// </para>
    /// <para>
    /// When the window state changes, an <see cref="EventType.WindowRestored"/> event will be emitted.
    /// Note that, as this is just a request, the windowing system can deny the state change.
    /// </para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while restoring the window.</exception>
    public void Restore()
    {
        if (!IsOpen)
            return;

        SDLException.ThrowIfFailed(SDLNative.SDL_RestoreWindow(_handle));
    }

    /// <summary>
    /// Show the window.
    /// </summary>
    /// <remarks>
    /// It's only the way to show a window that has been hidden
    /// with <see cref="Hide"/> or using <see cref="WindowOptions.Hidden"/> state.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while showing the window.</exception>
    public void Show()
    {
        if (!IsOpen)
            return;

        SDLException.ThrowIfFailed(SDLNative.SDL_ShowWindow(_handle));

        _options &= ~WindowOptions.Hidden;
    }

    /// <summary>
    /// Block until any pending window state is finalized.
    /// </summary>
    /// <remarks>
    /// <para>On windowing systems where changes are immediate, this does nothing.</para>
    /// <para>
    /// On asynchronous windowing systems, this acts as a synchronization barrier for pending window state.
    /// It will attempt to wait until any pending window state has been applied and is guaranteed to return within finite time.
    /// Note that for how long it can potentially block depends on the underlying window system,
    /// as window state changes may involve somewhat lengthy animations that must complete before the window is in its final requested state.
    /// </para>
    /// </remarks>
    /// <exception cref="SDLException">Failed to sync the window.</exception>
    public void Sync()
    {
        if (!IsOpen)
            return;

        SDLException.ThrowIfFailed(SDLNative.SDL_SyncWindow(_handle));
    }

    /// <summary>
    /// Move the mouse cursor to the given position withing the window.
    /// </summary>
    /// <remarks>
    /// <para>It generates a <see cref="EventType.MouseMotion"/> event if relative mode is not enabled.</para>
    /// <para>It will not move the mouse when used over Microsoft Remote Desktop.</para>
    /// </remarks>
    /// <param name="x">The x-coordinate within the window.</param>
    /// <param name="y">The y-coordinate within the window.</param>
    public void WarpMouse(float x, float y) => SDLNative.SDL_WarpMouseInWindow(_handle, x, y);

    /// <summary>
    /// Move the mouse cursor to the given position withing the window.
    /// </summary>
    /// <remarks>
    /// <para>It generates a <see cref="EventType.MouseMotion"/> event if relative mode is not enabled.</para>
    /// <para>It will not move the mouse when used over Microsoft Remote Desktop.</para>
    /// </remarks>
    /// <param name="position">The position within the window.</param>
    public void WarpMouse(Vector2 position) => WarpMouse(position.X, position.Y);

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="Window"/> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            _handle.Dispose();

        _disposed = true;
    }

    private WindowHandle InternalCreate(string title, int width, int height, WindowOptions options)
    {
        WindowHandle handle = SDLNative.SDL_CreateWindow(title, width, height, options);
        SDLException.ThrowIf(handle.IsInvalid);

        Id = SDLNative.SDL_GetWindowID(handle);
        SDLException.ThrowIfZero(Id);

        _title = title;
        _width = width;
        _height = height;
        _options = options;

        if (_position.IsZero)
        {
            SDLException.ThrowIfFailed(SDLNative.SDL_GetWindowPosition(handle, out int x, out int y));
            _position = new Vector2Int(x, y);
        }
        else
        {
            SDLException.ThrowIfFailed(SDLNative.SDL_SetWindowPosition(handle, _position.X, _position.Y));
        }

        return handle;
    }
}
