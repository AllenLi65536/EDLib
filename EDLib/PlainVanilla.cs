using System;

namespace EDLib.Pricing
{
    /// <summary>
    /// Used for plain vanilla options pricing
    /// </summary>
    public static class PlainVanilla
    {
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
            double time_sqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            double d2 = d1 - (sigma * time_sqrt);
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
            double time_sqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            double d2 = d1 - (sigma * time_sqrt);
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
            double time_sqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
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
            double time_sqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
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
            double time_sqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            return NormDist.n(d1) / (S * sigma * time_sqrt);
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
            double time_sqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            return NormDist.n(d1) / (S * sigma * time_sqrt);
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
            double time_sqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            return S * time_sqrt * NormDist.n(d1);
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
            double time_sqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            return S * time_sqrt * NormDist.n(d1);
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
            double time_sqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            double d2 = d1 - (sigma * time_sqrt);
            return -(S * sigma * NormDist.n(d1)) / (2 * time_sqrt) - r * X * Math.Exp(-r * T) * NormDist.N(d2);
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
            double time_sqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            double d2 = d1 - (sigma * time_sqrt);
            return -(S * sigma * NormDist.n(d1)) / (2 * time_sqrt) + r * X * Math.Exp(-r * T) * NormDist.N(-d2);
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
            double time_sqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            double d2 = d1 - (sigma * time_sqrt);
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
            double time_sqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            double d2 = d1 - (sigma * time_sqrt);
            return X * T * Math.Exp(-r * T) * NormDist.N(d2);
        }
        /// <summary>
        /// Calculate greeks of call option
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
            ref double delta, ref double gamma, ref double theta, ref double vega, ref double rho) {
            if (T == 0)
                T = 0.0000000000001;
            double time_sqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            double d2 = d1 - (sigma * time_sqrt);
            
            delta = NormDist.N(d1);
            gamma = NormDist.n(d1) / (S * sigma * time_sqrt);
            theta = -(S * sigma * NormDist.n(d1)) / (2 * time_sqrt) - r * X * Math.Exp(-r * T) * NormDist.N(d2);
            vega = S * time_sqrt * NormDist.n(d1);
            rho = X * T * Math.Exp(-r * T) * NormDist.N(d2);
        }
        /// <summary>
        /// Calculate greeks of put option
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
            double time_sqrt = Math.Sqrt(T);
            double d1 = (Math.Log(S / X) + r * T) / (sigma * time_sqrt) + 0.5 * sigma * time_sqrt;
            double d2 = d1 - (sigma * time_sqrt);

            delta = NormDist.N(d1) - 1;
            gamma = NormDist.n(d1) / (S * sigma * time_sqrt);
            theta = -(S * sigma * NormDist.n(d1)) / (2 * time_sqrt) + r * X * Math.Exp(-r * T) * NormDist.N(-d2);
            vega = S * time_sqrt * NormDist.n(d1);
            rho = X * T * Math.Exp(-r * T) * NormDist.N(d2);
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

            double sigma_low = 0.0001;
            double price = CallPrice(S, X, r, sigma_low, T);
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
            double sigma_high = 0.3;
            price = CallPrice(S, X, r, sigma_high, T);
            while (price < optionPrice) {
                sigma_high *= 2.0 ; // keep doubling.
                price = CallPrice(S, X, r, sigma_high, T);
                if (sigma_high > HIGH_VALUE)
                    return ERROR; // panic, something wrong.
            }
            for (int i = 0; i < MAX_ITERATIONS; i++) {
                double sigma = (sigma_low + sigma_high) * 0.5;
                price = CallPrice(S, X, r, sigma, T);
                double test = (price - optionPrice);
                if (Math.Abs(test) < ACCURACY)
                    return sigma;
                if (test < 0.0)
                    sigma_low = sigma;
                else
                    sigma_high = sigma;
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

            double sigma_low = 0.0001;
            double price = PutPrice(S, X, r, sigma_low, T);
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
            double sigma_high = 0.3;
            price = PutPrice(S, X, r, sigma_high, T);
            while (price < optionPrice) {
                sigma_high = 2.0 * sigma_high; // keep doubling.
                price = PutPrice(S, X, r, sigma_high, T);
                if (sigma_high > HIGH_VALUE)
                    return ERROR; // panic, something wrong.
            }
            for (int i = 0; i < MAX_ITERATIONS; i++) {
                double sigma = (sigma_low + sigma_high) * 0.5;
                price = PutPrice(S, X, r, sigma, T);
                double test = (price - optionPrice);
                if (Math.Abs(test) < ACCURACY)
                    return sigma;
                if (test < 0.0)
                    sigma_low = sigma;
                else
                    sigma_high = sigma;
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
            double sigma_low = 1e-5;
            double price = CallPrice(S, X, r, sigma_low, T);
            if (price > optionPrice)
                return 0.0;

            const int MAX_ITERATIONS = 100;
            const double ACCURACY = 1.0e-4;
            double t_sqrt = Math.Sqrt(T);

            double sigma = (optionPrice / S) / (0.398 * t_sqrt);    // find initial value
            for (int i = 0; i < MAX_ITERATIONS; i++) {
                price = CallPrice(S, X, r, sigma, T);
                double diff = optionPrice - price;
                if (Math.Abs(diff) < ACCURACY)
                    return sigma;
                double d1 = (Math.Log(S / X) + r * T) / (sigma * t_sqrt) + 0.5 * sigma * t_sqrt;
                double vega = S * t_sqrt * NormDist.n(d1);
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
            double sigma_low = 1e-5;
            double price = PutPrice(S, X, r, sigma_low, T);
            if (price > optionPrice)
                return 0.0;

            const int MAX_ITERATIONS = 100;
            const double ACCURACY = 1.0e-4;
            double t_sqrt = Math.Sqrt(T);

            double sigma = (optionPrice / S) / (0.398 * t_sqrt);    // find initial value
            for (int i = 0; i < MAX_ITERATIONS; i++) {
                price = PutPrice(S, X, r, sigma, T);
                double diff = optionPrice - price;
                if (Math.Abs(diff) < ACCURACY)
                    return sigma;
                double d1 = (Math.Log(S / X) + r * T) / (sigma * t_sqrt) + 0.5 * sigma * t_sqrt;
                double vega = S * t_sqrt * NormDist.n(d1);
                sigma = sigma + diff / vega;
            }
            return -99e10;  // something screwy happened, should throw exception
        }

    }
}
