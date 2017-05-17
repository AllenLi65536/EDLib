using System;

namespace EDLib.Pricing
{
    /// <summary>
    /// Use for options pricing
    /// </summary>
    internal class NamespaceDoc { }

    /// <summary>
    /// Normal distribution
    /// </summary>
    public static class NormDist
    {
        /// <summary>
        /// Probability density function of standard normal distribution
        /// </summary>
        /// <param name="z"></param>
        /// <returns>probability</returns>
        public static double n(double z) {
            return (1.0 / Math.Sqrt(2.0 * Math.PI)) * Math.Exp(-0.5 * z * z);
        }

        /// <summary>
        /// Probability density function of normal distribution
        /// </summary>
        /// <param name="r"></param>
        /// <param name="mu"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public static double n(double r, double mu, double sigma) {
            double nv = 1.0 / (Math.Sqrt(2.0 * Math.PI) * sigma);
            double z = (r - mu) / sigma;
            return nv * Math.Exp(-0.5 * z * z);
        }


        // cumulative univariate normal distribution.
        // This is a numerical approximation to the normal distribution.  
        // See Abramowitz and Stegun: Handbook of Mathemathical functions
        // for description.  The arguments to the functions are assumed 
        // normalized to a (0, 1) distribution. 

        /// <summary>
        /// Cumulative density function (Phi function)
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double N(double z) {
            double b1 = 0.31938153;
            double b2 = -0.356563782;
            double b3 = 1.781477937;
            double b4 = -1.821255978;
            double b5 = 1.330274429;
            double p = 0.2316419;
            //double c2 =  0.3989423; 
            double c2 = 0.3989422804;

            if (z > 6.0)
                return 1.0;  // this guards against overflow 
            if (z < -6.0)
                return 0.0;
            double a = Math.Abs(z);
            double t = 1.0 / (1.0 + a * p);
            double b = c2 * Math.Exp((-z) * (z / 2.0));
            double n = ((((b5 * t + b4) * t + b3) * t + b2) * t + b1) * t;
            n = 1.0 - b * n;
            if (z < 0.0)
                n = 1.0 - n;
            return n;
        }


        private static double CND1(double x) {
            // constants
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            // Save the sign of x
            int sign = 1;
            if (x < 0)
                sign = -1;
            x = Math.Abs(x) / Math.Sqrt(2.0);

            // A&S formula 7.1.26
            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return 0.5 * (1.0 + sign * y);
        }

        // Numerical approximation to the bivariate normal distribution, 
        //  as described e.g. in Hulls book

        /// <summary>
        ///  Numerical approximation to the bivariate normal distribution, 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="aprime"></param>
        /// <param name="bprime"></param>
        /// <param name="rho"></param>
        /// <returns></returns>
        private static double f(double x, double y, double aprime, double bprime, double rho) {
            double r = aprime * (2 * x - aprime) + bprime * (2 * y - bprime)
                + 2 * rho * (x - aprime) * (y - bprime);
            return Math.Exp(r);
        }

        /// <summary>
        ///  sign function
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static double sgn(double x) {
            return Math.Sign(x);
            /*if (x >= 0.0)
                return 1.0;
            return -1.0;*/
        }

        /// <summary>
        /// Cumulative density function of bivariate standard normal distribution
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="rho"></param>
        /// <returns></returns>
        public static double N(double a, double b, double rho) {
            if ((a <= 0.0) && (b <= 0.0) && (rho <= 0.0)) {
                double aprime = a / Math.Sqrt(2.0 * (1.0 - rho * rho));
                double bprime = b / Math.Sqrt(2.0 * (1.0 - rho * rho));
                double[] A = new double[4] { 0.3253030, 0.4211071, 0.1334425, 0.006374323 };
                double[] B = new double[4] { 0.1337764, 0.6243247, 1.3425378, 2.2626645 };
                double sum = 0;
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                        sum += A[i] * A[j] * f(B[i], B[j], aprime, bprime, rho);

                sum = sum * (Math.Sqrt(1.0 - rho * rho) / Math.PI);
                return sum;
            } else if (a * b * rho <= 0.0) {
                if ((a <= 0.0) && (b >= 0.0) && (rho >= 0.0)) 
                    return N(a) - N(a, -b, -rho);
                 else if ((a >= 0.0) && (b <= 0.0) && (rho >= 0.0)) 
                    return N(b) - N(-a, b, -rho);
                 else if ((a >= 0.0) && (b >= 0.0) && (rho <= 0.0)) 
                    return N(a) + N(b) - 1.0 + N(-a, -b, rho);
                
            } else if (a * b * rho >= 0.0) {
                double denum = Math.Sqrt(a * a - 2 * rho * a * b + b * b);
                double rho1 = ((rho * a - b) * sgn(a)) / denum;
                double rho2 = ((rho * b - a) * sgn(b)) / denum;
                double delta = (1.0 - sgn(a) * sgn(b)) / 4.0;
                return N(a, 0.0, rho1) + N(b, 0.0, rho2) - delta;
            }
            return -99.9; // should never get here
        }
    }
    public class Warrant
    {

    }


}
