using System;
using static System.Math;
using static InductanceCalc.PhysicalConstants;

namespace InductanceCalc
{
    //Builds RectangularAntenna data structure
    struct RectangularAntenna
    {
        public int WindingNumber;
        public double Current;
        public double AntennaWidth;
        public double AntennaLength;
        public double DistanceFromAntenna;

        public RectangularAntenna(int windingNumber, double current,
            double antennaWidth, double antennaLength, double distanceFromAntenna)
        {
            this.WindingNumber = windingNumber;
            this.Current = current;
            this.AntennaWidth = antennaWidth;
            this.AntennaLength = antennaLength;
            this.DistanceFromAntenna = distanceFromAntenna;
        }
    }
}
