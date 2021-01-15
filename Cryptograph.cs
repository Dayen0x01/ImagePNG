using System.IO;
using System.Security.Cryptography;

namespace ImagePNG
{
    public class Cryptograph
    {
        // MD5
        public static byte[] CalcularHashMD5(string arquivo)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(arquivo))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }

        // SHA 256
        public static byte[] CalcularHashSHA256(string arquivo)
        {
            using (var algoritmoHash = SHA256.Create())
            {
                using (var stream = File.OpenRead(arquivo))
                {
                    return algoritmoHash.ComputeHash(stream);
                }
            }
        }
    }
}