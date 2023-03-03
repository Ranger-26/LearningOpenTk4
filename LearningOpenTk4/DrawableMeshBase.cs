using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace LearningOpenTk4;

public abstract class DrawableMeshBase
{
    private Shader _shader;

    private Vector3 _position;

    private Vector3 _rotation;

    private Vector3 _scale;

    private Vector3 _oldPosition;

    private Texture tex1;

    private Texture tex2;
    
    public DrawableMeshBase()
    {
        _shader = new Shader(@"C:\Users\s2104427\Source\Repos\LearningOpenTk4\LearningOpenTk4\Shaders\shader.vert",
            @"C:\Users\s2104427\Source\Repos\LearningOpenTk4\LearningOpenTk4\Shaders\shader.frag");
        tex1 = new Texture(@"C:\Users\s2104427\Desktop\container.jpg");
        tex2 = new Texture(@"C:\Users\s2104427\Desktop\awesomeface.png");
            
            
        _shader.SetInt("texture0", 0);
        _shader.SetInt("texture1", 1);
        _shader.SetFloat("varyAmount", 0.2f);
        Initialize();
        Render();
    }

    public virtual void Initialize()
    {
        
    }
    
    public abstract void Draw();

    public void SetScale(Vector3 scale)
    {
        _scale = scale;
    }

    public void SetPosition(Vector3 position)
    {
        _oldPosition = _position;
        _position = position;
    }

    public void SetRotation(Vector3 rotation)
    {
        _rotation = rotation;
    }

    public void Render()
    {
        _shader.Use();
        tex1.Use();
        tex2.Use(TextureUnit.Texture1);

        Matrix4 scale = Matrix4.CreateScale(_scale);
        Matrix4 position = Matrix4.CreateTranslation(_position - _oldPosition);
        _shader.SetMatrix4("transform", scale * position);
        
        Draw();
    }
}