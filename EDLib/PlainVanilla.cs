using System;

namespace EDLib.Pricing.Option
{
    /// <summary>
    /// Use for options pricing
    /// </summary>
    internal class NamespaceDoc { }
    /// <summary>
    /// Used for plain vanilla options pricing
    /// </summary>
    public static class PlainVanilla
    {
        /// <summary>
        /// Bitmask that shows type of Greeks to be calculated
        /// </summary>
        [Flags]
        public enum Greeks
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// Δ
            /// </summary>
            Delta = 1,
            /// <summary>
            /// Γ
            /// </summary>
            Gamma = 2,
            /// <summary>
            /// Θ
            /// </summary>
            Theta = 4,
            /// <summary>
            /// ν
            /// </summary>
            Vega = 8,
            /// <summary>
            /// Oftenly used: Δ, Γ, Θ, ν
            /// </summary>
            Regular = Delta | Gamma | Theta | Vega,
            /// <summary>
            /// ρ
            /// </summary>
            Rho = 16,
            /// <summary>
            /// All 5 Greeks
            /// </summary>
            All = Delta | Gamma | Theta | Vega | Rho
        }
        /// <summary>
        /// Price of call option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <returns>Price of call option</returns>
        public static double CallPrice(double S, double X, double r, double sigma, double T) {
            if (T <= 0)
                return Math.Max(0, S - X);
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;
            double d2 = d1 - (sigma * timeSqrt);
            return S * NormDist.N(d1) - X * Math.Exp(-r * T) * NormDist.N(d2);
        }
        /// <summary>
        /// Price of put option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <returns>Price of put option</returns>
        public static double PutPrice(double S, double X, double r, double sigma, double T) {
            if (T <= 0)
                return Math.Max(0, X - S);
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;
            double d2 = d1 - (sigma * timeSqrt);
            return -S * NormDist.N(-d1) + X * Math.Exp(-r * T) * NormDist.N(-d2);
        }
        /// <summary>
        /// Δ of call option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <returns>Δ of call option</returns>
        public static double CallDelta(double S, double X, double r, double sigma, double T) {
            if (T <= 0)
                return 1;
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;
            return NormDist.N(d1);
        }
        /// <summary>
        /// Δ of put option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <returns>Δ of put option</returns>
        public static double PutDelta(double S, double X, double r, double sigma, double T) {
            if (T <= 0)
                return -1;
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;
            return NormDist.N(d1) - 1;
        }
        /// <summary>
        /// Γ of call option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <returns>Γ of call option</returns>
        public static double CallGamma(double S, double X, double r, double sigma, double T) {
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;
            return NormDist.n(d1) / (S * sigma * timeSqrt);
        }
        /// <summary>
        /// Γ of put option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <returns>Γ of put option</returns>
        public static double PutGamma(double S, double X, double r, double sigma, double T) {
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;
            return NormDist.n(d1) / (S * sigma * timeSqrt);
        }        
        /// <summary>
        /// Θ of call option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <returns>Θ of call option</returns>
        public static double CallTheta(double S, double X, double r, double sigma, double T) {
            if (T == 0)
                T = 0.0000000000001;
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;
            double d2 = d1 - (sigma * timeSqrt);
            return -(S * sigma * NormDist.n(d1)) / (2 * timeSqrt) - r * X * Math.Exp(-r * T) * NormDist.N(d2);
        }
        /// <summary>
        /// Θ of put option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <returns>Θ of put option</returns>
        public static double PutTheta(double S, double X, double r, double sigma, double T) {
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;
            double d2 = d1 - (sigma * timeSqrt);
            return -(S * sigma * NormDist.n(d1)) / (2 * timeSqrt) + r * X * Math.Exp(-r * T) * NormDist.N(-d2);
        }
        /// <summary>
        /// ν of call option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <returns>ν of call option</returns>
        public static double CallVega(double S, double X, double r, double sigma, double T) {
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;
            return S * timeSqrt * NormDist.n(d1);
        }
        /// <summary>
        /// ν of put option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <returns>ν of put option</returns>
        public static double PutVega(double S, double X, double r, double sigma, double T) {
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;
            return S * timeSqrt * NormDist.n(d1);
        }
        /// <summary>
        /// ρ of call option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <returns>ρ of call option</returns>
        public static double CallRho(double S, double X, double r, double sigma, double T) {
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;
            double d2 = d1 - (sigma * timeSqrt);
            return X * T * Math.Exp(-r * T) * NormDist.N(d2);
        }
        /// <summary>
        /// ρ of put option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <returns>ρ of put option</returns>
        public static double PutRho(double S, double X, double r, double sigma, double T) {
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;
            double d2 = d1 - (sigma * timeSqrt);
            return -X * T * Math.Exp(-r * T) * NormDist.N(-d2);
        }
        /// <summary>
        /// Calculate Greeks of call option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of spot price</param>
        /// <param name="T">Time to maturity</param>        
        /// <param name="delta">Output Δ</param>
        /// <param name="gamma">Output Γ</param>
        /// <param name="theta">Output Θ</param>
        /// <param name="vega">Output ν</param>
        /// <param name="rho">Output ρ</param>
        public static void CallGreeks(double S, double X, double r, double sigma, double T,
            out double delta, out double gamma, out double theta, out double vega, out double rho) {
            if (T == 0)
                T = 0.0000000000001;
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;
            double d2 = d1 - (sigma * timeSqrt);

            delta = NormDist.N(d1);
            gamma = NormDist.n(d1) / (S * sigma * timeSqrt);
            theta = -(S * sigma * NormDist.n(d1)) / (2 * timeSqrt) - r * X * Math.Exp(-r * T) * NormDist.N(d2);
            vega = S * timeSqrt * NormDist.n(d1);
            rho = X * T * Math.Exp(-r * T) * NormDist.N(d2);
        }
        /// <summary>
        /// Calculate specified Greeks of call option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="bitmask">Specify which greeks to calculate</param>
        /// <returns>Always returns an double array with 5 elements, having delta, gamma, theta, vega, rho, accordingly. Unspecified Greeks would be set to 0.</returns>
        /// <exception cref="ArgumentException">Nothing is set in bitmask</exception>
        /// <example>
        /// <code>
        /// double[] greeks = PlainVanilla.CallGreeks(150, 50, 0.025, 0.5, 0.56, PlainVanilla.Greeks.Delta | PlainVanilla.Greeks.Gamma | PlainVanilla.Greeks.Rho);
        /// for (int i = 0; i &lt; greeks.Length; i++)
        ///     Console.WriteLine(greeks[i]);
        /// //greeks[0]: Delta, greeks[1]: Gamma, greeks[4]: Rho, greeks[2]: 0, greeks[3]: 0
        /// </code>
        /// </example>
        public static double[] CallGreeks(double S, double X, double r, double sigma, double T, Greeks bitmask) {
            if (bitmask == Greeks.None)
                throw new ArgumentException("Nothing is set in bitmask.", "bitmask");

            double[] ret = new double[5];
            //int size = 0;
            if (T == 0)
                T = 0.0000000000001;
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;

            double nd1 = 0.0; //Compute them only when needed
            double d2 = 0.0; 
            double nd2 = 0.0;

            if ((bitmask & Greeks.Delta) != Greeks.None)
                ret[0] = NormDist.N(d1);

            if ((bitmask & Greeks.Gamma) != Greeks.None) {
                nd1 = NormDist.n(d1);
                ret[1] = nd1 / (S * sigma * timeSqrt);
            }

            if ((bitmask & Greeks.Theta) != Greeks.None) {
                d2 = d1 - (sigma * timeSqrt);
                nd2 = X * Math.Exp(-r * T) * NormDist.N(d2);
                if (nd1 == 0.0)
                    nd1 = NormDist.n(d1);
                ret[2] = -(S * sigma * nd1) / (2 * timeSqrt) - r * nd2;
            }
            if ((bitmask & Greeks.Vega) != Greeks.None) {
                if (nd1 == 0.0)
                    nd1 = NormDist.n(d1);
                ret[3] = S * timeSqrt * nd1;
            }

            if ((bitmask & Greeks.Rho) != Greeks.None) {
                if (d2 == 0.0) {
                    d2 = d1 - (sigma * timeSqrt);
                    nd2 = X * Math.Exp(-r * T) * NormDist.N(d2);
                }
                ret[4] = T * nd2;
            }
            //if (size != 5)
            //    Array.Resize(ref ret, size);
            return ret;
        }
        /// <summary>
        /// Calculate Greeks of put option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of spot price</param>
        /// <param name="T">Time to maturity</param>        
        /// <param name="delta">Output Δ</param>
        /// <param name="gamma">Output Γ</param>
        /// <param name="theta">Output Θ</param>
        /// <param name="vega">Output ν</param>
        /// <param name="rho">Output ρ</param>
        public static void PutGreeks(double S, double X, double r, double sigma, double T,
           out double delta, out double gamma, out double theta, out double vega, out double rho) {
            if (T == 0)
                T = 0.0000000000001;
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;
            double d2 = d1 - (sigma * timeSqrt);

            delta = NormDist.N(d1) - 1;
            gamma = NormDist.n(d1) / (S * sigma * timeSqrt);
            theta = -(S * sigma * NormDist.n(d1)) / (2 * timeSqrt) + r * X * Math.Exp(-r * T) * NormDist.N(-d2);
            vega = S * timeSqrt * NormDist.n(d1);
            rho = -X * T * Math.Exp(-r * T) * NormDist.N(-d2);
        }
        /// <summary>
        /// Calculate specified Greeks of put option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="bitmask">Specify which greeks to calculate</param>
        /// <returns>Always returns an double array with 5 elements, having delta, gamma, theta, vega, rho, accordingly. Unspecified Greeks would be set to 0.</returns>
        /// <exception cref="ArgumentException">Nothing is set in bitmask</exception>
        /// <example>
        /// <code>
        /// double[] greeks = PlainVanilla.PutGreeks(150, 50, 0.025, 0.5, 0.56, PlainVanilla.Greeks.Regular);
        /// for (int i = 0; i &lt; greeks.Length; i++)
        ///     Console.WriteLine(greeks[i]);
        /// //greeks[0]: Delta, greeks[1]: Gamma, greeks[2]: Theta, greeks[3]: Vega, greeks[4]: 0 
        /// </code>
        /// </example>
        public static double[] PutGreeks(double S, double X, double r, double sigma, double T, Greeks bitmask) {
            if (bitmask == Greeks.None)
                throw new ArgumentException("Nothing is set in bitmask.", "bitmask");

            double[] ret = new double[5];
            //int size = 0;
            if (T == 0)
                T = 0.0000000000001;
            double timeSqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * timeSqrt) + 0.5 * sigma * timeSqrt;

