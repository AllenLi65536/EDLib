using System;

namespace EDLib.Pricing
{
    /// <summary>
    /// Used for barrier options pricing
    /// </summary>
    public static class BarrierOption
    {
        /// <summary>
        /// Price of up and out call option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="H">Barrier price</param>
        /// <param name="n">N of trinominal tree</param>
        /// <returns>Price of up and out call option</returns>
        public static double CallUpOutPrice(double S, double X, double r, double sigma, double T, double H, int n) {
            //Price
            if (S == 0 || X == 0 || H == 0 || r == 0 || T == 0 || sigma == 0 || n == 0)
                return 0;
                       
            double dt = T / (double) n;
            int h = (int) Math.Floor(Math.Log(H / S) / (sigma * Math.Sqrt(dt)));
            if (h <= 0 || h > n)
                return -1;

            double lambda = Math.Log(H / S) / (h * sigma * Math.Sqrt(dt));

            if (h < 1) {
                n = (int) Math.Ceiling(sigma * sigma * T / Math.Pow(Math.Log(S / H), 2));
                dt = T / n;
                h = (int) Math.Floor(Math.Log(H / S) / (sigma * Math.Sqrt(dt)));
                lambda = Math.Log(H / S) / (h * sigma * Math.Sqrt(dt));
            }

            double R = Math.Exp(r * dt);
            double pu = 1.0 / (2.0 * lambda * lambda) + (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pd = 1.0 / (2.0 * lambda * lambda) - (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pm = 1.0 - pu - pd;
            double u = Math.Exp(lambda * sigma * Math.Sqrt(dt));

            double[] P = new double[2 * n + 1];

            for (int i = n - h; i <= 2 * n; i++) 
                P[i] = Math.Max(0, S * Math.Pow(u, n - i) - X);

            for (int j = n - 1; j >= h; j--) {
                for (int i = j - h + 1; i <= 2 * j; i++)
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, S * Math.Pow(u, j - i) - X);
                P[j - h] = H - X;
            }

            for (int j = h - 1; j >= 0; j--)
                for (int i = 0; i <= 2 * j; i++)
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, S * Math.Pow(u, j - i) - X);

            return P[0];
        }

        /// <summary>
        /// Δ of up and out call option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="H">Barrier price</param>
        /// <param name="n">N of trinominal tree</param>
        /// <returns>Δ of up and out call option</returns>
        public static double CallUpOutDelta(double S, double X, double r, double sigma, double T, double H, int n) {
            if (S == 0 || X == 0 || H == 0 || r == 0 || T == 0 || sigma == 0 || n == 0)
                return 0;
                       
            double dt = T / (double) n;

            int h = (int) Math.Floor(Math.Log(H / S) / (sigma * Math.Sqrt(dt)));

            if (h <= 0 || h > n)
                return -1;

            double lambda = Math.Log(H / S) / (h * sigma * Math.Sqrt(dt));

            if (h < 1) {
                n = (int) Math.Ceiling(sigma * sigma * T / Math.Pow(Math.Log(S / H), 2));
                dt = T / n;
                h = (int) Math.Floor(Math.Log(H / S) / (sigma * Math.Sqrt(dt)));
                lambda = Math.Log(H / S) / (h * sigma * Math.Sqrt(dt));
            }

            double R = Math.Exp(r * dt);
            double pu = 1.0 / (2.0 * lambda * lambda) + (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pd = 1.0 / (2.0 * lambda * lambda) - (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pm = 1.0 - pu - pd;
            double u = Math.Exp(lambda * sigma * Math.Sqrt(dt));

            double[] P = new double[2 * n + 1];

            for (int i = n - h; i <= 2 * n; i++) 
                P[i] = Math.Max(0, S * Math.Pow(u, n - i) - X);        

            for (int j = n - 1; j >= h; j--) {
                for (int i = j - h + 1; i <= 2 * j; i++) 
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, S * Math.Pow(u, j - i) - X);                
                P[j - h] = H - X;
            }

            for (int j = h - 1; j > 0; j--) 
                for (int i = 0; i <= 2 * j; i++) 
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, S * Math.Pow(u, j - i) - X);
                        
            double dif = S * (u - 1.0 / u);
            return (P[0] - P[2]) / dif;
        }

