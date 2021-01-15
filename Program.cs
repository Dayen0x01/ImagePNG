using System;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImagePNG
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.Title = "ImagePNG";
            Screen.DrawLogo();
            Screen.DrawMenu();

            Console.Read();
        }
    }
}
