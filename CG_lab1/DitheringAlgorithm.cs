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
    class DitheringAlgorithm
    {
        public Bitmap Ditering(Bitmap sourceBitmap, int coff)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            byte[] Buffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, Buffer, 0, Buffer.Length);

            sourceBitmap.UnlockBits(sourceData);

            double split = 255 / (coff - 1);
            List<byte> colors = new List<byte>();
            colors.Add(0);
            for (int i = 1; i < coff - 1; i++)
                colors.Add((byte)(split * i));
            colors.Add(255);
            List<MedianCut> thresh = new List<MedianCut>();
            for (int c = 0; c < coff-1; c++)
            {
                int counterR = 0;
                int counterG = 0;
                int counterB = 0;
                int sumR = 0;
                int sumG = 0;
                int sumB = 0;
                for (int y = 0; y < sourceBitmap.Height; y++)
                {
                    for (int x = 0; x < sourceBitmap.Width; x++)
                    {
                        int byteOffset = ((y * sourceData.Stride) + x * 4);
                        if ((Buffer[byteOffset] >= colors[c] && Buffer[byteOffset] < colors[c + 1]) || (Buffer[byteOffset] <= colors[c + 1] && c == coff - 1))
                        {
                            counterB++;
                            sumB += Buffer[byteOffset];
                        }
                        if ((Buffer[byteOffset + 1] >= colors[c] && Buffer[byteOffset + 1] < colors[c + 1]) || (Buffer[byteOffset + 1] <= colors[c + 1] && c == coff - 1))
                        {
                            counterG++;
                            sumG += Buffer[byteOffset + 1];
                        }
                        if ((Buffer[byteOffset + 2] >= colors[c] && Buffer[byteOffset + 2] < colors[c + 1]) || (Buffer[byteOffset + 2] <= colors[c + 1] && c == coff - 1))
                        {
                            counterR++;
                            sumR += Buffer[byteOffset + 2];
                        }

                    }
                }
                thresh.Add(new MedianCut( counterR==0? colors[c] : (byte)(sumR / counterR), counterG==0? colors[c] : (byte)(sumG / counterG), counterB==0? colors[c] : (byte)(sumB / counterB)));
            }
            for (int y = 0; y < sourceBitmap.Height; y++)
            {
                for (int x = 0; x < sourceBitmap.Width; x++)
                {
                    for (int i = 0; i < coff - 1; i++)
                    {
                        int byteOffset = ((y * sourceData.Stride) + x * 4);
                        if ((Buffer[byteOffset] >= colors[i] && Buffer[byteOffset] < colors[i + 1]) || (Buffer[byteOffset] <= colors[i + 1] && i == coff - 1))
                        {
                            if (Buffer[byteOffset] <= thresh[i].B)
                                Buffer[byteOffset] = colors[i];
                            else
                                Buffer[byteOffset] = colors[i + 1];

                        }
                        if ((Buffer[byteOffset + 1] >= colors[i] && Buffer[byteOffset + 1] < colors[i + 1]) || (Buffer[byteOffset + 1] <= colors[i + 1] && i == coff - 1))
                        {
                            if (Buffer[byteOffset + 1] <= thresh[i].G)
                                Buffer[byteOffset + 1] = colors[i];
                            else
                                Buffer[byteOffset+1] = colors[i + 1];
                        }
                        if ((Buffer[byteOffset + 2] >= colors[i] && Buffer[byteOffset + 2] < colors[i + 1]) || (Buffer[byteOffset + 2] <= colors[i + 1] && i == coff - 1))
                        {
                            if (Buffer[byteOffset + 2] <= thresh[i].R)
                                Buffer[byteOffset + 2] = colors[i];
                            else
                                Buffer[byteOffset + 2] = colors[i + 1];
                        }
                    }
                }
            }
            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(Buffer, 0, resultData.Scan0, Buffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
            
        }
    }
}