        /// <summary>
        /// Γ of up and out call option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="H">Barrier price</param>
        /// <param name="n">N of trinominal tree</param>
        /// <returns>Γ of up and out call option</returns>
        public static double CallUpOutGamma(double S, double X, double r, double sigma, double T, double H, int n) {
            //Gamma
            if (S == 0 || X == 0 || H == 0 || r == 0 || T == 0 || sigma == 0 || n == 0)
                return 0;
                       
            double dt = T / (double) n;

            int h = (int) Math.Floor(Math.Log(H / S) / (sigma * Math.Sqrt(dt)));

            if (h <= 0 || h > n)
                return -1;

            double lambda = Math.Log(H / S) / (h * sigma * Math.Sqrt(dt));

            if (h < 1) {
                n = (int) Math.Ceiling(sigma * sigma * T / Math.Pow(Math.Log(S / H), 2));
                dt = T / n;
                h = (int) Math.Floor(Math.Log(H / S) / (sigma * Math.Sqrt(dt)));
                lambda = Math.Log(H / S) / (h * sigma * Math.Sqrt(dt));
            }

            double R = Math.Exp(r * dt);
            double pu = 1.0 / (2.0 * lambda * lambda) + (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pd = 1.0 / (2.0 * lambda * lambda) - (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pm = 1.0 - pu - pd;
            double u = Math.Exp(lambda * sigma * Math.Sqrt(dt));

            double[] P = new double[2 * n + 1];

            for (int i = n - h; i <= 2 * n; i++) 
                P[i] = Math.Max(0, S * Math.Pow(u, n - i) - X);            

            for (int j = n - 1; j >= h; j--) {
                for (int i = j - h + 1; i <= 2 * j; i++) 
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, S * Math.Pow(u, j - i) - X);                
                P[j - h] = H - X;
            }

            for (int j = h - 1; j > 0; j--) 
                for (int i = 0; i <= 2 * j; i++) 
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, S * Math.Pow(u, j - i) - X);
                
            
            double dif1 = S * (Math.Pow(u, 1) - 1);
            double dif2 = S * (1 - Math.Pow(u, -1));
            double dif3 = S * (Math.Pow(u, 1) - Math.Pow(u, -1));

            return (((P[0] - P[1]) / dif1) - ((P[1] - P[2]) / dif2)) / (0.5 * dif3);
        }

        /// <summary>
        /// Θ of up and out call option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="H">Barrier price</param>
        /// <param name="n">N of trinominal tree</param>
        /// <returns>Θ of up and out call option</returns>
        public static double CallUpOutTheta(double S, double X, double r, double sigma, double T, double H, int n) {
            //Theta
            if (S == 0 || X == 0 || H == 0 || r == 0 || T == 0 || sigma == 0 || n == 0)
                return 0;
                       
            double dt = T / (double) n;
            int h = (int) Math.Floor(Math.Log(H / S) / (sigma * Math.Sqrt(dt)));
            if (h <= 0 || h > n)
                return -1;

            double lambda = Math.Log(H / S) / (h * sigma * Math.Sqrt(dt));

            if (h < 1) {
                n = (int) Math.Ceiling(sigma * sigma * T / Math.Pow(Math.Log(S / H), 2));
                dt = T / n;
                h = (int) Math.Floor(Math.Log(H / S) / (sigma * Math.Sqrt(dt)));
                lambda = Math.Log(H / S) / (h * sigma * Math.Sqrt(dt));
            }

            double R = Math.Exp(r * dt);
            double pu = 1.0 / (2.0 * lambda * lambda) + (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pd = 1.0 / (2.0 * lambda * lambda) - (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pm = 1.0 - pu - pd;
            double u = Math.Exp(lambda * sigma * Math.Sqrt(dt));

            double[] P = new double[2 * n + 1];

            for (int i = n - h; i <= 2 * n; i++) 
                P[i] = Math.Max(0, S * Math.Pow(u, n - i) - X);
            
            for (int j = n - 1; j >= h; j--) { 
                for (int i = j - h + 1; i <= 2 * j; i++) 
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, S * Math.Pow(u, j - i) - X);                
                P[j - h] = H - X;
            }

            for (int j = h - 1; j > 0; j--) 
                for (int i = 0; i <= 2 * j; i++) 
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, S * Math.Pow(u, j - i) - X);
                         
            double temp = P[1];
            double Price = Math.Max((pu * P[0] + pm * P[1] + pd * P[2]) / R, S - X);
            return (temp - Price) / dt;
        }

