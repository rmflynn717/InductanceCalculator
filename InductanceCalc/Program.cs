using System;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using static System.Math;
using static InductanceCalc.PhysicalConstants;

namespace InductanceCalc
{
    class Program
    {
        public static double[] OrderDoubles(double min, double max)
        {
            double[] x = new double[2];
            if (max < min)
            {
                x[0] = max;
                x[1] = min;
                return x;
            }
            x[0] = min;
            x[1] = max;
            return x;
        }

        public static double ReadDouble()
        {
            double parsedInput;
            string userInput = Console.ReadLine();
            while (!double.TryParse(userInput, out parsedInput))
            {
                Console.WriteLine("Invalid input, please enter a proper value:");
                Console.WriteLine();
                userInput = Console.ReadLine();
            }
            Console.WriteLine();
            return parsedInput;
        }

        public static void LineAntenna()
        {
            Console.WriteLine("Please enter a minimum radius (in meters):");
            Console.WriteLine();
            double minLineRadius = ReadDouble();

            //Gathering user input to build maxLineAntenna object
            Console.WriteLine("Please enter a maximum radius (in meters):\n");
            double maxLineRadius = ReadDouble();

            minLineRadius = OrderDoubles(minLineRadius, maxLineRadius)[0];
            maxLineRadius = OrderDoubles(minLineRadius, maxLineRadius)[1];

            //Printing min, max inductance from user input
            Console.WriteLine($"The inductance at {minLineRadius:0.00000} meters" +
                $" is {LineMagneticInductance(minLineRadius)} V-s/m^2");
            Console.WriteLine($"The inductance at {maxLineRadius:0.00000} meters" +
                $" is {LineMagneticInductance(maxLineRadius)} V-s/m^2");
            Console.WriteLine();

            double interval = 0.05;
            double intervalLineRadius = minLineRadius + interval;
            int i = 1;

            //Prints out inductance at user designated intervals
            while (intervalLineRadius < maxLineRadius)
            {
                Console.WriteLine($"The inductance at {intervalLineRadius:0.00000} " +
                    $"meters is " +
                    $"{LineMagneticInductance(intervalLineRadius)} V-s/m^2.");
                intervalLineRadius = minLineRadius + i * interval;
                i++;
            }
        }

        public static void CoilAntenna()
        {
            Console.WriteLine("Please enter a minimum separation distance (in meters):");
            Console.WriteLine();
            double minCoilDistance = ReadDouble();

            Console.WriteLine("Please enter a maximum separation distance (in meters):");
            Console.WriteLine();
            double maxCoilDistance = ReadDouble();

            minCoilDistance = OrderDoubles(minCoilDistance, maxCoilDistance)[0];
            maxCoilDistance = OrderDoubles(minCoilDistance, maxCoilDistance)[1];
            Console.WriteLine($"The inductance at {minCoilDistance:0.00000} meters" +
                $" is {CoilMagneticInductance(minCoilDistance)} V-s/m^2");
            Console.WriteLine($"The inductance at {maxCoilDistance:0.00000} meters" +
                $" is {CoilMagneticInductance(maxCoilDistance)} V-s/m^2");
            Console.WriteLine();
            Console.WriteLine($"The mutual inductance at {minCoilDistance:0.00000} meters" +
                $" is {TwoCoilMutualInductance(minCoilDistance)} V-s/m^2");
            Console.WriteLine($"The mutual inductance at {maxCoilDistance:0.00000} meters" +
                $" is {TwoCoilMutualInductance(maxCoilDistance)} V-s/m^2");
            Console.WriteLine();
            Console.WriteLine();
            double interval = 0.05;
            double intervalCoilDistance = minCoilDistance + interval;
            int i = 1;
            while (intervalCoilDistance < maxCoilDistance)
            {
                Console.WriteLine($"The inductance at {intervalCoilDistance:0.00000} " +
                    $"meters is "
                    + $"{CoilMagneticInductance(intervalCoilDistance)} V-s/m^2.");
                Console.WriteLine($"The mutual inductance at " +
                    $"{intervalCoilDistance:0.00000} meters is " +
                    $"{TwoCoilMutualInductance(intervalCoilDistance)} V-s/m^2.");
                Console.WriteLine();
                intervalCoilDistance = minCoilDistance + i * interval;
                i++;
            }
        }

