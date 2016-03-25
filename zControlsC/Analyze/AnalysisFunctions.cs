using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using zControlsC.Charts;

namespace zControlsC.Analyze
{
    public static class AnalysisFunctions
    {

        public static zLine GetLinearExpressionTrendY(zLine line)
        {
            return GetLinearExpressionTrendYInternal(line);
        }
        

        public static double GetStandardDeviation(double[] data)
        {
            return GetStandardDeviationInternal(data);
        }

        public static bool IsPrimeNumber(Int64 n)
        {
            return IsPrimeNumberInternal(n);
        }

        private static float GetSlope(zLine line)
        {
            float slope = 0;

            



            return slope;
        }

        private static float GetYIntercept(zLine line)
        {
            float yIntercept = 0;





            return yIntercept;
        }

        private static float GetCoEfficent(zLine line)
        {
            float CoEfficent = 0;




            return CoEfficent;
        }

        private static zLine GetLinearExpressionTrendYInternal(zLine line)
        {

            float slope = GetSlope(line);
            float yIntercept = GetYIntercept(line);

            return line;


        }


        private static double GetStandardDeviationInternal(double[] data)
        {
            
                double[] d = new double[data.GetLength(0)];
                double sd = 0.00;

                double total = 0.00;

                for (int i = 0; i <= data.GetLength(0) - 1; i++)
                {
                    total += data[i];
                }

                double average = (double)(total / data.GetLength(0));

                for (int i = 0; i <= data.GetLength(0) - 1; i++)
                {
                    sd += Math.Pow(data[i] - average, 2);
                }

                sd = Math.Sqrt(sd / (data.GetLength(0) - 1));

                return sd;
            

        }

        private static bool IsPrimeNumberInternal(Int64 n)
        {
            //If it's an even number and it's not 2 then we know it's not a prime number
            //2 is the only even prime number
            if (n != 2)
            {
                if (n % 2 == 0)
                {
                    return false;
                }
            }

            // i is the square root of the number we are checking
            // we only need to perform trial division up to the square root of our number
            Int64 i = (Int64)Math.Sqrt(n);
            
            //We can start on 3 because we already eliminated all the even numbers.
            //This eliminates one division calculation
            for (Int64 j = 3; j <= i; j++)
            {
                if (n % j == 0)
                {
                    return false;
                }
            }

            return true;

        }


        public static List<Int32> PrimeList(Int32 max)
        {

            var ret = new List<Int32>();

            //This is a byte array - one per number. A '0' means the number is prime,
            //and a '1' indicates that the number is a multiple of a prior prime.
            
            List<byte> b = new List<byte>();
            b.AddRange(new Byte[max]);
            
            Int32 n = max;
            Int32 lobound = 2;

            Int64 ubound = (Int64)Math.Sqrt(n);

            for (Int32 i = 2; i < ubound; i++)
            {
                if (b[i] == 0)
                {
                    for (Int32 z = (i * 2); z < n; z += i)
                    {
                        b[z] = 1;
                    }
                }
            }

            //Now that our byte array has only primes (up until our max) still
            //flagged as zero, we can build our return list.
            for (Int32 q = lobound; q < n; q++)
            {
                //if the byte at the corresponding array location
                //is zero, add this index to our return list
                if (b[q] == 0)
                {
                    ret.Add(q);
                }
            }

            return ret;

        }

        public static int GetStandardDeviationSigma(decimal value, decimal mean, decimal sd)
        {
            int sigma = 0;

            decimal sd1a = mean + (sd * 1);
            decimal sd1b = mean - (sd * 1);

            decimal sd2a = mean + (sd * 2);
            decimal sd2b = mean - (sd * 2);

            decimal sd3a = mean + (sd * 3);
            decimal sd3b = mean - (sd * 3);

            decimal sd4a = mean + (sd * 4);
            decimal sd4b = mean - (sd * 4);

            decimal sd5a = mean + (sd * 5);
            decimal sd5b = mean - (sd * 5);

            decimal sd6a = mean + (sd * 6);
            decimal sd6b = mean - (sd * 6);

            decimal sd7a = mean + (sd * 7);
            decimal sd7b = mean - (sd * 7);

            decimal sd8a = mean + (sd * 8);
            decimal sd8b = mean - (sd * 8);

            decimal sd9a = mean + (sd * 9);
            decimal sd9b = mean - (sd * 9);

            
            if ((value <= sd1a) && (value >= sd1b))
            {
                sigma = 1;
            }
            else if ((value <= sd2a) && (value >= sd2b))
            {
                sigma = 2;
            }
            else if ((value <= sd3a) && (value >= sd3b))
            {
                sigma = 3;
            }
            else if ((value <= sd4a) && (value >= sd4b))
            {
                sigma = 4;
            }
            else if ((value <= sd5a) && (value >= sd5b))
            {
                sigma = 5;
            }
            else if ((value <= sd6a) && (value >= sd6b))
            {
                sigma = 6;
            }
            else if ((value <= sd7a) && (value >= sd7b))
            {
                sigma = 7;
            }
            else if ((value <= sd8a) && (value >= sd8b))
            {
                sigma = 8;
            }
            else if ((value <= sd9a) && (value >= sd9b))
            {
                sigma = 9;
            }


            if (value < mean)
            {
                sigma = -sigma;
            }


            return sigma;


        }


        
    }
}
