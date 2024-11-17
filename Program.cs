using System;

public class Program
{
    public static string[] Arguments { get; set; }
    public static void Main(string[] args)
    {
        Arguments = args;
        Console.WriteLine("Test");
    }
}