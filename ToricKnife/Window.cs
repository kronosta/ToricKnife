using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;

namespace ToricKnife
{
    public class Window : GameWindow
    {
        public float time = 0.0f;

        private float[] TestVertices = new float[]
        {
            -0.5f, -0.5f, 0.0f,
            0.5f, -0.5f, 0.0f,
            0.0f, 0.5f, 0.0f
        };
        private int TestVBO;
        private int TestVAO;
        private string TestVertexShaderSource =
@"#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;


uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 Normal;
out vec3 FragPos;


void main()
{
    gl_Position = projection * view * model * vec4(aPosition, 1.0);
    Normal = aNormal;
    FragPos = vec3(model * vec4(aPosition, 1.0));
}";
        private string TestFragmentShaderSource =
@"#version 330 core
uniform vec3 LightPos;

in vec3 Normal;
in vec3 FragPos;
out vec4 FragColor;

void main()
{
    vec3 ambient = vec3(0.2, 0.2, 0.2);
    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(LightPos - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * vec3(1.0, 1.0, 1.0);
    FragColor = vec4((diffuse + ambient) * FragColor.rgb, 1.0);
}";
        private VertexFragmentShader TestVFShader;
        private Matrix4 M4Model;
        private Matrix4 M4View;
        private Matrix4 M4Projection;

        float[] w = new float[] { 0.0f };

        public MarchingCubesMezh[] DimObjs;
        public Window(int width, int height, string title) : base(
            GameWindowSettings.Default,
            new NativeWindowSettings() { ClientSize = (width, height), Title = title })
        {
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            MarchingCubesMezh[] vToriglome =
                Enumerable
                .Range(0, 1)
                .Select(v => v / 2.0f)
                .Select(v =>
                    new MarchingCubesMezh(
                        10,
                        10,
                        10,
                        -3.0f, -3.0f, -3.0f,
                        3.0f, 3.0f, 3.0f,
                        (x, y, z) => (float)Math.Sqrt(
                            Math.Pow(Math.Sqrt(x * x + y * y + w[0] * w[0] + v * v) - 2.0f, 2)
                            + z * z
                        ) - 1.0f,
                        TestVertexShaderSource,
                        TestFragmentShaderSource,
                        -1
                    ){ DelayCounter = new Random((int)(v * 4.0f)).Next(0,9) }
                )
                .ToArray();
            foreach (var i in vToriglome) i.Setup();
            DimObjs = vToriglome;

            new Thread(() =>
            {
                Random random = new Random(901394);
                while (true)
                {
                    //DimObjs[random.Next(0,DimObjs.Length)].MarchingCubes();
                }
            }).Start();

        }

        protected override void OnUnload()
        {
            base.OnUnload();

            foreach (var i in DimObjs)
            {
                i.Dispose();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            base.OnRenderFrame(e);

            time += 4.0f * (float)e.Time;

            M4Model = Matrix4.Identity;//Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(time * 4.0f));
            M4Model *= Matrix4.CreateTranslation(-2.0f, -2.0f, -2.0f);
            M4View = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(time * 3.0f)) *
                Matrix4.CreateTranslation(0.0f, 0.0f, -16.0f); 
            M4Projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(90.0f),
                Size.X / (float)Size.Y, 0.1f, 100.0f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            w[0] = (float)Math.Sin(time / 6.0f) * 3.0f;

            for (int i = 0; i < DimObjs.Length; i++)
            {
                DimObjs[i].MarchingCubes();

                DimObjs[i].GetShader().SetVector3("LightPos", new Vector3(0.0f, 5.0f, 0.0f));

                DimObjs[i].Draw(
                    M4Model * Matrix4.CreateTranslation((float)i, 0.0f, 0.0f),
                    M4View, M4Projection);
            }

            SwapBuffers();
            Console.WriteLine(watch.ElapsedMilliseconds + " milliseconds to render");
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
}
