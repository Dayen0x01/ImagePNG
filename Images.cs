using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System;

namespace ImagePNG
{
    public class Images
    {
        private static ImageCodecInfo GetEncoderInfo(string mime_type)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i <= encoders.Length; i++)
            {
                if (encoders[i].MimeType == mime_type) return encoders[i];
            }
            return null;
        }
        private static void Compress(Image image, string file_name, long compression)
        {
            try
            {
                EncoderParameters encoder_params = new EncoderParameters(1);
                encoder_params.Param[0] = new EncoderParameter(Encoder.Quality, compression);

                ImageCodecInfo image_codec_info =
                    GetEncoderInfo("image/jpeg");
                File.Delete(file_name);
                image.Save(file_name, image_codec_info, encoder_params);
                image.Dispose();
            }
            catch (Exception ex)
            {
                Screen.WriteFormattedLine("\n[!] Warning: {0}", new string[] { ex.Message });
            }
        }
        public static void CompressImageJpg(string FolderPath, long Compression)
        {
            Screen.WriteFormattedLine("\n[!] Scanning images in {0}", new string[] { FolderPath });
            string[] files = Directory.GetFiles(FolderPath, "*.jpg");
            Screen.WriteFormattedLine("\n[*] We found {0} files!", new string[] { files.Count().ToString() });
            string save = FolderPath + "\\[Compress] ImagePNG\\";

            Directory.CreateDirectory(save);
            int i = 0;

            foreach (var f in files)
            {
                i++;
                var img = Image.FromFile(f);
                Compress(img, save + f.Split('\\').Last(), (long)Compression);

                Screen.WriteFormattedLine("\n[!] Compress {0} file of {1} files", new string[] { i.ToString(), files.Count().ToString(), f });

            }
        }
        public static void ConvertToPNG(string FolderPath, string Pattern, bool KeepImages=true)
        {
            Screen.WriteFormattedLine("\n[!] Scanning images in {0}", new string[] { FolderPath });
            string[] files = Directory.GetFiles(FolderPath, Pattern);
            Screen.WriteFormattedLine("\n[*] We found {0} files!", new string[] { files.Count().ToString() });

            string save = KeepImages == true ? FolderPath + "\\ImagePNG\\" : FolderPath +"\\";

            if (!Directory.Exists(save))
            {
                Directory.CreateDirectory(save);
            }
            int i = 0;

            foreach (var f in files)
            {
                i++;
                Screen.WriteFormattedLine("\n[!] Converting {0} file of {1} files - {2}", new string[] { i.ToString(), files.Count().ToString(), f });

                var img = Bitmap.FromFile(f);
                img.Save(save + f.Split('\\').Last().Replace(Pattern.Replace("*",""),"") + ".png", ImageFormat.Png);
                img.Dispose();
                if (KeepImages == false)
                {
                    File.Delete(f);
                }
            }
        }
    }
}
