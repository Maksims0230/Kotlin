using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace KinoStudio_NET.Models
{
    public static class PosterImageModel
    {
        public static async Task<(string, byte[])> SelectImage()
        {
            OpenFileDialog OFD = new OpenFileDialog()
            {
                Filter = "Файлы изображения|*.png;*.jpg;*.jpeg",
                Multiselect = false
            };

            if (OFD.ShowDialog() ?? false)
            {
                await using (var file = new FileStream(OFD.FileName, FileMode.Open))
                {
                    var byteImage = new byte[file.Length];
                    await file.ReadAsync(byteImage, 0, byteImage.Length);
                    return (OFD.SafeFileName, byteImage);
                }
            }
            else return ("", new byte[] { });
        }
    }
}