        /// <summary>
        /// ν of up and out call option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="H">Barrier price</param>
        /// <param name="n">N of trinominal tree</param>
        /// <returns>ν of up and out call option</returns>
        public static double CallUpOutVega(double S, double X, double r, double sigma, double T, double H, int n) {
            //double temp1 = CallUpOutPrice(S, X, r, sigma, T, H, n);
            //double temp2 = CallUpOutPrice(S, X, r, sigma + 0.0001, T, H, n);
            return (CallUpOutPrice(S, X, r, sigma + 0.0001, T, H, n) - CallUpOutPrice(S, X, r, sigma, T, H, n)) / 0.0001;
        }
        /// <summary>
        /// ρ of up and out call option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="H">Barrier price</param>
        /// <param name="n">N of trinominal tree</param>
        /// <returns>ρ of up and out call option</returns>
        public static double CallUpOutRho(double S, double X, double r, double sigma, double T, double H, int n) {
            //double temp1 = CallUpOutPrice(S, X, r, sigma, T, H, n);
            //double temp2 = CallUpOutPrice(S, X, r + 0.0001, sigma, T, H, n);
            return (CallUpOutPrice(S, X, r + 0.0001, sigma, T, H, n) - CallUpOutPrice(S, X, r, sigma, T, H, n)) / 0.0001;
        }

        /// <summary>
        /// Implied volatility of up and out call option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="H">Barrier price</param>
        /// <param name="optionPrice">Option price</param>
        /// <param name="n">N of trinominal tree</param>
        /// <returns>Implied vloatility of up and out call option</returns>
        public static double CallUpOutIV(double S, double X, double r, double T, double H, double optionPrice, int n) {
            double Vmin = 0.05, Vmax = 2, Vmid = (Vmin + Vmax) / 2;
            double Fmin = CallUpOutPrice(S, X, r, Vmin, T, H, n) - optionPrice;
            double Fmax = CallUpOutPrice(S, X, r, Vmax, T, H, n) - optionPrice;
            double Fmid = CallUpOutPrice(S, X, r, Vmid, T, H, n) - optionPrice;

            do {
                if (Fmin * Fmid < 0) {
                    Vmax = Vmid;
                    Vmid = (Vmin + Vmax) / 2;
                    Fmax = Fmid;
                    Fmid = CallUpOutPrice(S, X, r, Vmid, T, H, n) - optionPrice;
                } else {
                    if (Fmax * Fmid < 0) {
                        Vmin = Vmid;
                        Vmid = (Vmin + Vmax) / 2;
                        Fmin = Fmid;
                        Fmid = CallUpOutPrice(S, X, r, Vmid, T, H, n) - optionPrice;
                    } else
                        return Vmax;
                }
            }
            while (Vmax - Vmin >= 0.0001);

            Vmid = (Vmin + Vmax) / 2.0;
            if (Vmid > 9)
                return 0;
            return Vmid;
        }

