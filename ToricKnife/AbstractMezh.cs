using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ToricKnife
{
    // Class is called Mezh because it doesn't have to be a specific set of triangles
    public abstract class AbstractMezh
    {
        public float[]? vertices;
        public float[]? normals;
        public int VBO, VBONormal, VAO;
        public int maxTriangles;

        public virtual void Setup()
        {
            PreSetup();
            maxTriangles = GetMaxTriangles();
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float),
                vertices,
                GetBufferUsageHint());

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            VBONormal = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBONormal);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                normals.Length * sizeof(float),
                normals,
                GetBufferUsageHint());
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);
        }

        public virtual void CalculateNormals()
        {
            for (int i = 0; i < vertices.Length; i += 9)
            {
                Vector3 a = new Vector3(
                    vertices[i + 3] - vertices[i + 0],
                    vertices[i + 4] - vertices[i + 1],
                    vertices[i + 5] - vertices[i + 2]);
                Vector3 b = new Vector3(
                    vertices[i + 6] - vertices[i + 0],
                    vertices[i + 7] - vertices[i + 1],
                    vertices[i + 8] - vertices[i + 2]);
                Vector3 cross = Vector3.Cross(a, b);
                normals[i] = normals[i + 3] = normals[i + 6] = cross.X;
                normals[i + 1] = normals[i + 4] = normals[i + 7] = cross.Y;
                normals[i + 2] = normals[i + 5] = normals[i + 8] = cross.Z;
            }
        }

        public virtual void Draw(Matrix4 model, Matrix4 view, Matrix4 projection)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float),
                vertices,
                GetBufferUsageHint());
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBONormal);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                normals.Length * sizeof(float),
                normals,
                GetBufferUsageHint());

            VertexFragmentShader shader = GetShader();
            GL.UseProgram(shader.Handle);
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);
            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, GetMaxTriangles() * 3);
        }

        public virtual void Dispose()
        {
            GL.DeleteProgram(GetShader().Handle);
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(VBONormal);
            GL.DeleteVertexArray(VAO);
            GetShader().Dispose();
        }

        public virtual void PreSetup() { }

        public abstract VertexFragmentShader GetShader();

        // The number of elements in the buffer * 3
        public abstract int GetMaxTriangles();
        public abstract BufferUsageHint GetBufferUsageHint();
    }
}
