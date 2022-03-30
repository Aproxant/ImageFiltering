using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CG_lab1
{
    static class ConvolutionBitmap
    {
        public static Bitmap ConvolutionFilter(Bitmap sourceBitmap, AbstractConvolution filter)
        {
            //Part of coping a Bitmap to byte array
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];//allocating buffer for new bitmap

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);


            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;


            int filtersizeY = filter.KernelMatrix.GetLength(0);
            int filtersizeX = filter.KernelMatrix.GetLength(1);
            int filterOffsetX = (filtersizeX - 1) / 2;
            int filterOffsetY = (filtersizeY - 1) / 2;
            int byteOffset = 0;
            int resultOffset = 0;


            for (int offsetY = 0; offsetY < sourceBitmap.Height; offsetY++)
            {
                for (int offsetX = 0; offsetX <sourceBitmap.Width; offsetX++)
                {
                    blue = 0;
                    green = 0;
                    red = 0;
                    for (int filterY = -filterOffsetY+filter.AnchorY; filterY <= filterOffsetY+filter.AnchorY; filterY++)
                    { 
                        for (int filterX = -filterOffsetX+filter.AnchorX;filterX <= filterOffsetX+filter.AnchorX; filterX++)
                        {
                            
                            int vertPos = offsetY + filterY; 
                            int HorPos = 4 * (offsetX + filterX); 

                            if (vertPos > sourceData.Height - 1)
                                vertPos = sourceData.Height - 1;
                            else if (vertPos < 0)
                                vertPos = 0;
                            if (HorPos >= sourceData.Stride)
                                HorPos = sourceData.Stride - 4;
                            else if (HorPos < 0)
                                HorPos = 0;

                            byteOffset=((vertPos * sourceData.Stride) + HorPos);  
                           
                            blue += (double)(pixelBuffer[byteOffset]) *
                                     filter.KernelMatrix[filterY-filter.AnchorY + filterOffsetY,
                                     filterX-filter.AnchorX + filterOffsetX];


                            green += (double)(pixelBuffer[byteOffset + 1]) *
                                      filter.KernelMatrix[filterY-filter.AnchorY + filterOffsetY,
                                      filterX-filter.AnchorX + filterOffsetX];


                            red += (double)(pixelBuffer[byteOffset + 2]) *
                                    filter.KernelMatrix[filterY-filter.AnchorY + filterOffsetY,
                                    filterX-filter.AnchorX + filterOffsetX];
                        }
                    }
                    resultOffset = (offsetX * 4) + (offsetY*sourceData.Stride);
                    double resultBlue = (blue /filter.Divisor) + filter.Offset;
                    double resultGreen =(green/ filter.Divisor) + filter.Offset;
                    double resultRed= (red/ filter.Divisor) + filter.Offset;
                    resultBuffer[resultOffset] = (byte) ((resultBlue > 255) ? 255 : (resultBlue < 0) ? 0 : resultBlue);
                    resultBuffer[resultOffset + 1] = (byte)((resultGreen > 255) ? 255 : (resultGreen < 0) ? 0 : resultGreen);
                    resultBuffer[resultOffset + 2] = (byte)((resultRed > 255) ? 255 : (resultRed < 0) ? 0 : resultRed);
                    resultBuffer[resultOffset + 3] = 255;
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }
    }
}
