using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_lab1
{
    class EdgeDetection : AbstractConvolution
    {
        private string name = "Edge Detection";

        public override string Name
        {
            get { return name; }
        }
        private double divisor = 1.0;
        public override double Divisor
        {
            get { return divisor; }
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

        private double offset = 0.0;
        public override double Offset
        {
            get { return offset; }
        }


        private double[,] kernelMatrix =
            new double[,] { { -1, -1, -1, },
                        { -1,  8, -1, },
                        { -1, -1, -1, }, };


        public override double[,] KernelMatrix
        {
            get { return kernelMatrix; }
        }
    }
}