            double nd1 = 0.0;//Compute them only when needed
            double d2 = 0.0; 
            double nd2 = 0.0;

            if ((bitmask & Greeks.Delta) != Greeks.None)
                ret[0] = NormDist.N(d1) - 1;

            if ((bitmask & Greeks.Gamma) != Greeks.None) {
                nd1 = NormDist.n(d1);
                ret[1] = nd1 / (S * sigma * timeSqrt);
            }

            if ((bitmask & Greeks.Theta) != Greeks.None) {
                d2 = d1 - (sigma * timeSqrt);
                nd2 = X * Math.Exp(-r * T);
                if (nd1 == 0.0)
                    nd1 = NormDist.n(d1);
                ret[2] = -(S * sigma * nd1) / (2 * timeSqrt) + r * nd2 * NormDist.N(d2);
            }
            if ((bitmask & Greeks.Vega) != Greeks.None) {
                if (nd1 == 0.0)
                    nd1 = NormDist.n(d1);
                ret[3] = S * timeSqrt * nd1;
            }

            if ((bitmask & Greeks.Rho) != Greeks.None) {
                if (d2 == 0.0) {
                    d2 = d1 - (sigma * timeSqrt);
                    nd2 = X * Math.Exp(-r * T);
                }
                ret[4] = -T * nd2 * NormDist.N(-d2);
            }           
            return ret;
        }
        /// <summary>
        /// Implied volatility of call option by bisection method
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>        
        /// <param name="T">Time to maturity</param>  
        /// <param name="optionPrice">Option's price</param>
        /// <returns>Implied volatility</returns>
        public static double CallIVBisection(double S, double X, double r, double T, double optionPrice) {
            // check for arbitrage violations: 
            // if price at almost zero volatility greater than price, return 0

            double sigmaLow = 0.0001;
            double price = CallPrice(S, X, r, sigmaLow, T);
            if (price > optionPrice)
                return 0.0;

            // simple binomial search for the implied volatility.
            // relies on the value of the option increasing in volatility
            const double ACCURACY = 1.0e-5; // make this smaller for higher accuracy
            const int MAX_ITERATIONS = 100;
            const double HIGH_VALUE = 1e10;
            const double ERROR = -1e38;//-1e40;

            // want to bracket sigma. first find a maximum sigma by finding a sigma
            // with a estimated price higher than the actual price.
            double sigmaHigh = 0.3;
            price = CallPrice(S, X, r, sigmaHigh, T);
            while (price < optionPrice) {
                sigmaHigh *= 2.0; // keep doubling.
                price = CallPrice(S, X, r, sigmaHigh, T);
                if (sigmaHigh > HIGH_VALUE)
                    return ERROR; // panic, something wrong.
            }
            for (int i = 0; i < MAX_ITERATIONS; i++) {
                double sigma = (sigmaLow + sigmaHigh) * 0.5;
                price = CallPrice(S, X, r, sigma, T);
                double test = (price - optionPrice);
                if (Math.Abs(test) < ACCURACY)
                    return sigma;
                if (test < 0.0)
                    sigmaLow = sigma;
                else
                    sigmaHigh = sigma;
            }
            return ERROR;
        }
        /// <summary>
        /// Implied volatility of put option by bisection method
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>        
        /// <param name="T">Time to maturity</param>  
        /// <param name="optionPrice">Option's price</param>
        /// <returns>Implied volatility</returns>
        public static double PutIVBisection(double S, double X, double r, double T, double optionPrice) {
            // check for arbitrage violations: 
            // if price at almost zero volatility greater than price, return 0

            double sigmaLow = 0.0001;
            double price = PutPrice(S, X, r, sigmaLow, T);
            if (price > optionPrice)
                return 0.0;

            // simple binomial search for the implied volatility.
            // relies on the value of the option increasing in volatility
            const double ACCURACY = 1.0e-5; // make this smaller for higher accuracy
            const int MAX_ITERATIONS = 100;
            const double HIGH_VALUE = 1e10;
            const double ERROR = -1e38;//-1e40;

            // want to bracket sigma. first find a maximum sigma by finding a sigma
            // with a estimated price higher than the actual price.
            double sigmaHigh = 0.3;
            price = PutPrice(S, X, r, sigmaHigh, T);
            while (price < optionPrice) {
                sigmaHigh = 2.0 * sigmaHigh; // keep doubling.
                price = PutPrice(S, X, r, sigmaHigh, T);
                if (sigmaHigh > HIGH_VALUE)
                    return ERROR; // panic, something wrong.
            }
            for (int i = 0; i < MAX_ITERATIONS; i++) {
                double sigma = (sigmaLow + sigmaHigh) * 0.5;
                price = PutPrice(S, X, r, sigma, T);
                double test = (price - optionPrice);
                if (Math.Abs(test) < ACCURACY)
                    return sigma;
                if (test < 0.0)
                    sigmaLow = sigma;
                else
                    sigmaHigh = sigma;
            }
            return ERROR;
        }
        /// <summary>
        /// Implied volatility of call option by Newton method
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>        
        /// <param name="T">Time to maturity</param>  
        /// <param name="optionPrice">Option's price</param>
        /// <returns>Implied volatility</returns>
        public static double CallIVNewton(double S, double X, double r, double T, double optionPrice) {
            // check for arbitrage violations:
            // if price at almost zero volatility greater than price, return 0
            double sigmaLow = 1e-5;
            double price = CallPrice(S, X, r, sigmaLow, T);
            if (price > optionPrice)
                return 0.0;

            const int MAX_ITERATIONS = 100;
            const double ACCURACY = 1.0e-4;
            double tSqrt = Math.Sqrt(T);

            double sigma = (optionPrice / S) / (0.398 * tSqrt);    // find initial value
            for (int i = 0; i < MAX_ITERATIONS; i++) {
                price = CallPrice(S, X, r, sigma, T);
                double diff = optionPrice - price;
                if (Math.Abs(diff) < ACCURACY)
                    return sigma;
                double d1 = (Math.Log(S / X) + r * T) / (sigma * tSqrt) + 0.5 * sigma * tSqrt;
                double vega = S * tSqrt * NormDist.n(d1);
                sigma = sigma + diff / vega;
            }
            return -99e10;  // something screwy happened, should throw exception
        }
        /// <summary>
        /// Implied volatility of put option by Newton method
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>        
        /// <param name="T">Time to maturity</param>  
        /// <param name="optionPrice">Option's price</param>
        /// <returns>Implied volatility</returns>
        public static double PutIVNewton(double S, double X, double r, double T, double optionPrice) {
            // check for arbitrage violations:
            // if price at almost zero volatility greater than price, return 0
            double sigmaLow = 1e-5;
            double price = PutPrice(S, X, r, sigmaLow, T);
            if (price > optionPrice)
                return 0.0;

            const int MAX_ITERATIONS = 100;
            const double ACCURACY = 1.0e-4;
            double tSqrt = Math.Sqrt(T);

            double sigma = (optionPrice / S) / (0.398 * tSqrt);    // find initial value
            for (int i = 0; i < MAX_ITERATIONS; i++) {
                price = PutPrice(S, X, r, sigma, T);
                double diff = optionPrice - price;
                if (Math.Abs(diff) < ACCURACY)
                    return sigma;
                double d1 = (Math.Log(S / X) + r * T) / (sigma * tSqrt) + 0.5 * sigma * tSqrt;
                double vega = S * tSqrt * NormDist.n(d1);
                sigma = sigma + diff / vega;
            }
            return -99e10;  // something screwy happened, should throw exception
        }

    }
}
