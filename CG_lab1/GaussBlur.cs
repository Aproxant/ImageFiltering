using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_lab1
{
    class GaussBlur : AbstractConvolution
    {
        private string name = "Gauss Blur";

        public override string Name
        {
            get { return name; }
        }
        private double divisor = 8.0;
        public override double Divisor
        {
            get { return divisor; }
        }


        private double offset = 0.0;
        public override double Offset
        {
            get { return offset; }
        }


        private double[,] kernelMatrix =
            new double[,] { { 0, 1, 0, },
                        { 1, 4, 1, },
                        { 0, 1, 0, }, };


        public override double[,] KernelMatrix
        {
            get { return kernelMatrix; }
        }
        private int anchorX=0;
        public override int AnchorX
        {
            get { return anchorX; }
        }
        private int anchorY=0;
        public override int AnchorY
        {
            get { return anchorY; }
        }
    }
}
