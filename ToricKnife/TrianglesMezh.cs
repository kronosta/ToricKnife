using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToricKnife
{
    public class TrianglesMezh : AbstractMezh
    {
        public VertexFragmentShader Shader { get; set; }
        public BufferUsageHint Usage { get; init; }
        public TrianglesMezh(string vertexSrc, string fragmentSrc, float[] vertices, BufferUsageHint usage)
        {
            Shader = new VertexFragmentShader(vertexSrc, fragmentSrc);
            this.vertices = vertices;
            Usage = usage;
        }

        public override VertexFragmentShader GetShader() => Shader;
        public override int GetMaxTriangles() => vertices.Length / 9;
        public override BufferUsageHint GetBufferUsageHint() => Usage;
    }
}
