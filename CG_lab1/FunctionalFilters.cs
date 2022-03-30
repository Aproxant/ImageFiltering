using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_lab1
{
    static class  FunctionalFilters
    {
        private const int brightness = 30;
        private const int contrastFactor = 2;
        private const double gamma = 0.5;
        public static Bitmap SetGamma(Bitmap im)
        {
            Bitmap bmp = new Bitmap(im);
            System.Drawing.Color c;
            int[] gammaArray = new int[256];
            for (int i = 0; i < 256; ++i)
            {
                gammaArray[i] =(int) ((255.0 * Math.Pow(i / 255.0, gamma)));
            }
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    c = bmp.GetPixel(i, j);
                  
                    bmp.SetPixel(i, j, System.Drawing.Color.FromArgb(gammaArray[c.R],gammaArray[c.G], gammaArray[c.B]));
                }
            }
            return bmp;
        }
        public static Bitmap SetBrightness(Bitmap im)
        {
            Bitmap bmp = new Bitmap(im);
            System.Drawing.Color c;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    c = bmp.GetPixel(i, j);
                    int R = c.R + brightness;
                    int G = c.G + brightness;
                    int B = c.B + brightness;

                    if (R < 0) R = 0;
                    if (R > 255) R = 255;

                    if (G < 0) G = 0;
                    if (G > 255) G = 255;

                    if (B < 0) B = 0;
                    if (B > 255) B = 255;

                    bmp.SetPixel(i, j, System.Drawing.Color.FromArgb(R,G,B));
                }
            }
            return bmp;
        }
        public static Bitmap SetContrast(Bitmap im)
        {
            Bitmap bmp = new Bitmap(im);
            Color c;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    c = bmp.GetPixel(i, j);
                    double newRed = contrastFactor * (c.R - 128) + 128;
                    if (newRed < 0) newRed = 0;
                    if (newRed > 255) newRed = 255;

                    double newGreen = contrastFactor * (c.G - 128) + 128;
                    if (newGreen < 0) newGreen = 0;
                    if (newGreen > 255) newGreen = 255;

                    double newBlue = contrastFactor * (c.B - 128) + 128;
                    if (newBlue < 0) newBlue = 0;
                    if (newBlue > 255) newBlue = 255;

                    bmp.SetPixel(i, j,Color.FromArgb((int)newRed,(int) newGreen,(int) newBlue));
                }
            }
            return bmp;
        }
        public static Bitmap SetInverse(Bitmap im)
        {
            Bitmap bmp = new Bitmap(im);
            for (int y = 0; y <bmp.Height; y++)
            {
                for (int x = 0;x <bmp.Width; x++)
                {
                    System.Drawing.Color inverse = bmp.GetPixel(x, y);
                    inverse = System.Drawing.Color.FromArgb(255, (255 - inverse.R), (255 - inverse.G), (255 - inverse.B));
                    bmp.SetPixel(x, y, inverse);
                }
            }
            return bmp;
        }
        public static Bitmap GrayImage(Bitmap im)
        {
            Bitmap bmp = new Bitmap(im);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    System.Drawing.Color grayIm = bmp.GetPixel(x, y);
                    int gray = (grayIm.R + grayIm.G + grayIm.B)/3;
                    grayIm = System.Drawing.Color.FromArgb(255, gray,gray, gray);
                    bmp.SetPixel(x, y, grayIm);
                }
            }
            return bmp;
        }
    }
}
