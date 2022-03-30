using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CG_lab1
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //Place to set Convuctional filter Parameters
        private const int Brightness = 30;
        private const int Contrast = 20;
        private const double Gamma = 3;

        private Bitmap photo = null;
        private Bitmap orginal = null;
        private int matrixSizeX = 0;
        private int previousSizeX = 0;
        private int matrixSizeY = 0;
        private int previousSizeY = 0;
        public MainWindow()
        {
            InitializeComponent();
            inverse.IsEnabled = false;
            bright.IsEnabled = false;
            contrast.IsEnabled = false;
            gamma.IsEnabled = false;
            Reset.IsEnabled = false;
            Save.IsEnabled = false;
            grayScale.IsEnabled = false;
            Med.IsEnabled = false;
            Average.IsEnabled = false;
            ConvoList.Items.Add(new Blur());
            ConvoList.Items.Add(new GaussBlur());
            ConvoList.Items.Add(new Sharpen());
            ConvoList.Items.Add(new EdgeDetection());
            ConvoList.Items.Add(new Emboss());
            ConvoList1.Items.Add(new Blur());
            ConvoList1.Items.Add(new GaussBlur());
            ConvoList1.Items.Add(new Sharpen());
            ConvoList1.Items.Add(new EdgeDetection());
            ConvoList1.Items.Add(new Emboss());
            s3.IsSelected = true;
            sy3.IsSelected = true;
        }
        private void Load_Image(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.Title = "Open Image";
            openfiledialog.Filter = "Image File|*.bmp; *.jpeg; *.png;";
            string filename = null;
            if (openfiledialog.ShowDialog() == true)
            {
                inverse.IsEnabled = true;
                bright.IsEnabled = true;
                contrast.IsEnabled = true;
                gamma.IsEnabled = true;
                grayScale.IsEnabled = true;
                Average.IsEnabled = true;
                Med.IsEnabled = true;
                filename = openfiledialog.FileName;
            }
            if (filename != null)
            {
                grayScale.IsEnabled = true;
                inverse.IsEnabled = true;
                bright.IsEnabled = true;
                contrast.IsEnabled = true;
                gamma.IsEnabled = true;
                Reset.IsEnabled = true;
                Save.IsEnabled = true;
                photo = new Bitmap(filename);
                orginal = new Bitmap(filename);
                img.Source = BitmapToImageSource(photo);
            }

        }
        private void ListViewItem_Click(object sender, MouseButtonEventArgs e)
        {

            var item = sender as ListViewItem;
            if (item != null && photo != null)
            {
                AbstractConvolution filt = (AbstractConvolution)item.DataContext;
                photo = ConvolutionBitmap.ConvolutionFilter(photo, filt);
                img.Source = BitmapToImageSource(photo);
            }
        }
        private void ListViewItem_Click1(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                AbstractConvolution filt = (AbstractConvolution)item.DataContext;
                filt_name.Text = filt.Name;
                divi.Text = filt.Divisor.ToString();
                off.Text = filt.Offset.ToString();
                anchX.Text = filt.AnchorX.ToString();
                anchY.Text = filt.AnchorY.ToString();
                int l = filt.KernelMatrix.GetLength(0);
                int z = filt.KernelMatrix.GetLength(1);
                //x cordinate
                if (z == 3)
                    s3.IsSelected = true;
                else if (z == 1)
                    s1.IsSelected = true;
                else if (z == 5)
                    s5.IsSelected = true;
                else if (z == 7)
                    s7.IsSelected = true;
                else if (z == 9)
                    s9.IsSelected = true;
                //y cordinate
                if (l == 3)
                    sy3.IsSelected = true;
                else if (l == 1)
                    sy1.IsSelected = true;
                else if (l == 5)
                    sy5.IsSelected = true;
                else if (l == 7)
                    sy7.IsSelected = true;
                else if (l == 9)
                    sy9.IsSelected = true;
                int indexX = (l - 1) / 2;
                int indexY = (z - 1) / 2;
                int x = 0;
                int y = 0;

                MatrixText.Children.Clear();
                /*
                                for (int i = 4 - indexX; i < (4 + indexX + 1); i++)
                                {
                                    x = 0;
                                    for (int j = 4 - indexY; j < (4 + indexY + 1); j++)
                                    {
                                        var elem = MatrixText.Children.OfType<TextBox>().FirstOrDefault(k => k.Name == "text_" + i.ToString() + "_" + j.ToString());
                                        elem.Text = string.Empty;
                                        elem.Text = filt.KernelMatrix[y, x].ToString();
                                        x++;
                                    }
                                    y++;
                                }*/
                for (int i = 4 - indexX; i < (4 + indexX + 1); i++)
                {
                    x = 0;
                    for (int j = 4 - indexY; j < (4 + indexY + 1); j++)
                    {

                        TextBox textBOX = new TextBox();
                        textBOX.Name = "text_" + i.ToString() + "_" + j.ToString();
                        textBOX.Text = filt.KernelMatrix[y, x].ToString();
                        Grid.SetColumn(textBOX, j);
                        Grid.SetRow(textBOX, i);
                        MatrixText.Children.Add(textBOX);
                        x++;
                    }
                    y++;
                }
            }
        }
        private void Reset_Image(object sender, RoutedEventArgs e)
        {
            photo = new Bitmap(orginal);
            img.Source = BitmapToImageSource(photo);
            //img1.Source= BitmapToImageSource(photo);
            //img2.Source= BitmapToImageSource(photo);
            //img3.Source= BitmapToImageSource(photo);
        }
        private void Save_Image(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PNG Image|*.png|Bitmap Image|*.bmp|JPEG Image|*.jpeg";
            if (dialog.ShowDialog() == true)
            {
                switch (dialog.FilterIndex)
                {
                    case 0:
                        photo.Save(dialog.FileName, ImageFormat.Png);
                        break;
                    case 1:
                        photo.Save(dialog.FileName, ImageFormat.Bmp);
                        break;
                    case 2:
                        photo.Save(dialog.FileName, ImageFormat.Jpeg);
                        break;

                }
            }
        }
        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {

            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
        private void compute_divisor(object sender, RoutedEventArgs e)
        {
            if (matrixSizeX != 0 && matrixSizeY != 0)
            {
                int indexX = (matrixSizeX - 1) / 2;
                int indexY = (matrixSizeY - 1) / 2;
                double sum = 0;
                for (int i = 4 - indexY; i < (4 + indexY + 1); i++)
                {
                    for (int j = 4 - indexX; j < (4 + indexX + 1); j++)
                    {
                        var elem = MatrixText.Children.OfType<TextBox>().FirstOrDefault(k => k.Name == "text_" + i.ToString() + "_" + j.ToString());
                        if (Double.TryParse(elem.Text, out double el))
                        {
                            sum += el;
                        }
                        else
                        {
                            sum += 0;
                        }
                    }
                }
                divi.Text = sum.ToString();
            }
        }
        private void Save_Filter(object sender, RoutedEventArgs e)
        {
            try
            {
                double[,] kernel = new double[matrixSizeY, matrixSizeX];
                int indexX = (matrixSizeX - 1) / 2;
                int indexY = (matrixSizeY - 1) / 2;
                double div = 1;
                double o = 0;
                int oX = 0;
                int oY = 0;
                int x = 0;
                int y = 0;
                for (int i = 4 - indexY; i < (4 + indexY + 1); i++)
                {
                    for (int j = 4 - indexX; j < (4 + indexX + 1); j++)
                    {
                        var elem = MatrixText.Children.OfType<TextBox>().FirstOrDefault(k => k.Name == "text_" + i.ToString() + "_" + j.ToString());
                        if (Double.TryParse(elem.Text, out double el))
                        {
                            kernel[y, x] = el;
                        }
                        else
                        {
                            kernel[y, x] = 1;
                        }
                        x++;
                    }
                    y++;
                    x = 0;
                }

                if (Double.TryParse(divi.Text, out double di))
                {
                    div = di;
                }
                if (Double.TryParse(off.Text, out double of))
                {
                    o = of;
                }
                if (Int32.TryParse(anchX.Text, out int aX))
                {
                    oX = aX;
                }
                if (Int32.TryParse(anchY.Text, out int aY))
                {
                    oY = aY;
                }
                if (filt_name.Text == "")
                {
                    MessageBox.Show("Provide Filter Name");
                    return;
                }
                if (div == 0)
                {
                    MessageBox.Show("Divisor can' t equal 0");
                }
                AbstractConvolution newFilter = new NewFilter(filt_name.Text, div, o, kernel, oX, oY);
                for (int i = 0; i < ConvoList.Items.Count; i++)
                {
                    AbstractConvolution a = ConvoList.Items[i] as AbstractConvolution;
                    if (a.Name == filt_name.Text)
                    {
                        ConvoList.Items[i] = newFilter;
                        ConvoList1.Items[i] = newFilter;
                        return;
                    }
                }
                ConvoList.Items.Add(newFilter);
                ConvoList1.Items.Add(newFilter);

            }
            catch
            {
                MessageBox.Show("Provide Kernel Matrix");
            }
        }
        private void add_1x1(object sender, RoutedEventArgs e)
        {
            matrixSizeX = 1;
            add_textboxes();
            previousSizeX = 1;
        }

        private void add_3x3(object sender, RoutedEventArgs e)
        {
            matrixSizeX = 3;
            add_textboxes();
            previousSizeX = 3;
        }
        private void add_5x5(object sender, RoutedEventArgs e)
        {
            matrixSizeX = 5;
            add_textboxes();
            previousSizeX = 5;
        }
        private void add_7x7(object sender, RoutedEventArgs e)
        {
            matrixSizeX = 7;
            add_textboxes();
            previousSizeX = 7;
        }
        private void add_9x9(object sender, RoutedEventArgs e)
        {
            matrixSizeX = 9;
            add_textboxes();
            previousSizeX = 9;
        }
        private void add_1x1Y(object sender, RoutedEventArgs e)
        {
            matrixSizeY = 1;
            add_textboxes();
            previousSizeY = 1;
        }

        private void add_3x3Y(object sender, RoutedEventArgs e)
        {
            matrixSizeY = 3;
            add_textboxes();
            previousSizeY = 3;
        }
        private void add_5x5Y(object sender, RoutedEventArgs e)
        {
            matrixSizeY = 5;
            add_textboxes();
            previousSizeY = 5;
        }
        private void add_7x7Y(object sender, RoutedEventArgs e)
        {
            matrixSizeY = 7;
            add_textboxes();
            previousSizeY = 7;
        }
        private void add_9x9Y(object sender, RoutedEventArgs e)
        {
            matrixSizeY = 9;
            add_textboxes();
            previousSizeY = 9;
        }
        private void add_textboxes()
        {
            int indexX = (matrixSizeX - 1) / 2;
            int indexY = (matrixSizeY - 1) / 2;
            if ((previousSizeX != matrixSizeX && previousSizeX != 0) || (previousSizeY != 0 && previousSizeY != matrixSizeY))
            {
                MatrixText.Children.Clear();
            }
            for (int i = 4 - indexY; i < (4 + indexY + 1); i++)
            {
                for (int j = 4 - indexX; j < (4 + indexX + 1); j++)
                {
                    TextBox textBOX = new TextBox();
                    textBOX.Name = "text_" + i.ToString() + "_" + j.ToString();
                    textBOX.Text = "1";
                    Grid.SetColumn(textBOX, j);
                    Grid.SetRow(textBOX, i);
                    MatrixText.Children.Add(textBOX);
                }
            }
        }
        private void Inverse_Image(object sender, RoutedEventArgs e)
        {
            photo = FunctionalFilters.SetInverse(photo);
            img.Source = BitmapToImageSource(photo);
        }
        private void Bright_Image(object sender, RoutedEventArgs e)
        {
            photo = FunctionalFilters.SetBrightness(photo);
            img.Source = BitmapToImageSource(photo);
        }
        private void Contrast_Image(object sender, RoutedEventArgs e)
        {

            photo = FunctionalFilters.SetContrast(photo);
            img.Source = BitmapToImageSource(photo);
        }
        private void Gamma_Image(object sender, RoutedEventArgs e)
        {
            photo = FunctionalFilters.SetGamma(photo);
            img.Source = BitmapToImageSource(photo);
        }
        private void Gray_Image(object sender, RoutedEventArgs e)
        {
            photo = FunctionalFilters.GrayImage(photo);
            img.Source = BitmapToImageSource(photo);
        }
        private void Dith_Image(object sender, RoutedEventArgs e)
        {
            int div;
            if (Double.TryParse(DithText.Text, out double di))
            {
                div = (int)di;
            }
            else
            {
                div = 2;
            }
            DitheringAlgorithm dith = new DitheringAlgorithm();
            photo = dith.Ditering(photo, div);
            img.Source = BitmapToImageSource(photo);
        }
        private void Med_Image(object sender, RoutedEventArgs e)
        {
            int div;
            if (Double.TryParse(MedianText.Text, out double di))
            {

                div = (int)Math.Floor(Math.Log(di,2));
            }
            else
            {
                div = 1;
            }
            MedianCutAlgorithm Med = new MedianCutAlgorithm();
            photo = Med.MedianC(photo, div);
            img.Source = BitmapToImageSource(photo);
        }
        private void Ycbcr(object sender, RoutedEventArgs e)
        {
            Bitmap Y = new Bitmap(photo);
            Bitmap cb = new Bitmap(photo);
            Bitmap cr = new Bitmap(photo);
            System.Drawing.Color c;
            for (int i = 0; i < Y.Width; i++)
            {
                for (int j = 0; j < Y.Height; j++)
                {
                    c = Y.GetPixel(i, j);
                    int gray = (c.R + c.G + c.B) / 3;
                    Y.SetPixel(i, j, System.Drawing.Color.FromArgb(255, gray, gray, gray));
                    cb.SetPixel(i, j, System.Drawing.Color.FromArgb(255, 127, intepolation(255, 0, c.G), intepolation(0, 255, c.B)));
                    cr.SetPixel(i, j, System.Drawing.Color.FromArgb(255, intepolation(0, 255, c.R), intepolation(255, 0, c.G), 127));
                }
            }
            img.Source = BitmapToImageSource(photo);
            //img1.Source = BitmapToImageSource(Y);
            //img2.Source = BitmapToImageSource(cb);
            //img3.Source = BitmapToImageSource(cr);
        }
        private byte intepolation(int a, int b, int parm)
        {
            double val = parm / 255.0;
            return (byte)((1 - val) * a + val * b);
        }
        private void Ycbcr1(object sender, RoutedEventArgs e)
        {
            BitmapData sourceData = photo.LockBits(new Rectangle(0, 0,
                                        photo.Width, photo.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);



            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] pixelBuffer1 = new byte[sourceData.Stride * sourceData.Height];
            byte[] pixelBuffer2 = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            Marshal.Copy(sourceData.Scan0, pixelBuffer1, 0, pixelBuffer1.Length);
            Marshal.Copy(sourceData.Scan0, pixelBuffer2, 0, pixelBuffer2.Length);
            photo.UnlockBits(sourceData);
            for (int i = 0; i < photo.Height; i++)
            {
                for (int j = 0; j < photo.Width; j++)
                {
                    int byteOffset = ((i * sourceData.Stride) + j * 4);
                    int g1, g2, g3, d1;
                    g1 = pixelBuffer[byteOffset + 2];
                    g2 = pixelBuffer[byteOffset+1];
                    g3 = pixelBuffer[byteOffset];
                    d1 = (g1 + g2 + g3) / 3;
                    pixelBuffer[byteOffset]=(byte)d1;
                    pixelBuffer[byteOffset+1]=(byte)d1;
                    pixelBuffer[byteOffset+2]=(byte)d1;

                    pixelBuffer1[byteOffset] = intepolation(0, 255, pixelBuffer1[byteOffset]);
                    pixelBuffer1[byteOffset + 1] = intepolation(255, 0, pixelBuffer1[byteOffset + 1]);
                    pixelBuffer1[byteOffset + 2] = 127;

                    pixelBuffer2[byteOffset] = 127;
                    pixelBuffer2[byteOffset + 1] = intepolation(255, 0, pixelBuffer2[byteOffset + 1]);
                    pixelBuffer2[byteOffset + 2] = intepolation(0, 255, pixelBuffer2[byteOffset +2]);
                }
            }
            Bitmap resultBitmap = new Bitmap(photo.Width, photo.Height);
            Bitmap resultBitmap1 = new Bitmap(photo.Width, photo.Height);
            Bitmap resultBitmap2 = new Bitmap(photo.Width, photo.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData resultData1 = resultBitmap1.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData resultData2 = resultBitmap2.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            Marshal.Copy(pixelBuffer1, 0, resultData1.Scan0, pixelBuffer1.Length);
            Marshal.Copy(pixelBuffer2, 0, resultData2.Scan0, pixelBuffer2.Length);
            resultBitmap.UnlockBits(resultData);
            resultBitmap.UnlockBits(resultData1);
            resultBitmap.UnlockBits(resultData2);
            img.Source = BitmapToImageSource(photo);
            //img1.Source = BitmapToImageSource(resultBitmap);
            //img2.Source = BitmapToImageSource(resultBitmap1);
            //img3.Source = BitmapToImageSource(resultBitmap2);
        }
    }
}
