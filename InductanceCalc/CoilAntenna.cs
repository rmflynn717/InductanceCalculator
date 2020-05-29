using System;
using static System.Math;
using static InductanceCalc.PhysicalConstants;

namespace InductanceCalc
{
    //Builds CoilAntenna data structure
    struct CoilAntenna
    {
        public double Current;
        public int WindingNumber;
        public double CoilRadius;
        public double CenterCoilDistance;

        public CoilAntenna(int windingNumber, double current, double coilRadius,
            double centerCoilDistance)
        {
            this.WindingNumber = windingNumber;
            this.Current = current;
            this.CoilRadius = coilRadius;
            this.CenterCoilDistance = centerCoilDistance;
        }
    }
}
