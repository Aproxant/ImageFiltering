using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CG_lab1
{
    public class MedianCutAlgorithm
    {
        public List<System.Drawing.Color> allColors = new List<System.Drawing.Color>();
        public byte[] pixelBuffer;
        public Bitmap MedianC(Bitmap sourceBitmap, int coff)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);



            pixelBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            List<MedianCut> colors = new List<MedianCut>();
            for (int y = 0; y < sourceBitmap.Height; y++)
            {
                for (int x = 0; x < sourceBitmap.Width; x++)
                {
                    int byteOffset = ((y * sourceData.Stride) + x * 4);
                    
                    colors.Add(new MedianCut(pixelBuffer[byteOffset + 2], pixelBuffer[byteOffset + 1], pixelBuffer[byteOffset]));
                }
            }
            split_into_bucket(colors, coff);

            find_representative_color(sourceData);
            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }
        public void find_representative_color(BitmapData data)
        {
            for (int y = 0; y < data.Height; y++)
            {
                for (int x = 0; x < data.Width; x++)
                {
                    int index = 0;
                    int byteOffset = ((y * data.Stride) + x * 4);
                    double closest = distanceFromRGB(pixelBuffer[byteOffset + 2], pixelBuffer[byteOffset + 1], pixelBuffer[byteOffset], allColors[0]);
                    for (int i = 1; i < allColors.Count; i++)
                    {
                        if (closest > distanceFromRGB(pixelBuffer[byteOffset + 2], pixelBuffer[byteOffset + 1], pixelBuffer[byteOffset], allColors[i]))
                        {
                            closest = distanceFromRGB(pixelBuffer[byteOffset + 2], pixelBuffer[byteOffset + 1], pixelBuffer[byteOffset], allColors[i]);
                            index = i;
                        }
                    }
                    pixelBuffer[byteOffset] = allColors[index].B;
                    pixelBuffer[byteOffset + 1] = allColors[index].G;
                    pixelBuffer[byteOffset + 2] = allColors[index].R;
                }
            }
        }

        public double distanceFromRGB(int R, int G, int B, System.Drawing.Color col2)
        {
            double rdiff = Math.Abs(R - col2.R);
            double gdiff = Math.Abs(G - col2.G);
            double bdiff = Math.Abs(B - col2.B);
            return ((rdiff + gdiff + bdiff) / 3);
        }
        public void split_into_bucket(List<MedianCut> c, int size)
        {
            if (size == 0)
            {
                calculate_color(c);
                return;
            }

            byte r_range = (byte)(c.Max(col => col.R) - (byte)c.Min(l => l.R));
            byte g_range = (byte)(c.Max(p => p.G) - c.Min(col => col.G));
            byte b_range = (byte)(c.Max(col => col.B) - c.Min(col => col.B));

            if (r_range >= g_range && r_range >= b_range)
                c.Sort((x, y) => x.R.CompareTo(y.R));
            else if (b_range >= r_range && b_range >= g_range)
                c.Sort((x, y) => x.B.CompareTo(y.B));
            else if (g_range >= r_range && g_range >= b_range)
                c.Sort((x, y) => x.G.CompareTo(y.G));

            int median = c.Count / 2;
            int z = c.Count - c.GetRange(0, median).Count;
            split_into_bucket(c.GetRange(0, median), size - 1);
            split_into_bucket(c.GetRange(median, z), size - 1);
        }
        public void calculate_color(List<MedianCut> c)
        {
            int r_averange = c.Sum(x => x.R) / c.Count;
            int g_averange = c.Sum(x => x.G) / c.Count;
            int b_averange = c.Sum(x => x.B) / c.Count;
            System.Drawing.Color col = System.Drawing.Color.FromArgb(255, r_averange, g_averange, b_averange);
            allColors.Add(col);
        }
    }
}
