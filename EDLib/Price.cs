﻿using System;

namespace EDLib.Pricing
{
    /// <summary>
    /// Tools for options and warrants pricing
    /// </summary>
    internal class NamespaceDoc { }

    /// <summary>
    /// Normal distribution
    /// </summary>
    public static class NormDist
    {
        //private static readonly double sqrt2pi = Math.Sqrt(2.0 * Math.PI);
        /// <summary>
        /// Probability density function of standard normal distribution
        /// </summary>
        /// <param name="z">z value</param>
        /// <returns>Probability density</returns>
        public static double n(double z) {
            return (1.0 / Math.Sqrt(2.0 * Math.PI)) * Math.Exp(-0.5 * z * z);
        }

        /// <summary>
        /// Probability density function of normal distribution
        /// </summary>
        /// <param name="r">x value</param>
        /// <param name="mu">Mean</param>
        /// <param name="sigma">Standard Deviation</param>
        /// <returns>Probability density</returns>
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
        /// <param name="z">z value</param>
        /// <returns>Cumulative probability</returns>
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
            //double a = Math.Abs(z);
            double t = 1.0 / (1.0 + Math.Abs(z) * p);
            //double b = c2 * Math.Exp((-z) * (z / 2.0));
            double n = ((((b5 * t + b4) * t + b3) * t + b2) * t + b1) * t;
            n = 1.0 - c2 * Math.Exp(-z * (z / 2.0)) * n;
            if (z < 0.0)
                n = 1.0 - n;
            return n;
        }

        /// <summary>
        /// Cumulative density function (Phi function)
        /// </summary>
        /// <param name="z">z value</param>
        /// <returns>Cumulative probability</returns>
        public static double N1(double z) {
            // constants
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            // Save the sign of x
            int sign = 1;
            if (z < 0)
                sign = -1;
            z = Math.Abs(z) / Math.Sqrt(2.0);

            // A&S formula 7.1.26
            double t = 1.0 / (1.0 + p * z);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-z * z);

            return 0.5 * (1.0 + sign * y);
        }

        // Numerical approximation to the bivariate normal distribution, 
        //  as described e.g. in Hulls book

        /// <summary>
        /// Numerical approximation to the bivariate normal distribution, 
        /// </summary>
        /// <param name="x">x1 value</param>
        /// <param name="y">x2 value</param>
        /// <param name="aprime">a'</param>
        /// <param name="bprime">b'</param>
        /// <param name="rho">Correlation coefficient</param>
        /// <returns>Probability density</returns>
        private static double f(double x, double y, double aprime, double bprime, double rho) {
            double r = aprime * (2 * x - aprime) + bprime * (2 * y - bprime)
                + 2 * rho * (x - aprime) * (y - bprime);
            return Math.Exp(r);
        }

        /// <summary>
        /// Sign function
        /// </summary>
        /// <param name="x">x</param>
        /// <returns>-1 for negative number, 1 otherwise</returns>
        private static double sgn(double x) {
            //return Math.Sign(x);
            if (x >= 0.0)
                return 1.0;
            return -1.0;
        }

        /// <summary>
        /// Cumulative density function of bivariate standard normal distribution
        /// </summary>
        /// <param name="a">x1 value</param>
        /// <param name="b">x2 value</param>
        /// <param name="rho">Correlation coefficient</param>
        /// <returns>Cumulative probability</returns>
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
}
