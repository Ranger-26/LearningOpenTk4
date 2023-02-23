using System;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace LearningOpenTk4
{
    public class Texture
    {
        public int Handle;

        public Texture(string path)
        {
            //crate a new opengl texture
            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            //make the library load from the bottom left
            StbImage.stbi_set_flip_vertically_on_load(1);
            //load in image texture
            ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);
            //buffer the image data to the texture
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}
