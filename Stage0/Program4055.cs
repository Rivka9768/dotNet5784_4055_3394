/*// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");*/
using System;

namespace Stage0 // Note: actual namespace depends on the project name.
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome4055();
            Welcome3394();
            Console.ReadKey();

        }

        static partial void Welcome3394();
        private static void Welcome4055()
        {
            Console.Write("Enter your name: ");
            string user_name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", user_name);
            /*    Console.Write("press any key to continue...");*/
        }
    }
}