// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Exceptions;
using Phantom.Geometry;
using Phantom.Graphics.Drawing;
using Phantom.Graphics.Primitives;
using Phantom.Input.Events;
using Phantom.Interop.SDL;
using Phantom.Interop.SDL.Handles;
using Phantom.Windows;
using System.Drawing;

namespace Phantom.Graphics.Rendering;

/// <summary>
/// Represents a window that can be used to render 2D graphics using SDL Renderer API.
/// </summary>
public sealed class RenderWindow : Window, IRenderTarget
{
    private RendererHandle _renderer = RendererHandle.Zero;

    /// <summary>
    /// Creates an empty window.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Window.Create(string, int, int, WindowOptions)"/> to create a window.
    /// </remarks>
    public RenderWindow() => CreateRenderer();

    /// <summary>
    /// Creates a new window with the specified title, width, height, and state.
    /// </summary>
    /// <param name="title">The title of the window.</param>
    /// <param name="width">The width of the window.</param>
    /// <param name="height">The height of the window.</param>
    /// <param name="options">The state of the window.</param>
    public RenderWindow(string title, int width, int height, WindowOptions options = WindowOptions.None) : base(title, width, height, options)
        => CreateRenderer();

    /// <summary>
    /// Gets the current output size in pixels of a rendering context.
    /// </summary>
    /// <remarks>
    /// If a rendering target is active, this will return the size of the rendering target in pixels,
    /// otherwise if a logical size is set, it will return the logical size,
    /// otherwise it will return the value of <see cref="OutputSize"/>.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while getting the current renderer output size.</exception>
    public (int Width, int Height) CurrentOutputSize
    {
        get
        {
            if (_renderer.IsInvalid)
                return (0, 0);

            SDLException.ThrowIfFailed(SDLNative.SDL_GetCurrentRenderOutputSize(_renderer, out int w, out int h));
            return (w, h);
        }
    }

    /// <summary>
    /// Gets the output size in pixels of a rendering context.
    /// </summary>
    /// <remarks>
    /// It return the true output size in pixels, ignoring any render targets or logical size and presentation.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while getting the renderer output size.</exception>
    public (int Width, int Height) OutputSize
    {
        get
        {
            if (_renderer.IsInvalid)
                return (0, 0);

            SDLException.ThrowIfFailed(SDLNative.SDL_GetRenderOutputSize(_renderer, out int w, out int h));
            return (w, h);
        }
    }

