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
    public abstract class AbstractConvolution
    {
        public abstract string Name
        {
            get;
        }
        public abstract double Divisor
        {
            get;
        }
        public abstract int AnchorX
        {
            get;
        }
        public abstract int AnchorY
        {
            get;
        }

        public abstract double Offset
        {
            get;
        }


        public abstract double[,] KernelMatrix
        {
            get;
        }
    }

}

