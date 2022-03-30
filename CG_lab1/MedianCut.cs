using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_lab1
{
    public class MedianCut
    {
        public byte R;
        public byte G;
        public byte B;
        public MedianCut(byte _R, byte _G, byte _B)
        {
            R = _R;
            G = _G;
            B = _B;
        }
        private class SortRed : IComparer
        {
            public int Compare(object x, object y)
            {
                MedianCut R1 = (MedianCut)x;
                MedianCut R2 = (MedianCut)y;

                if (R1.R > R2.R)
                    return 1;

                if (R1.R < R2.R)
                    return -1;
                else
                    return 0;
            }
        }
        private class SortGreen : IComparer
        {
            public int Compare(object x, object y)
            {
                MedianCut R1 = (MedianCut)x;
                MedianCut R2 = (MedianCut)y;

                if (R1.G > R2.G)
                    return 1;

                if (R1.G < R2.G)
                    return -1;
                else
                    return 0;
            }
        }
        private class SortBlue : IComparer
        {
            public int Compare(object x, object y)
            {
                MedianCut R1 = (MedianCut)x;
                MedianCut R2 = (MedianCut)y;

                if (R1.B > R2.B)
                    return 1;

                if (R1.B < R2.B)
                    return -1;
                else
                    return 0;
            }
        }
        public static IComparer sortRAscending()
        {
            return (IComparer)new SortRed();
        }
        public static IComparer sortGAscending()
        {
            return (IComparer)new SortGreen();
        }
        public static IComparer sortBAscending()
        {
            return (IComparer)new SortBlue();
        }
    }
}
