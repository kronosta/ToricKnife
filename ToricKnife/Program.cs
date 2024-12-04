using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace ToricKnife
{
    public class Program
    {
        public static string[] Arguments { get; set; }
        public static void Main(string[] args)
        {
            Arguments = args;
            using (Window window = new Window(800, 600, "ToricKnife Rendering Window"))
            {
                window.Run();
            }
        }
    }
}