    /// <summary>
    /// Gets or sets a device independent resolution and presentation mode for rendering.
    /// </summary>
    /// <remarks>
    /// It sets the width and height of the logical rendering output.
    /// The renderer will act as if the window is always the requested dimensions, scaling to the actual window resolution as necessary.
    /// This can be useful for games that expect a fixed size, but would like to scale the output to whatever is available,
    /// regardless of how a user resizes a window, or if the display is high DPI.
    /// You can disable logical coordinates by setting the mode to <see cref="LogicalPresentation.Disabled"/>,
    /// and in that case you get the full pixel resolution of the output window;
    /// it is safe to toggle logical presentation during the rendering of a frame: perhaps most of the rendering is done to specific dimensions
    /// but to make fonts look sharp, the app turns off logical presentation while drawing text.
    /// Letterboxing will only happen if logical presentation is enabled during <see cref="Render"/>; be sure to reenable it first if you were using it.
    /// You can convert coordinates in an event into rendering coordinates using <see cref="MapEventToCoordinates(ref Event)"/> or <see cref="MapPixelsToCoordinates(Vector2)"/>.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">The width or height is negative.</exception>
    /// <exception cref="SDLException">An error occurred while setting the logical presentation.</exception>
    public (int Width, int Height, LogicalPresentation Mode) Presentation
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value.Width);
            ArgumentOutOfRangeException.ThrowIfNegative(value.Height);

            field = value;

            if (_renderer.IsInvalid)
                return;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetRenderLogicalPresentation(_renderer, value.Width, value.Height, value.Mode));
        }
    }

    /// <summary>
    /// Gets the final presentation rectangle for rendering.
    /// </summary>
    /// <remarks>
    /// It returns the calculated rectangle used for logical presentation, based on the presentation
    /// mode and output size. If logical presentation is <see cref="LogicalPresentation.Disabled"/>, it will fill
    /// the rectangle with the output size, in pixels.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while getting the renderer presentation rectangle.</exception>
    public Rect PresentationRectangle
    {
        get
        {
            if (_renderer.IsInvalid)
                return default;

            SDLException.ThrowIfFailed(SDLNative.SDL_GetRenderLogicalPresentationRect(_renderer, out Rect rectangle));
            return rectangle;
        }
    }

    /// <summary>
    /// Gets the name of the rendering driver used by the renderer.
    /// </summary>
    /// <remarks>
    /// A list of available renderers can be obtained by calling <see cref="Renderer.GetAll"/>.
    /// </remarks>
    public string? RendererName
    {
        get
        {
            if (_renderer.IsInvalid)
                return null;

            return field ?? SDLNative.SDL_GetRendererName(_renderer);
        }
        init;
    }

    /// <summary>
    /// Gets or sets the vertical synchronization (VSync) of the renderer.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When a renderer is created, VSync defaults to <see cref="VSync.Disabled"/> which means that VSync is disabled.
    /// </para>
    /// <para>
    /// The value can be 1 to synchronize present with every vertical refresh, 2 to synchronize present with every other vertical refresh, and so on.
    /// <see cref="VSync.Adaptive"/> can be used for adaptive VSync or <see cref="VSync.Disabled"/> to disable. Not every value is supported by every driver.
    /// </para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the renderer VSync.</exception>
    public int VSync
    {
        get;
        set
        {
            if (field == value)
                return;

            field = value;

            if (_renderer.IsInvalid)
                return;

            SDLException.ThrowIfFailed(SDLNative.SDL_SetRenderVSync(_renderer, value));
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// The render target is cleared with a black color.
    /// If you want to clear the render target with a different color, use <see cref="Clear(Color)"/> instead.
    /// </remarks>
    public void Clear() => Clear(Color.Black);

    /// <inheritdoc/>
    public void Clear(Color color)
    {
        SDLNative.SDL_SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);
        SDLNative.SDL_RenderClear(_renderer);
    }

    /// <inheritdoc/>
    public void Draw(ReadOnlySpan<Vertex> vertices) => Draw(vertices, RenderState.Default);

    /// <inheritdoc/>
    public void Draw(ReadOnlySpan<Vertex> vertices, in RenderState state)
    {
        ApplyState(state);
        SDLNative.SDL_RenderGeometry(_renderer, nint.Zero, vertices, vertices.Length, [], 0);
    }

    /// <inheritdoc/>
    public void Draw(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<int> indices) => Draw(vertices, indices, RenderState.Default);

    /// <inheritdoc/>
    public void Draw(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<int> indices, in RenderState state)
    {
        ApplyState(state);
        SDLNative.SDL_RenderGeometry(_renderer, nint.Zero, vertices, vertices.Length, indices, indices.Length);
    }

    /// <inheritdoc/>
    public void MapEventToCoordinates(ref Event e)
        => SDLNative.SDL_ConvertEventToRenderCoordinates(_renderer, ref e);

    /// <inheritdoc/>
    public Vector2 MapPixelsToCoordinates(Vector2 point)
    {
        SDLNative.SDL_RenderCoordinatesFromWindow(_renderer, point.X, point.Y, out float x, out float y);
        return new Vector2(x, y);
    }

    /// <inheritdoc/>
    public Vector2 MapCoordinatesToPixels(Vector2 point)
    {
        SDLNative.SDL_RenderCoordinatesToWindow(_renderer, point.X, point.Y, out float x, out float y);
        return new Vector2(x, y);
    }

    /// <summary>
    /// Renders all the graphics to the window since the last call.
    /// </summary>
    public void Render() => SDLNative.SDL_RenderPresent(_renderer);

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _renderer.Dispose();

        base.Dispose(disposing);
    }

    private void CreateRenderer()
    {
        _renderer = SDLNative.SDL_CreateRenderer(Handle, RendererName);

        SDLException.ThrowIf(_renderer.IsInvalid);

        SDLException.ThrowIfFailed(SDLNative.SDL_SetRenderVSync(_renderer, VSync));
        SDLException.ThrowIfFailed(SDLNative.SDL_SetRenderLogicalPresentation(_renderer, Presentation.Width, Presentation.Height, Presentation.Mode));
    }

    private void ApplyState(in RenderState state)
    {
        if (state.BlendMode.HasValue)
            SDLNative.SDL_SetRenderDrawBlendMode(_renderer, state.BlendMode.Value);

        if (state.Scale.HasValue)
        {
            Vector2 scale = state.Scale.Value;
            SDLNative.SDL_SetRenderScale(_renderer, scale.X, scale.Y);
        }

        if (state.ColorScale.HasValue)
            SDLNative.SDL_SetRenderColorScale(_renderer, state.ColorScale.Value);

        SDLNative.SetRenderClip(_renderer, state.Clip);
    }
}
