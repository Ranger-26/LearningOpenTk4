using OpenTK.Graphics.ES30;

namespace LearningOpenTk4;

public class TriangleMesh : DrawableMeshBase
{
    private float[] _vertices = {
        -0.5f, -0.5f, 0.0f, //Bottom-left vertex
        0.5f, -0.5f, 0.0f, //Bottom-right vertex
        0.0f,  0.5f, 0.0f  //Top vertex
    };

    private int VBO;
    private int _VAO;
    public override void Initialize()
    {
        Console.WriteLine("Drawing triangle...");
        VBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
    }

    
    
    public override void Draw()
    {
        throw new NotImplementedException();
    }
}