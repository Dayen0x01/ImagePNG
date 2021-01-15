using System;
using System.Collections.Generic;
using MetadataExtractor;

namespace ImagePNG
{
    public class Metadata
    {
        public static void getMetadata(string imagePath)
        {
            try
            {
                IEnumerable<Directory> directories = ImageMetadataReader.ReadMetadata(imagePath);
                foreach (var directory in directories)
                    foreach (var tag in directory.Tags)
                        Console.WriteLine($"[!] {directory.Name} - {tag.Name} = {tag.Description}");
            }
            catch(Exception ex)
            {
               // Console.WriteLine(ex.Message);
            }
        }
    }
}
