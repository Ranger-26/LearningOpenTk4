using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using LearningOpenTk4;
using LearnOpenTK;
using OpenTK.Mathematics;


namespace LearnOpenTK

{   // Here we'll be elaborating on what shaders can do from the Hello World project we worked on before.
    // Specifically we'll be showing how shaders deal with input and output from the main program 
    // and between each other.
    public class Game : GameWindow
    {
        // We're assigning three different colors at the asscoiate vertex position:
        // blue for the top, green for the bottom left and red for the bottom right.
        float[] _vertices =
        {
            //Position          Texture coordinates         Second texture 
             0.5f,  0.5f, 0.0f, 1f, 1f, // top right
             0.5f, -0.5f, 0.0f, 1f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1f  // top left
        };

        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };
        
        private int _vertexBufferObject;

        private int _elementBufferObject;
        
        private int _vertexArrayObject;

        private int _vertexBufferObject2;

        private int _elementBufferObject2;
        
        private Texture tex1;
        private Texture tex2;
        
        private Shader _shader;

        private Stopwatch _stopwatch;
        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        // Now, we start initializing OpenGL.
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _vertexBufferObject = GL.GenBuffer();
            _elementBufferObject = GL.GenBuffer();
            
            //generate 2nd vbo
            _vertexBufferObject2 = GL.GenBuffer();
            _elementBufferObject2 = GL.GenBuffer();
            
            //bind vertex buffer object
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
            //element array object
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            
            /*
            //bind second vbo
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject2);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
            //element array object 2
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject2);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            */
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            
            // Just like before, we create a pointer for the 3 position components of our vertices.
            // The only difference here is that we need to account for the 3 color values in the stride variable.
            // Therefore, the stride contains the size of 6 floats instead of 3.
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            
            GL.GetInteger(GetPName.MaxVertexAttribs, out int maxAttributeCount);
            Debug.WriteLine($"Maximum number of vertex attributes supported: {maxAttributeCount}");

            //create and use shader
            _shader = new Shader(@"C:\Users\s2104427\Source\Repos\LearningOpenTk4\LearningOpenTk4\Shaders\shader.vert", @"C:\Users\s2104427\Source\Repos\LearningOpenTk4\LearningOpenTk4\Shaders\shader.frag");
            _shader.Use();
            
            //set the texture attributes
            int texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3*sizeof(float));
            //load in texture

            //set texture paramaters for x and y axis
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            //set filtering method for texture
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            tex1 = new Texture(@"C:\Users\s2104427\Desktop\container.jpg");
            tex2 = new Texture(@"C:\Users\s2104427\Desktop\awesomeface.png");
            
            
            _shader.SetInt("texture0", 0);
            _shader.SetInt("texture1", 1);
            _shader.SetFloat("varyAmount", 0.2f);

            _stopwatch = new Stopwatch();
            _stopwatch.Start();            
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            tex1.Use();
            tex2.Use(TextureUnit.Texture1);
            
            _shader.Use();
            

            GL.BindVertexArray(_vertexArrayObject);
            _shader.SetMatrix4("transform", Matrix4.Identity);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length * sizeof(uint), DrawElementsType.UnsignedInt, _indices);
            Matrix4 trans = Matrix4.CreateTranslation(-0.5f, 0.5f, 0.0f);
            float value = (float)Math.Sin((double)_stopwatch.ElapsedMilliseconds/1000);
            Matrix4 scale = Matrix4.CreateScale(value, value, value);
            _shader.SetMatrix4("transform", scale*trans);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length * sizeof(uint), DrawElementsType.UnsignedInt, _indices);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}