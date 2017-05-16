using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P17012CEF
{
    public static class ADCInterpreter
    {
        public const int ADCMax = 4095;
        public const int ADCMin = 0;

        public static double LbsPerInch = 6.29;

        private const bool _useFake = false;
        private static int _fakeReading = 100;
        private static bool _fakeDown = true;

        public static double GetForce(int adc_value)
        {
            if (_useFake) // simulation code or if bt is down and we still need to look at some data.  TODO: remove or create interface instead
            {
                if (_fakeReading >= 100 || (_fakeDown && _fakeReading > 0))
                {
                    _fakeReading--;
                    _fakeDown = true;
                }
                else if (!_fakeDown || _fakeReading <= 0)
                {
                    _fakeReading++;
                    _fakeDown = false;
                }

                return _fakeReading;
            }
            else
            {
                double x = adc_value;
                double inches = 0.0003 * x + 0.0387; // linearized equation from testing.  TODO: Implement more accurate distribution since our knee brace design is not perfectly linear and there are 3 "pulley" points

                return inches * LbsPerInch;
            }
        }

        public static double GetForcePercentage(int adc_value)
        {
            return (((double)adc_value) / ((double)ADCMax) * 100); // TODO: Get a better idea of exactly how far the range is we get on the ADC, since the implementation will not use the full range
        }

        public static double GetFlexion(int adc_value, bool degrees = true)
        {
            // use basic line equation based on what we observed, TODO: flesh this out to be more accurate.
            // y = mx + b
            // 90 = m(3100) + 40
            // m = (90 - 40) / 3100
            double x = adc_value;
            double b = 40;
            double m = (90 - b) / 3100;
            

            double y = m * x + b;

            // adjust the value if we need radians instead of degrees
            if (!degrees)
            {
                y = DegreesToRadians(y);
            }

            return y;
        }

        private static double DegreesToRadians(double degrees)
        {
            return (degrees / 180 * Math.PI);
        }
    }
}
