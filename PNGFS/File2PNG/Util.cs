using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace File2PNG
{
    public static class Util
    {
        public static byte[] FromImage(Image Image)
        {
            var RecoveredData = ImageToBytes(Image).ToArray();
            var FileLengthBytes = RecoveredData.Take(4).ToArray();
            var FileLength = BitConverter.ToInt32(FileLengthBytes, 0);
            return RecoveredData.Skip(4).Take(FileLength).ToArray();
        }

        public static Image ToImage(byte[] InputData)
        {
            var InputSize = BitConverter.GetBytes(InputData.Length);
            var OutputData = new byte[InputSize.Length + InputData.Length];
            InputSize.CopyTo(OutputData, 0);
            InputData.CopyTo(OutputData, 4);
            var OutputSize = (int)Math.Ceiling(Math.Sqrt(Math.Ceiling(OutputData.Length / 3.0)));
            var Image = new Bitmap(OutputSize, OutputSize, PixelFormat.Format24bppRgb);
            var pixel = 0;
            var offset = 0;
            int B = 0, G = 0, R = 0;
            do
            {
                B = OutputData[offset++];
                G = OutputData[offset++];
                R = OutputData[offset++];
                Image.SetPixel(pixel % OutputSize, pixel / OutputSize, Color.FromArgb(0, R, G, B));
                ++pixel;
            } while (offset < OutputData.Length - 3);
            if (offset < OutputData.Length)
                B = OutputData[offset++];
            if (offset < OutputData.Length)
                G = OutputData[offset++];
            if (offset < OutputData.Length)
                R = OutputData[offset++];
            Image.SetPixel(pixel % OutputSize, pixel / OutputSize, Color.FromArgb(0, R, G, B));
            return Image;
        }

        private static IEnumerable<byte> ImageToBytes(Image Input)
        {
            var Bitmap = new Bitmap(Input);
            var pixel = 0;
            var width = Input.Width;
            var total = width * width;
            do
            {
                var Color = Bitmap.GetPixel(pixel % width, pixel / width);
                yield return Color.B;
                yield return Color.G;
                yield return Color.R;
                ++pixel;
            } while (pixel < total);
        }
    }
}