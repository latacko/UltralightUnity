using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

class Window : GameWindow
{
    UltralightUnity.ULView view;
    UltralightUnity.ULRenderer renderer;
    private int _vao;
    private int _vbo;
    private int _ebo;
    private int _texture;
    private int _shader;
    private float _time;
    OpenTK.Mathematics.Vector2 currentMousePos;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, UltralightUnity.ULRenderer renderer, UltralightUnity.ULView view) : base(gameWindowSettings, nativeWindowSettings)
    {
        this.view = view;
        this.renderer = renderer;
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(Color4.White);

        float[] vertices =
{
    //  pos            // uv
    -1f, -1f, 0f,      0f, 1f,   // 0 bottom-left
     1f, -1f, 0f,      1f, 1f,   // 1 bottom-right
     1f,  1f, 0f,      1f, 0f,   // 2 top-right
    -1f,  1f, 0f,      0f, 0f    // 3 top-left
};

        uint[] indices =
        {
    0, 1, 2,
    2, 3, 0
};

        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();
        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer,
        vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        // Position 
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // Texcoords 
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        _texture = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _texture);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

        var bmp = view.GetSurface().GetBitmap();

        GL.TexImage2D(TextureTarget.Texture2D, 0,
            PixelInternalFormat.Rgba,
            (int)bmp.Width, (int)bmp.Height,
            0,
            PixelFormat.Bgra,
            PixelType.UnsignedByte,
            IntPtr.Zero);


        _shader = CreateShader();
        GL.UseProgram(_shader);
        GL.Uniform1(GL.GetUniformLocation(_shader, "tex"), 0);
    }

    private static int CreateShader()
    {
        string vertex = @"
            #version 330 core
            layout(location = 0) in vec3 aPos;
            layout(location = 1) in vec2 aTex;
            out vec2 TexCoord;
            void main()
            {
                gl_Position = vec4(aPos, 1.0);
                TexCoord = aTex;
            }
        ";

        string fragment = @"
            #version 330 core
            in vec2 TexCoord;
            out vec4 FragColor;
            uniform sampler2D tex;
            void main()
            {
                FragColor = texture(tex, TexCoord);
            }
        ";

        int vs = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vs, vertex);
        GL.CompileShader(vs);
        CheckShader(vs, "VERTEX");

        int fs = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fs, fragment);
        GL.CompileShader(fs);
        CheckShader(fs, "FRAGMENT");

        int program = GL.CreateProgram();
        GL.AttachShader(program, vs);
        GL.AttachShader(program, fs);
        GL.LinkProgram(program);

        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int status);
        if (status == 0)
        {
            string info = GL.GetProgramInfoLog(program);
            throw new Exception("Program link error: " + info);
        }

        GL.DeleteShader(vs);
        GL.DeleteShader(fs);

        return program;
    }

    private static void CheckShader(int shader, string name)
    {
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);
        if (status == 0)
        {
            string info = GL.GetShaderInfoLog(shader);
            throw new Exception($"{name} shader compile error: {info}");
        }
    }
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        renderer.RefreshDisplay(0);
        renderer.Render();

        var bitmap = view.GetSurface().GetBitmap();
        var pixels = bitmap.LockPixels();

        GL.BindTexture(TextureTarget.Texture2D, _texture);

        GL.TexSubImage2D(
            TextureTarget.Texture2D,
            0,
            0, 0,
            (int)bitmap.Width,
            (int)bitmap.Height,
            PixelFormat.Bgra,
            PixelType.UnsignedByte,
            pixels);

        bitmap.UnlockPixels();

        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.UseProgram(_shader);
        GL.BindVertexArray(_vao);
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, _texture);

        GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        GL.DeleteTexture(_texture);
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);
        GL.DeleteVertexArray(_vao);
        GL.DeleteProgram(_shader);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        renderer.Update();
        var kb = KeyboardState;
        if (kb.IsKeyDown(Keys.Escape))
            Close();
    }

    UltralightUnity.Enums.ULMouseButton currentPressedButton = UltralightUnity.Enums.ULMouseButton.None;
    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
        base.OnMouseMove(e);
        currentMousePos = e.Position;
        view.FireMouseEvent(
            new(
                UltralightUnity.Enums.ULMouseEventType.MouseMoved,
                (int)currentMousePos.X, (int)currentMousePos.Y,
                currentPressedButton));
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);
        view.FireScrollEvent(new(UltralightUnity.Enums.ULScrollEventType.ScrollByPixel, (int)e.OffsetX, (int)e.OffsetY));
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);
        view.FireMouseEvent(new(UltralightUnity.Enums.ULMouseEventType.MouseDown, (int)currentMousePos.X, (int)currentMousePos.Y, GetMouseButton(e.Button)));
        if (currentPressedButton == UltralightUnity.Enums.ULMouseButton.None)
            currentPressedButton = GetMouseButton(e.Button);
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        base.OnMouseUp(e);
        view.FireMouseEvent(new(UltralightUnity.Enums.ULMouseEventType.MouseUp, (int)currentMousePos.X, (int)currentMousePos.Y, GetMouseButton(e.Button)));
        if (currentPressedButton == GetMouseButton(e.Button))
            currentPressedButton = UltralightUnity.Enums.ULMouseButton.None;
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);

        view.Resize((uint)e.Width, (uint)e.Height);

        // Reallocate texture to match new bitmap size
        var bmp = view.GetSurface().GetBitmap();

        GL.BindTexture(TextureTarget.Texture2D, _texture);
        GL.TexImage2D(TextureTarget.Texture2D, 0,
            PixelInternalFormat.Rgba,
            (int)bmp.Width, (int)bmp.Height,
            0,
            PixelFormat.Bgra,
            PixelType.UnsignedByte,
            IntPtr.Zero);
    }


    UltralightUnity.Enums.ULMouseButton GetMouseButton(MouseButton btn)
    {
        return btn switch
        {
            MouseButton.Left => UltralightUnity.Enums.ULMouseButton.Left,
            MouseButton.Middle => UltralightUnity.Enums.ULMouseButton.Middle,
            MouseButton.Right => UltralightUnity.Enums.ULMouseButton.Right,
            _ => UltralightUnity.Enums.ULMouseButton.None,
        };
    }

    UltralightUnity.Enums.ULKeyEventModifiers modifiers;

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        base.OnKeyDown(e);
        System.Console.WriteLine($"Key down: {e.Key}");

        if (e.Key == Keys.LeftControl)
        {
            modifiers |= UltralightUnity.Enums.ULKeyEventModifiers.CtrlKey;
        }
        else if (e.Key == Keys.LeftAlt)
        {
            modifiers |= UltralightUnity.Enums.ULKeyEventModifiers.AltKey;
        }
        else if (e.Key == Keys.LeftShift)
        {
            modifiers |= UltralightUnity.Enums.ULKeyEventModifiers.ShiftKey;
        }

        view.FireKeyEvent(ULKeyEvent.KeyDown(ToDomKey(e.Key), ToDomKey(e.Key), e.IsRepeat, modifiers));
    }

    int ToDomKey(Keys key)
    {
        return key switch
        {
            // navigation
            Keys.Left => 37,
            Keys.Up => 38,
            Keys.Right => 39,
            Keys.Down => 40,

            // editing
            Keys.Backspace => 8,
            Keys.Tab => 9,
            Keys.Enter => 13,
            Keys.Escape => 27,
            Keys.Delete => 46,

            // modifiers (not usually sent as DOM keys)
            Keys.LeftShift => 16,
            Keys.RightShift => 16,
            Keys.LeftControl => 17,
            Keys.RightControl => 17,
            Keys.LeftAlt => 18,
            Keys.RightAlt => 18,

            // page/navigation
            Keys.Home => 36,
            Keys.End => 35,
            Keys.PageUp => 33,
            Keys.PageDown => 34,

            // space
            Keys.Space => 32,

            // default: ASCII-like mapping
            _ => (int)key
        };
    }
    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);
        // Console.WriteLine((char)e.Unicode);
        view.FireKeyEvent(ULKeyEvent.Character((char)e.Unicode, 0));
    }

    protected override void OnKeyUp(KeyboardKeyEventArgs e)
    {
        base.OnKeyUp(e);
        System.Console.WriteLine($"Key up: {e.Key}");

        if (e.Key == Keys.LeftControl)
        {
            modifiers &= ~UltralightUnity.Enums.ULKeyEventModifiers.CtrlKey;
        }
        else if (e.Key == Keys.LeftAlt)
        {
            modifiers &= ~UltralightUnity.Enums.ULKeyEventModifiers.AltKey;
        }
        else if (e.Key == Keys.LeftShift)
        {
            modifiers &= ~UltralightUnity.Enums.ULKeyEventModifiers.ShiftKey;
        }

        view.FireKeyEvent(ULKeyEvent.KeyUp((int)e.Key, (int)e.Key, e.IsRepeat, modifiers));
    }
}