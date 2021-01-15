using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ImagePNG
{
    public class Screen
    {
        public static void WriteFormattedLine(string Format, params string[] Params)
        {
            int formatLength = Format.Length;
            int currIndex = 0;
            bool readingNumber = false;
            string numberRead = string.Empty;
            while (currIndex < formatLength)
            {
                var ch = Format[currIndex];
                switch (ch)
                {
                    case '{':
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        readingNumber = true;
                        numberRead = string.Empty;
                        break;
                    case '}':
                        var number = int.Parse(numberRead);
                        var answer = Params[number];
                        Console.Write(answer);
                        Console.ResetColor();
                        readingNumber = false;
                        break;
                    default:
                        if (readingNumber)
                            numberRead += ch;
                        else
                            Console.Write(ch);
                        break;
                }

                currIndex++;
            }
        }
        public static void DrawLogo()
        {
            Console.WriteLine(@" _____                            _____  _   _  _____ ");
            Console.WriteLine(@"|_   _|                          |  __ \| \ | |/ ____|");
            Console.WriteLine(@"  | |  _ __ ___   __ _  __ _  ___| |__) |  \| | |  __ ");
            Console.WriteLine(@"  | | | '_ ` _ \ / _` |/ _` |/ _ \  ___/| . ` | | |_ |");
            Console.WriteLine(@" _| |_| | | | | | (_| | (_| |  __/ |    | |\  | |__| |");
            Console.WriteLine(@"|_____|_| |_| |_|\__,_|\__, |\___|_|    |_| \_|\_____|");
            WriteFormattedLine(@"                       __/ |      Created by {0}     ", new string[] { "Dayen0x01" });
            WriteFormattedLine("\n                      |___/       V{0}             ", new string[] { "1.1" });
        }
        public static void DrawMenu()
        {
            Dictionary<string, Action> Commands = new Dictionary<string, Action>();

            Commands.Add("1", ConvertToPNG);
            Commands.Add("2", CompressJPG);
            Commands.Add("3", ExtractMetadata);
            Commands.Add("4", HiddenText);
            Commands.Add("5", GetFileHash);

            WriteFormattedLine("\n\n[{0}] Convert images to {1}", new string[] { "1", "PNG" });
            WriteFormattedLine("\n[{0}] Compress {1} images", new string[] { "2", "JPG" });
            WriteFormattedLine("\n[{0}] Extract metadata", new string[] { "3" });
            WriteFormattedLine("\n[{0}] {1} text into a image", new string[] { "4","Hidden" });
            WriteFormattedLine("\n[{0}] Get file {1}\n\n", new string[] { "5", "hash" });

            while (true)
            {
                Console.Write("[*] Selec an option: ");

                string Input = Console.ReadLine();

                if (Commands.Keys.Contains(Input))
                {
                    Commands[Input].Invoke();
                    break;
                }
            }
        }
        public static void GetFileHash()
        {
            while (true)
            {
                Console.Write("[!] Select file to get hash: ");
                string file = Console.ReadLine();

                if (File.Exists(file))
                {
                    var HashMD5 = Cryptograph.CalcularHashMD5(file);
                    var HashSHA256 = Cryptograph.CalcularHashSHA256(file);

                    var HashStringMD5 = BitConverter.ToString(HashMD5).Replace("-", "").ToLower();
                    var HashStringSHA256 = BitConverter.ToString(HashSHA256).Replace("-", "").ToLower();

                    Screen.WriteFormattedLine("[*] Hash MD5: {0}\n[*] Hash Sha256: {1}", new string[] {HashStringMD5.ToString(),HashStringSHA256.ToString()  });

                    break;
                }
            }
        }
        public static void HiddenText()
        {
            while (true)
            {
                Console.Write("[!] Select .txt file: ");
                string text = Console.ReadLine();

                Console.Write("[!] Select image file: ");
                string image = Console.ReadLine();

                if (File.Exists(text) && File.Exists(image))
                {

                    Console.Write("[!] Select path to save new file: ");
                    string save = Console.ReadLine();

                    Steganography.Hide(image, text, save);

                    break;
                }
            }
        }
        public static void ExtractMetadata()
        {
            while (true)
            {
                Console.Write("[!] Select image path: ");
                string path = Console.ReadLine();

                if (File.Exists(path))
                {
                    Metadata.getMetadata(path);
                    break;
                }
            }
        }
        public static void CompressJPG()
        {
            while (true)
            {
                Console.Write("[!] Select images folder: ");
                string path = Console.ReadLine();

                if (Directory.Exists(path))
                {
                    Console.Write("[!] Selection compression level 0-100: ");
                    long level = Convert.ToInt64(Console.ReadLine());

                    Images.CompressImageJpg(path, level);

                    break;
                }
            }
        }
        public static void ConvertToPNG()
        {
            while (true)
            {
                Console.Write("[!] Select images folder: ");
                string path = Console.ReadLine();

                if (Directory.Exists(path))
                {
                    WriteFormattedLine("[!] Do you want to keep the original images? ({0}/{1}) ", new string[] { "Y", "N" });

                    bool KeepImages = Console.ReadLine().ToLower() == "y" ? true : false;

                    Console.Write("[!] Input pattern (Ex: *.jpg): ");
                    string pattern = Console.ReadLine();

                    Images.ConvertToPNG(path, pattern, KeepImages);

                    break;
                }
            }
        }
    }
}
