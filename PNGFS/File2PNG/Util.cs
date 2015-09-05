using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace File2PNG
{
    public static class Util
    {
        public static Image ToImage(byte[] InputData)
        {
            var OutputSize = (int)Math.Ceiling(Math.Sqrt(Math.Ceiling(InputData.Length / 3.0)));
            var Image = new Bitmap(OutputSize, OutputSize, PixelFormat.Format24bppRgb);
            var pixel = 0;
            var offset = 0;
            int B = 0, G = 0, R = 0;
            do
            {
                B = InputData[offset++];
                G = InputData[offset++];
                R = InputData[offset++];
                Image.SetPixel(pixel % OutputSize, pixel / OutputSize, Color.FromArgb(0, R, G, B));
                ++pixel;
            } while (offset < InputData.Length - 3);
            if (offset < InputData.Length)
                B = InputData[offset++];
            if (offset < InputData.Length)
                G = InputData[offset++];
            if (offset < InputData.Length)
                R = InputData[offset++];
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