        public static void RectangularAntenna()
        {
            Console.WriteLine("Please enter a minimum separation distance (in meters):");
            Console.WriteLine();
            double minRectDistance = ReadDouble();

            Console.WriteLine("Please enter a maximum separation distance (in meters):");
            Console.WriteLine();
            double maxRectDistance = ReadDouble();

            minRectDistance = OrderDoubles(minRectDistance, maxRectDistance)[0];
            maxRectDistance = OrderDoubles(minRectDistance, maxRectDistance)[1];
            Console.WriteLine($"The inductance at {minRectDistance:0.00000} meters" +
                $" is {RectangularMagneticInductance(minRectDistance)} V-s/m^2");
            Console.WriteLine($"The inductance at {maxRectDistance:0.00000} meters" +
                $" is {RectangularMagneticInductance(maxRectDistance)} V-s/m^2");
            Console.WriteLine();
            Console.WriteLine($"The mutual inductance at {minRectDistance:0.00000} meters" +
                $" is {RectangularAndLineInductance(minRectDistance)} V-s/m^2");
            Console.WriteLine($"The mutual inductance at {maxRectDistance:0.00000} meters" +
                $" is {RectangularAndLineInductance(maxRectDistance)} V-s/m^2");
            Console.WriteLine();
            Console.WriteLine();
            double interval = 0.05;
            double intervalRectDistance = minRectDistance + interval;
            int i = 1;
            while (intervalRectDistance < maxRectDistance)
            {
                Console.WriteLine($"The inductance at {intervalRectDistance:0.00000} " +
                    $"meters is "
                    + $"{RectangularMagneticInductance(intervalRectDistance)} " +
                    $"V-s/m^2.");
                Console.WriteLine($"The mutual inductance at " +
                    $"{intervalRectDistance:0.00000} " +
                    $"meters is {RectangularAndLineInductance(intervalRectDistance)} " +
                    $"V-s/m^2.");
                Console.WriteLine();
                intervalRectDistance = minRectDistance + i * interval;
                i++;
            }
        }

        public static double LineMagneticInductance(double radius)
        {
            double current = 0.01;

            return MagneticFieldConstant * current / (2 * PI * radius);
        }

        public static double CoilMagneticInductance(double distanceFromCenter)
        {
            double current = 0.02;
            int windingNumber = 100;
            double radius = 0.1;

            return MagneticFieldConstant * current * windingNumber * Pow(radius, 2) /
                 (2 * Pow(Pow(radius, 2) + Pow(distanceFromCenter, 2), 1.5));
        }

        public static double RectangularMagneticInductance(double distanceFromAntenna)
        {
            double width = 0.3;
            double length = 0.2;
            double current = 0.03;
            int nWindingNumber = 1;

            double distance = Sqrt(Pow(width / 2, 2) + Pow(length / 2, 2) +
                                                        Pow(distanceFromAntenna, 2));
            double inverseSquareA = Pow(width / 2, 2) + Pow(distanceFromAntenna, 2);
            double inverseSquareB = Pow(length / 2, 2) + Pow(distanceFromAntenna, 2);

            return MagneticFieldConstant * current * nWindingNumber * width * length /
                   (4 * PI * distance) * (1 / inverseSquareA + 1 / inverseSquareB);
        }

        public static double TwoCoilMutualInductance(double distanceBetweenCoils)
        {
            double nWindingNumber1 = 100;
            double radius1 = 0.1;
            double nWindingNumber2 = 200;
            double radius2 = 0.15;

            //Returns Mutual Inductance (in volts-seconds per meter squared)
            return 0.5 * MagneticFieldConstant * nWindingNumber1 *
                   Pow(radius1, 2) * nWindingNumber2 *
                   Pow(radius2, 2) * PI /
                   Sqrt(Pow(Pow(radius2, 2) +
                             Pow(distanceBetweenCoils, 2), 3));
        }

        public static double RectangularAndLineInductance(double distanceFromAntenna)
        {
            double width = 0.3;
            double length = 0.2;
            return 0.5 * MagneticFieldConstant / PI * width *
                        Log((distanceFromAntenna + length) / distanceFromAntenna);
        }

        public static string UserStartMenu()
        {
            //Menu Selection
            Console.WriteLine("Please select an antenna to compute inductance:");
            Console.WriteLine();
            Console.WriteLine("1. Press \'l\' for line antenna.");
            Console.WriteLine("2. Press \'c\' for coil antenna.");
            Console.WriteLine("3. Press \'r\' for rectangular antenna.");

            //Data gather from user
            string userSelection = Console.ReadLine();
            userSelection = userSelection.ToLower();

            //Input validation
            while (!userSelection.Equals("l") && !userSelection.Equals("c") &&
                   !userSelection.Equals("r"))
            {
                Console.WriteLine("Invalid input, please select an antenna to compute " +
                                    "inductance:");
                Console.WriteLine();
                Console.WriteLine("1. Press \'l\' for line antenna.");
                Console.WriteLine("2. Press \'c\' for coil antenna.");
                Console.WriteLine("3. Press \'r\' for rectangular antenna.");

                userSelection = Console.ReadLine();
                userSelection = userSelection.ToLower();
            }
            return userSelection;
        }

        public static void UserContinueMenu()
        {
            Console.WriteLine("Press \'n\' to enter new inputs or \'q\' to quit.");

            //Data gather from user
            string userSelection = Console.ReadLine();
            userSelection = userSelection.ToLower();

            //Input validation
            while (!userSelection.Equals("n") && !userSelection.Equals("q"))
            {
                Console.WriteLine("Press \'n\' to enter new inputs or \'q\' to quit.");

                userSelection = Console.ReadLine();
                userSelection = userSelection.ToLower();
            }

            while (userSelection.Equals("n"))
            {
                Main();
            }

            return;
        }

        private static void Main()
        {
            string userSelection = UserStartMenu();

            switch (userSelection)
            {
                case "l":

                    LineAntenna();
                    UserContinueMenu();
                    break;

                case "c":

                    CoilAntenna();
                    UserContinueMenu();
                    break;

                case "r":

                    RectangularAntenna();
                    UserContinueMenu();
                    break;

                default:
                    Console.WriteLine("User selected exit. Quitting now.");
                    break;
            }
        }
    }
}
