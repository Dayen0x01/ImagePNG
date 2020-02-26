using System;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImagePNG
{
    class Program
    {
        private static void WriteFormattedLine(string format, params string[] answers)
        {
            int formatLength = format.Length;
            int currIndex = 0;
            bool readingNumber = false;
            string numberRead = string.Empty;
            while (currIndex < formatLength)
            {
                var ch = format[currIndex];
                switch (ch)
                {
                    case '{':
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        readingNumber = true;
                        numberRead = string.Empty;
                        break;
                    case '}':
                        var number = int.Parse(numberRead);
                        var answer = answers[number];
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

        static ImageCodecInfo GetEncoderInfo(string mime_type)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i <= encoders.Length; i++)
            {
                if (encoders[i].MimeType == mime_type) return encoders[i];
            }
            return null;
        }

        static void SaveJpg(Image image, string file_name, long compression)
        {
            try
            {
                EncoderParameters encoder_params = new EncoderParameters(1);
                encoder_params.Param[0] = new EncoderParameter(Encoder.Quality, compression);

                ImageCodecInfo image_codec_info =
                    GetEncoderInfo("image/jpeg");
                File.Delete(file_name);
                image.Save(file_name, image_codec_info, encoder_params);
            }
            catch (Exception ex)
            {
                WriteFormattedLine("\n[!] Warning: {0}", new string[] { ex.Message });
            }
        }

        static void ScanImageJpg(string path)
        {
            WriteFormattedLine("\n[!] Scanning images in {0}", new string[] { path });
            string[] files = Directory.GetFiles(path, "*.jpg");
            WriteFormattedLine("\n[*] We found {0} files!", new string[] { files.Count().ToString() });
            string save = path + "\\[Compress] ImagePNG\\";

            Directory.CreateDirectory(save);
            int i = 0;

            foreach (var f in files)
            {
                i++;
                var img = Image.FromFile(f);
                SaveJpg(img, save + f.Split('\\').Last(), (long)80);
                FileInfo nfi = new FileInfo(save + f.Split('\\').Last());


                WriteFormattedLine("\n[!] Compress {0} file of {1} files", new string[] { i.ToString(), files.Count().ToString(), f});
                
            }
        }
        static void ScanImage(string path, string pattern)
        {
            WriteFormattedLine("\n[!] Scanning images in {0}", new string[] { path });
            string[] files = Directory.GetFiles(path, pattern);
            WriteFormattedLine("\n[*] We found {0} files!", new string[] { files.Count().ToString() });

            string save = path + "\\ImagePNG\\";

            Directory.CreateDirectory(save);

            int i = 0;

            foreach(var f in files)
            {
                i++;
                WriteFormattedLine("\n[!] Converting {0} file of {1} files - {2}", new string[] { i.ToString(),files.Count().ToString(), f });

                var img = Bitmap.FromFile(f);
                img.Save(save + f.Split('\\').Last() + ".png", ImageFormat.Png);
            }
            
        }
        static void Main(string[] args)
        {
            begin:
            Console.Title = "ImagePNG";
            string[] param = { "Dayen0x01" };
            WriteFormattedLine("[*] ImagePNG create by {0}", param);
            WriteFormattedLine("\n[*] Version {0}", new string[] { "1.0" });
            WriteFormattedLine("\n[*] Contact: {0}", new string[] { "https://www.instagram.com/neydame" });
            Console.Write("\n[*] Select a option: ");
            WriteFormattedLine("\n  [{0}] - Convert image to {1}\n  [{2}] - Compress {3}", new string[] {"1","PNG","2","JPG" });
            Console.Write("\n[*] Select: ");
            string response = Console.ReadLine();

            switch (response)
            {
                case "1":

                    Console.Write("\n[!] Select images folder: ");
                    string path = Console.ReadLine();
                    if(!Directory.Exists(path))
                    {
                        goto begin;
                    }
                    Console.Write("[!] Input pattern (Ex: *.jpg): ");
                    string pattern = Console.ReadLine();

                    ScanImage(path, pattern);
                    break;
                case "2":
                    Console.Write("\n[!] Select images folder: ");
                    string tpath = Console.ReadLine();
                    ScanImageJpg(tpath);
                    break;
            }

            Console.Read();
        }
    }
}