        /// <summary>
        /// Price of down and out put option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="H">Barrier price</param>
        /// <param name="n">N of trinominal tree</param>
        /// <returns>Price of down and out put option</returns>
        public static double PutDownOutPrice(double S, double X, double r, double sigma, double T, double H, int n) {
            if (S == 0 || X == 0 || H == 0 || r == 0 || T == 0 || sigma == 0 || n == 0)
                return 0;
                       
            double dt = T / (double) n;

            int h = (int) Math.Floor(Math.Log(S / H) / (sigma * Math.Sqrt(dt)));

            if (h <= 0 || h > n)
                return -1;

            double lambda = Math.Log(S / H) / (h * sigma * Math.Sqrt(dt));

            if (h < 1) {
                n = (int) Math.Ceiling(sigma * sigma * T / Math.Pow(Math.Log(H / S), 2));
                dt = T / n;
                h = (int) Math.Floor(Math.Log(S / H) / (sigma * Math.Sqrt(dt)));
                lambda = Math.Log(S / H) / (h * sigma * Math.Sqrt(dt));
            }

            double R = Math.Exp(r * dt);
            double pu = 1.0 / (2.0 * lambda * lambda) + (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pd = 1.0 / (2.0 * lambda * lambda) - (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pm = 1.0 - pu - pd;
            double u = Math.Exp(lambda * sigma * Math.Sqrt(dt));

            double[] P = new double[2 * n + 1];

            for (int i = 0; i <= n + h; i++) 
                P[i] = Math.Max(0, X - S * Math.Pow(u, n - i));
            

            for (int j = n - 1; j >= h; j--) {
                for (int i = 0; i <= j + h - 1; i++) {
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, X - S * Math.Pow(u, j - i));
                }
                P[j + h] = X - H;
            }

            for (int j = h - 1; j >= 0; j--) 
                for (int i = 0; i <= 2 * j; i++) 
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, X - S * Math.Pow(u, j - i));
                         
            return P[0];
        }

