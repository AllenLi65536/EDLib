using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDLib.Pricing
{
    /// <summary>
    /// Used for Plain Vanilla options pricing
    /// </summary>
    public static class PlainVanilla
    {
        /// <summary>
        /// Price of call option
        /// </summary>
        /// <param name="S">Spot</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of underlying</param>
        /// <param name="time">Time to expiration</param>
        /// <returns>Price of call option</returns>
        public static double CallPrice(double S, double X, double r, double sigma, double time) {
            if (time > 0) {
                double time_sqrt = Math.Sqrt(time);
                double d1 = (Math.Log(S / X) + r * time) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
                double d2 = d1 - (sigma * time_sqrt);
                return S * NormDist.N(d1) - X * Math.Exp(-r * time) * NormDist.N(d2);
            }
            return Math.Max(0, S - X);

        }
        /// <summary>
        /// Price of put option
        /// </summary>
        /// <param name="S">Spot</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of underlying</param>
        /// <param name="time">Time to expiration</param>
        /// <returns>Price of put option</returns>
        public static double PutPrice(double S, double X, double r, double sigma, double time) {
            if (time > 0) {
                double time_sqrt = Math.Sqrt(time);
                double d1 = (Math.Log(S / X) + r * time) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
                double d2 = d1 - (sigma * time_sqrt);
                return -S * NormDist.N(-d1) + X * Math.Exp(-r * time) * NormDist.N(-d2);
            }
            return Math.Max(0, X - S);
        }
        /// <summary>
        /// Delta of call option
        /// </summary>
        /// <param name="S">Spot</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of underlying</param>
        /// <param name="time">Time to expiration</param>
        /// <returns>Delta of call option</returns>
        public static double CallDelta(double S, double X, double r, double sigma, double time) {
            double time_sqrt = Math.Sqrt(time);
            double d1 = (Math.Log(S / X) + r * time) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            return NormDist.N(d1);
        }
        /// <summary>
        /// Delta of put option
        /// </summary>
        /// <param name="S">Spot</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of underlying</param>
        /// <param name="time">Time to expiration</param>
        /// <returns>Delta of put option</returns>
        public static double PutDelta(double S, double X, double r, double sigma, double time) {
            double time_sqrt = Math.Sqrt(time);
            double d1 = (Math.Log(S / X) + r * time) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            return NormDist.N(d1) - 1;
        }
        /// <summary>
        /// Gamma of call option
        /// </summary>
        /// <param name="S">Spot</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of underlying</param>
        /// <param name="time">Time to expiration</param>
        /// <returns>Gamma of call option</returns>
        public static double CallGamma(double S, double X, double r, double sigma, double time) {
            double time_sqrt = Math.Sqrt(time);
            double d1 = (Math.Log(S / X) + r * time) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            return NormDist.n(d1) / (S * sigma * time_sqrt);
        }
        /// <summary>
        /// Gamma of put option
        /// </summary>
        /// <param name="S">Spot</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of underlying</param>
        /// <param name="time">Time to expiration</param>
        /// <returns>Gamma of put option</returns>
        public static double PutGamma(double S, double X, double r, double sigma, double time) {
            double time_sqrt = Math.Sqrt(time);
            double d1 = (Math.Log(S / X) + r * time) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            return NormDist.n(d1) / (S * sigma * time_sqrt);
        }
        /// <summary>
        /// Vega of call option
        /// </summary>
        /// <param name="S">Spot</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of underlying</param>
        /// <param name="time">Time to expiration</param>
        /// <returns>Vega of call option</returns>
        public static double CallVega(double S, double X, double r, double sigma, double time) {
            double time_sqrt = Math.Sqrt(time);
            double d1 = (Math.Log(S / X) + r * time) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            return S * time_sqrt * NormDist.n(d1);
        }
        /// <summary>
        /// Vega of put option
        /// </summary>
        /// <param name="S">Spot</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of underlying</param>
        /// <param name="time">Time to expiration</param>
        /// <returns>Vega of put option</returns>
        public static double PutVega(double S, double X, double r, double sigma, double time) {
            double time_sqrt = Math.Sqrt(time);
            double d1 = (Math.Log(S / X) + r * time) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            return S * time_sqrt * NormDist.n(d1);
        }
        /// <summary>
        /// Theta of call option
        /// </summary>
        /// <param name="S">Spot</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of underlying</param>
        /// <param name="time">Time to expiration</param>
        /// <returns>Theta of call option</returns>
        public static double CallTheta(double S, double X, double r, double sigma, double time) {
            if (time == 0) time = 0.0000000000001;
            double time_sqrt = Math.Sqrt(time);
            double d1 = (Math.Log(S / X) + r * time) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            double d2 = d1 - (sigma * time_sqrt);
            return -(S * sigma * NormDist.n(d1)) / (2 * time_sqrt) - r * X * Math.Exp(-r * time) * NormDist.N(d2);
        }
        /// <summary>
        /// Theta of put option
        /// </summary>
        /// <param name="S">Spot</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of underlying</param>
        /// <param name="time">Time to expiration</param>
        /// <returns>Theta of put option</returns>
        public static double PutTheta(double S, double X, double r, double sigma, double time) {
            double time_sqrt = Math.Sqrt(time);
            double d1 = (Math.Log(S / X) + r * time) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            double d2 = d1 - (sigma * time_sqrt);
            return -(S * sigma * NormDist.n(d1)) / (2 * time_sqrt) + r * X * Math.Exp(-r * time) * NormDist.N(-d2);
        }
        /// <summary>
        /// Rho of call option
        /// </summary>
        /// <param name="S">Spot</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of underlying</param>
        /// <param name="time">Time to expiration</param>
        /// <returns>Rho of call option</returns>
        public static double CallRho(double S, double X, double r, double sigma, double time) {
            double time_sqrt = Math.Sqrt(time);
            double d1 = (Math.Log(S / X) + r * time) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            double d2 = d1 - (sigma * time_sqrt);
            return X * time * Math.Exp(-r * time) * NormDist.N(d2);
        }
        /// <summary>
        /// Rho of put option
        /// </summary>
        /// <param name="S">Spot</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of underlying</param>
        /// <param name="time">Time to expiration</param>
        /// <returns>Rho of put option</returns>
        public static double PutRho(double S, double X, double r, double sigma, double time) {
            double time_sqrt = Math.Sqrt(time);
            double d1 = (Math.Log(S / X) + r * time) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            double d2 = d1 - (sigma * time_sqrt);
            return X * time * Math.Exp(-r * time) * NormDist.N(d2);
        }
        /// <summary>
        /// Calculate greeks of call option
        /// </summary>
        /// <param name="S">Spot</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of underlying</param>
        /// <param name="time">Time to expiration</param>        
        /// <param name="Delta">Output Δ</param>
        /// <param name="Gamma">Output Γ</param>
        /// <param name="Theta">Output Θ</param>
        /// <param name="Vega">Output ν</param>
        /// <param name="Rho">Output ρ</param>
        public static void CallGreeks(double S, double X, double r, double sigma, double time,
            out double Delta, out double Gamma, out double Theta, out double Vega, out double Rho) {
            if (time == 0) time = 0.0000000000001;
            double time_sqrt = Math.Sqrt(time);
            double d1 = (Math.Log(S / X) + r * time) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            double d2 = d1 - (sigma * time_sqrt);

            Delta = NormDist.N(d1);
            Gamma = NormDist.n(d1) / (S * sigma * time_sqrt);
            Theta = -(S * sigma * NormDist.n(d1)) / (2 * time_sqrt) - r * X * Math.Exp(-r * time) * NormDist.N(d2);
            Vega = S * time_sqrt * NormDist.n(d1);
            Rho = X * time * Math.Exp(-r * time) * NormDist.N(d2);
        }
        /// <summary>
        /// Calculate greeks of put option
        /// </summary>
        /// <param name="S">Spot</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">Sigma of underlying</param>
        /// <param name="time">Time to expiration</param>        
        /// <param name="Delta">Output Δ</param>
        /// <param name="Gamma">Output Γ</param>
        /// <param name="Theta">Output Θ</param>
        /// <param name="Vega">Output ν</param>
        /// <param name="Rho">Output ρ</param>
        public static void PutGreeks(double S, double X, double r, double sigma, double time,
           out double Delta, out double Gamma, out double Theta, out double Vega, out double Rho) {
            if (time == 0) time = 0.0000000000001;
            double time_sqrt = Math.Sqrt(time);
            double d1 = (Math.Log(S / X) + r * time) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            double d2 = d1 - (sigma * time_sqrt);

            Delta = NormDist.N(d1) - 1;
            Gamma = NormDist.n(d1) / (S * sigma * time_sqrt);
            Theta = -(S * sigma * NormDist.n(d1)) / (2 * time_sqrt) + r * X * Math.Exp(-r * time) * NormDist.N(-d2);
            Vega = S * time_sqrt * NormDist.n(d1);
            Rho = X * time * Math.Exp(-r * time) * NormDist.N(d2);
        }
        public static double CallIVBisection(double S, double X, double r, double time, double option_price) {
            // check for arbitrage violations: 
            // if price at almost zero volatility greater than price, return 0

            double sigma_low = 0.0001;
            double price = CallPrice(S, X, r, sigma_low, time);
            if (price > option_price) return 0.0;

            // simple binomial search for the implied volatility.
            // relies on the value of the option increasing in volatility
            const double ACCURACY = 1.0e-5; // make this smaller for higher accuracy
            const int MAX_ITERATIONS = 100;
            const double HIGH_VALUE = 1e10;
            const double ERROR = -1e38;//-1e40;

            // want to bracket sigma. first find a maximum sigma by finding a sigma
            // with a estimated price higher than the actual price.
            double sigma_high = 0.3;
            price = CallPrice(S, X, r, sigma_high, time);
            while (price < option_price) {
                sigma_high = 2.0 * sigma_high; // keep doubling.
                price = CallPrice(S, X, r, sigma_high, time);
                if (sigma_high > HIGH_VALUE) return ERROR; // panic, something wrong.
            }
            for (int i = 0; i < MAX_ITERATIONS; i++) {
                double sigma = (sigma_low + sigma_high) * 0.5;
                price = CallPrice(S, X, r, sigma, time);
                double test = (price - option_price);
                if (Math.Abs(test) < ACCURACY) { return sigma; };
                if (test < 0.0) { sigma_low = sigma; } else { sigma_high = sigma; }
            }
            return ERROR;
        }
        public static double PutIVBisection(double S, double X, double r, double time, double option_price) {
            // check for arbitrage violations: 
            // if price at almost zero volatility greater than price, return 0

            double sigma_low = 0.0001;
            double price = PutPrice(S, X, r, sigma_low, time);
            if (price > option_price)
                return 0.0;

            // simple binomial search for the implied volatility.
            // relies on the value of the option increasing in volatility
            const double ACCURACY = 1.0e-5; // make this smaller for higher accuracy
            const int MAX_ITERATIONS = 100;
            const double HIGH_VALUE = 1e10;
            const double ERROR = -1e38;//-1e40;

            // want to bracket sigma. first find a maximum sigma by finding a sigma
            // with a estimated price higher than the actual price.
            double sigma_high = 0.3;
            price = PutPrice(S, X, r, sigma_high, time);
            while (price < option_price) {
                sigma_high = 2.0 * sigma_high; // keep doubling.
                price = PutPrice(S, X, r, sigma_high, time);
                if (sigma_high > HIGH_VALUE)
                    return ERROR; // panic, something wrong.
            }
            for (int i = 0; i < MAX_ITERATIONS; i++) {
                double sigma = (sigma_low + sigma_high) * 0.5;
                price = PutPrice(S, X, r, sigma, time);
                double test = (price - option_price);
                if (Math.Abs(test) < ACCURACY)
                    return sigma;
                if (test < 0.0)
                    sigma_low = sigma;
                else
                    sigma_high = sigma;
            }
            return ERROR;
        }
        public static double CallIVNewton(double S, double X, double r, double time, double option_price) {
            // check for arbitrage violations:
            // if price at almost zero volatility greater than price, return 0
            double sigma_low = 1e-5;
            double price = CallPrice(S, X, r, sigma_low, time);
            if (price > option_price)
                return 0.0;

            const int MAX_ITERATIONS = 100;
            const double ACCURACY = 1.0e-4;
            double t_sqrt = Math.Sqrt(time);

            double sigma = (option_price / S) / (0.398 * t_sqrt);    // find initial value
            for (int i = 0; i < MAX_ITERATIONS; i++) {
                price = CallPrice(S, X, r, sigma, time);
                double diff = option_price - price;
                if (Math.Abs(diff) < ACCURACY)
                    return sigma;
                double d1 = (Math.Log(S / X) + r * time) / (sigma * t_sqrt) + 0.5 * sigma * t_sqrt;
                double vega = S * t_sqrt * NormDist.n(d1);
                sigma = sigma + diff / vega;
            }
            return -99e10;  // something screwy happened, should throw exception
        }
        public static double PutIVNewton(double S, double X, double r, double time, double option_price) {
            // check for arbitrage violations:
            // if price at almost zero volatility greater than price, return 0
            double sigma_low = 1e-5;
            double price = PutPrice(S, X, r, sigma_low, time);
            if (price > option_price) return 0.0;

            const int MAX_ITERATIONS = 100;
            const double ACCURACY = 1.0e-4;
            double t_sqrt = Math.Sqrt(time);

            double sigma = (option_price / S) / (0.398 * t_sqrt);    // find initial value
            for (int i = 0; i < MAX_ITERATIONS; i++) {
                price = PutPrice(S, X, r, sigma, time);
                double diff = option_price - price;
                if (Math.Abs(diff) < ACCURACY)
                    return sigma;
                double d1 = (Math.Log(S / X) + r * time) / (sigma * t_sqrt) + 0.5 * sigma * t_sqrt;
                double vega = S * t_sqrt * NormDist.n(d1);
                sigma = sigma + diff / vega;
            }
            return -99e10;  // something screwy happened, should throw exception
        }

    }
}
