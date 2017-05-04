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

        private static int _fakeReading = 100;
        private static bool _fakeDown = true;

        public static double GetForce(int adc_value)
        {
            //if (_fakeReading >= 100 || (_fakeDown && _fakeReading > 0))
            //{
            //    _fakeReading--;
            //    _fakeDown = true;
            //}
            //else if (!_fakeDown || _fakeReading <= 0)
            //{
            //    _fakeReading++;
            //    _fakeDown = false;
            //}

            //return _fakeReading;

            double x = adc_value;
            double inches = 0.0003*x + 0.0387; // linearized equation from testing.

            return inches * LbsPerInch;
        }

        public static double GetForcePercentage(int adc_value)
        {
            return (((double)adc_value) / ((double)ADCMax) * 100);
        }

        public static double GetFlexion(int adc_value, bool degrees = true)
        {
            // y = mx + b
            // 90 = m(3100) + 40
            // m = (90 - 40) / 3100
            double x = adc_value;
            double b = 40;
            double m = (90 - b) / 3100;
            

            double y = m * x + b;

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