        /// <summary>
        /// Δ of down and out put option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="H">Barrier price</param>
        /// <param name="n">N of trinominal tree</param>
        /// <returns>Δ of down and out put option</returns>
        public static double PutDownOutDelta(double S, double X, double r, double sigma, double T, double H, int n) {
            if (S == 0 || X == 0 || H == 0 || r == 0 || T == 0 || sigma == 0 || n == 0)
                return 0;
                       
            double dt = T / (double) n;

            int h = (int) Math.Floor(Math.Log(S / H) / (sigma * Math.Sqrt(dt)));

            if (h <= 0 || h > n)
                return -1;

            double lambda = Math.Log(S / H) / (h * sigma * Math.Sqrt(dt));

            if (h < 1) {
                n = (int) Math.Ceiling(sigma * sigma * T / Math.Pow(Math.Log(H / S), 2));
                dt = T / n;
                h = (int) Math.Floor(Math.Log(S / H) / (sigma * Math.Sqrt(dt)));
                lambda = Math.Log(S / H) / (h * sigma * Math.Sqrt(dt));
            }

            double R = Math.Exp(r * dt);
            double pu = 1.0 / (2.0 * lambda * lambda) + (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pd = 1.0 / (2.0 * lambda * lambda) - (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pm = 1.0 - pu - pd;
            double u = Math.Exp(lambda * sigma * Math.Sqrt(dt));

            double[] P = new double[2 * n + 1];

            for (int i = 0; i <= n + h; i++) 
                P[i] = Math.Max(0, X - S * Math.Pow(u, n - i));            

            for (int j = n - 1; j >= h; j--) {
                for (int i = 0; i <= j + h - 1; i++) 
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, X - S * Math.Pow(u, j - i));                
                P[j + h] = X - H;
            }

            for (int j = h - 1; j > 0; j--) 
                for (int i = 0; i <= 2 * j; i++) 
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, X - S * Math.Pow(u, j - i));
                            
            double dif = S * (Math.Pow(u, 1) - Math.Pow(u, -1));
            return (P[0] - P[2]) / dif;
        }
        /// <summary>
        /// Γ of down and out put option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="H">Barrier price</param>
        /// <param name="n">N of trinominal tree</param>
        /// <returns>Γ of down and out put option</returns>
        public static double PutDownOutGamma(double S, double X, double r, double sigma, double T, double H, int n) {
            if (S == 0 || X == 0 || H == 0 || r == 0 || T == 0 || sigma == 0 || n == 0)
                return 0;
                        
            double dt = T / (double) n;

            int h = (int) Math.Floor(Math.Log(S / H) / (sigma * Math.Sqrt(dt)));

            if (h <= 0 || h > n)
                return -1;

            double lambda = Math.Log(S / H) / (h * sigma * Math.Sqrt(dt));

            if (h < 1) {
                n = (int) Math.Ceiling(sigma * sigma * T / Math.Pow(Math.Log(H / S), 2));
                dt = T / n;
                h = (int) Math.Floor(Math.Log(S / H) / (sigma * Math.Sqrt(dt)));
                lambda = Math.Log(S / H) / (h * sigma * Math.Sqrt(dt));
            }

            double R = Math.Exp(r * dt);
            double pu = 1.0 / (2.0 * lambda * lambda) + (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pd = 1.0 / (2.0 * lambda * lambda) - (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pm = 1.0 - pu - pd;
            double u = Math.Exp(lambda * sigma * Math.Sqrt(dt));

            double[] P = new double[2 * n + 1];

            for (int i = 0; i <= n + h; i++) 
                P[i] = Math.Max(0, X - S * Math.Pow(u, n - i));
            
            for (int j = n - 1; j >= h; j--) { 
                for (int i = 0; i <= j + h - 1; i++) 
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, X - S * Math.Pow(u, j - i));                
                P[j + h] = X - H;
            }

            for (int j = h - 1; j > 0; j--) 
                for (int i = 0; i <= 2 * j; i++) 
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, X - S * Math.Pow(u, j - i));
                       
            double dif1 = S * (Math.Pow(u, 1) - 1);
            double dif2 = S * (1 - Math.Pow(u, -1));
            double dif3 = S * (Math.Pow(u, 1) - Math.Pow(u, -1));

            return (((P[0] - P[1]) / dif1) - ((P[1] - P[2]) / dif2)) / (0.5 * dif3);
        }

        /// <summary>
        /// Θ of down and out put option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="H">Barrier price</param>
        /// <param name="n">N of trinominal tree</param>
        /// <returns>Θ of down and out put option</returns>
        public static double PutDownOutTheta(double S, double X, double r, double sigma, double T, double H, int n) {
            if (S == 0 || X == 0 || H == 0 || r == 0 || T == 0 || sigma == 0 || n == 0)
                return 0;
                        
            double dt = T / (double) n;

            int h = (int) Math.Floor(Math.Log(S / H) / (sigma * Math.Sqrt(dt)));

            if (h <= 0 || h > n)
                return -1;

            double lambda = Math.Log(S / H) / (h * sigma * Math.Sqrt(dt));

            if (h < 1) {
                n = (int) Math.Ceiling(sigma * sigma * T / Math.Pow(Math.Log(H / S), 2));
                dt = T / n;
                h = (int) Math.Floor(Math.Log(S / H) / (sigma * Math.Sqrt(dt)));
                lambda = Math.Log(S / H) / (h * sigma * Math.Sqrt(dt));
            }

            double R = Math.Exp(r * dt);
            double pu = 1.0 / (2.0 * lambda * lambda) + (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pd = 1.0 / (2.0 * lambda * lambda) - (r - sigma * sigma / 2.0) * Math.Sqrt(dt) / (2.0 * lambda * sigma);
            double pm = 1.0 - pu - pd;
            double u = Math.Exp(lambda * sigma * Math.Sqrt(dt));

            double[] P = new double[2 * n + 1];

            for (int i = 0; i <= n + h; i++) 
                P[i] = Math.Max(0, X - S * Math.Pow(u, n - i));            

            for (int j = n - 1; j >= h; j--) {
                for (int i = 0; i <= j + h - 1; i++) 
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, X - S * Math.Pow(u, j - i));                
                P[j + h] = X - H;
            }

            for (int j = h - 1; j > 0; j--) 
                for (int i = 0; i <= 2 * j; i++) 
                    P[i] = Math.Max((pu * P[i] + pm * P[i + 1] + pd * P[i + 2]) / R, X - S * Math.Pow(u, j - i));
                          
            double temp = P[1];
            double Price = Math.Max((pu * P[0] + pm * P[1] + pd * P[2]) / R, X - S);
            return (temp - Price) / dt;
        }

