using System;
using static InductanceCalc.PhysicalConstants;

namespace InductanceCalc
{
    //Builds Line Antenna data structure
    struct LineAntenna
    {
        public double Current;

        public double Radius;

        public LineAntenna(double current, double radius)
        {
            this.Current = current;
            this.Radius = radius;
        }
    }
}
