using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_lab1
{
    class NewFilter : AbstractConvolution
    {
        public NewFilter(string _name,double _divisor,double _offset,double [,] _kernelMatrix, int _anchorX,int _anchorY)
        {
            name = _name;
            divisor = _divisor;
            offset = _offset;
            kernelMatrix = _kernelMatrix;
            anchorX = _anchorX;
            anchorY = _anchorY;
        }
        private string name;

        public override string Name
        {
            get { return name; }
        }
        private double divisor;
        public override double Divisor
        {
            get { return divisor; }
        }
        private double offset;
        public override double Offset
        {
            get { return offset; }
        }
        private int anchorX;
        public override int AnchorX
        {
            get { return anchorX; }
        }
        private int anchorY;
        public override int AnchorY
        {
            get { return anchorY; }
        }

        private double[,] kernelMatrix;

        public override double[,] KernelMatrix
        {
            get { return kernelMatrix; }
        }
    }
}