        /// <summary>
        /// ν of down and out put option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="H">Barrier price</param>
        /// <param name="n">N of trinominal tree</param>
        /// <returns>ν of down and out put option</returns>
        public static double PutDownOutVega(double S, double X, double r, double sigma, double T, double H, int n) {
            //double temp1 = PutDownOutPrice(S, X, r, sigma, T, H, n);
            //double temp2 = PutDownOutPrice(S, X, r, sigma + 0.0001, T, H, n);
            return (PutDownOutPrice(S, X, r, sigma + 0.0001, T, H, n) - PutDownOutPrice(S, X, r, sigma, T, H, n)) / 0.0001;
        }

        /// <summary>
        /// ρ of down and out put option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="sigma">σ of spot price</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="H">Barrier price</param>
        /// <param name="n">N of trinominal tree</param>
        /// <returns>ρ of down and out put option</returns>
        public static double PutDownOutRho(double S, double X, double r, double sigma, double T, double H, int n) {
            //double temp1 = PutDownOutPrice(S, X, r, sigma, T, H, n);
            //double temp2 = PutDownOutPrice(S, X, r + 0.0001, sigma, T, H, n);
            return (PutDownOutPrice(S, X, r + 0.0001, sigma, T, H, n) - PutDownOutPrice(S, X, r, sigma, T, H, n)) / 0.0001;
        }
        /// <summary>
        /// Implied volatility of down and out put option
        /// </summary>
        /// <param name="S">Spot price</param>
        /// <param name="X">Exercise price</param>
        /// <param name="r">Interest rate</param>
        /// <param name="T">Time to maturity</param>
        /// <param name="H">Barrier price</param>
        /// <param name="optionPrice">Option price</param>
        /// <param name="n">N of trinominal tree</param>
        /// <returns>Implied vloatility of down and out put option</returns>
        public static double PutDownOutIV(double S, double X, double r, double T, double H, double optionPrice, int n) {
            double Vmin = 0.05, Vmax = 2, Vmid = (Vmin + Vmax) / 2;
            double Fmin = PutDownOutPrice(S, X, r, Vmin, T, H, n) - optionPrice;
            double Fmax = PutDownOutPrice(S, X, r, Vmax, T, H, n) - optionPrice;
            double Fmid = PutDownOutPrice(S, X, r, Vmid, T, H, n) - optionPrice;

            do {
                if (Fmin * Fmid < 0) {
                    Vmax = Vmid;
                    Vmid = (Vmin + Vmax) / 2;
                    Fmax = Fmid;
                    Fmid = PutDownOutPrice(S, X, r, Vmid, T, H, n) - optionPrice;
                } else {
                    if (Fmax * Fmid < 0) {
                        Vmin = Vmid;
                        Vmid = (Vmin + Vmax) / 2;
                        Fmin = Fmid;
                        Fmid = PutDownOutPrice(S, X, r, Vmid, T, H, n) - optionPrice;
                    } else
                        return Vmax;
                }
            }
            while (Vmax - Vmin >= 0.0001);

            Vmid = (Vmin + Vmax) / 2.0;
            if (Vmid > 9)
                return 0;
            return Vmid;
        }
    }
}
