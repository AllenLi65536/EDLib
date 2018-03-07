using System;
using EDLib.Pricing.Option;

namespace EDLib.Pricing.Warrant
{
    /// <summary>
    /// Use for warrants pricing
    /// </summary>
    internal class NamespaceDoc { }
    /// <summary>
    /// Type of warrants
    /// </summary>
    public enum WarrantType
    {
        /// <summary>
        /// Call
        /// </summary>
        Call,
        /// <summary>
        /// Put
        /// </summary>
        Put,
        /// <summary>
        /// Up and knock out call
        /// </summary>
        CallUpOut,
        /// <summary>
        /// Down and knock out put
        /// </summary>
        PutDownOut,
        /// <summary>
        /// Down and knock out call (aka Bull)
        /// </summary>
        CallDownOut,
        /// <summary>
        /// Up and knock out put (aka Bear)
        /// </summary>
        PutUpOut,
        /// <summary>
        /// Other unlisted warrants (Should not have any)
        /// </summary>
        Others
    }

    /*switch (wClass) {
                case "c":
                    this.wClass = WarrantType.Call;
                    break;
                case "p":
                    this.wClass = WarrantType.Put;
                    break;
                case "cuo":
                    this.wClass = WarrantType.CallUpOut;
                    break;
                case "pdo":
                    this.wClass = WarrantType.PutDownOut;
                    break;
                case "cdo":
                case "xcdo":
                    this.wClass = WarrantType.CallDownOut;
                    break;
                case "puo":
                case "xpuo":
                    this.wClass = WarrantType.PutUpOut;
                    break;
                default:
                    this.wClass = WarrantType.Others;
                    break;
            }*/
    /// <summary>
    /// Used for pricing warrants
    /// </summary>
    public abstract class Warrant
    {
        /// <summary>
        /// Warrant ID
        /// </summary>
        public readonly string WID;
        /// <summary>
        /// Issuer ID of the warrant
        /// </summary>
        public readonly string issuer;
        /// <summary>
        /// Trader ID
        /// </summary>
        public readonly string traderID;
        /// <summary>
        /// Underlying ID
        /// </summary>
        public readonly string UID;
        /// <summary>
        /// Exercise price
        /// </summary>
        public readonly double X;
        /// <summary>
        /// Time to maturity in year
        /// </summary>
        public readonly double T;
        /// <summary>
        /// Conversion ratio
        /// </summary>
        public readonly double CR;
        /// <summary>
        /// Interest rate
        /// </summary>
        public readonly double r = 0.025;

        /// <summary>
        /// Underlying spot price
        /// </summary>
        public double S = 0;
        /// <summary>
        /// Sigma or Financial rate (Bull/Bear)
        /// </summary>
        public double sigma_finRate = 0.1;



        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="WID">Warrant ID</param>
        /// <param name="issuer">Issuer ID of the warrant</param>
        /// <param name="traderID">Trader ID</param>
        /// <param name="UID">Underlying ID</param>
        /// <param name="X">Exercise price</param>
        /// <param name="T">Time to maturity in year</param>
        /// <param name="CR">Conversion ratio</param>
        public Warrant(string WID, string issuer, string traderID, string UID, double X, double T, double CR) {
            this.WID = WID;
            this.issuer = issuer;
            this.traderID = traderID;
            this.UID = UID;
            this.X = X;
            this.T = T;
            this.CR = CR;
        }
        /// <summary>
        /// Warrant price
        /// </summary>
        /// <returns>Warrant price</returns>
        public abstract double Price();
        //public abstract double Price(double S);
        /// <summary>
        /// Warrant delta
        /// </summary>
        /// <returns>Warrant delta</returns>
        public abstract double Delta();
        //public abstract double Delta(double S);
        /// <summary>
        /// Warrant gamma
        /// </summary>
        /// <returns>Warrant gamma</returns>
        public abstract double Gamma();
        //public abstract double Gamma(double S);
        /// <summary>
        /// Warrant theta
        /// </summary>
        /// <returns>Warrant theta</returns>
        public abstract double Theta();
        //public abstract double Theta(double S);
        /// <summary>
        /// Warrant vega
        /// </summary>
        /// <returns>Warrant vega</returns>
        public abstract double Vega();
        //public abstract double Vega(double S);
        /// <summary>
        /// Warrant rho
        /// </summary>
        /// <returns>Warrant rho</returns>
        public abstract double Rho();
        //public abstract double Rho(double S);

    }
    /// <summary>
    /// Barrier warrants
    /// </summary>
    public abstract class BarrierWarrant:Warrant
    {
        /// <summary>
        /// Barrier
        /// </summary>
        public readonly double H;
        /// <summary>
        /// Constructor of barrier warrant
        /// </summary>
        /// <param name="WID">Warrant ID</param>
        /// <param name="issuer">Issuer ID of the warrant</param>
        /// <param name="traderID">Trader ID</param>
        /// <param name="UID">Underlying ID</param>
        /// <param name="X">Exercise price</param>
        /// <param name="T">Time to maturity in year</param>
        /// <param name="CR">Conversion ratio</param>
        /// <param name="H">Barrier</param>
        public BarrierWarrant(string WID, string issuer, string traderID, string UID, double X, double T, double CR, double H) : base(WID, issuer, traderID, UID, X, T, CR) {
            this.H = H;
        }
    }

    /// <summary>
    /// Call
    /// </summary>
    public class Call:Warrant
    {
        /// <summary>
        /// Constructor of call
        /// </summary>
        /// <param name="WID">Warrant ID</param>
        /// <param name="issuer">Issuer ID of the warrant</param>
        /// <param name="traderID">Trader ID</param>
        /// <param name="UID">Underlying ID</param>
        /// <param name="X">Exercise price</param>
        /// <param name="T">Time to maturity in year</param>
        /// <param name="CR">Conversion ratio</param>
        public Call(string WID, string issuer, string traderID, string UID, double X, double T, double CR) : base(WID, issuer, traderID, UID, X, T, CR) {
        }
        /// <summary>
        /// Warrant price
        /// </summary>
        /// <returns>Warrant price</returns>
        public override double Price() {
            return PlainVanilla.CallPrice(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Update stock price and calculate warrant price
        /// </summary>
        /// <param name="S">Stock price</param>
        /// <returns>Warrant price</returns>
        public double Price(double S) {
            this.S = S;
            return PlainVanilla.CallPrice(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Warrant delta
        /// </summary>
        /// <returns>Warrant delta</returns>
        public override double Delta() {
            return PlainVanilla.CallDelta(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Update stock price and calculate warrant delta
        /// </summary>
        /// <param name="S">Stock price</param>
        /// <returns>Warrant delta</returns>
        public double Delta(double S) {
            this.S = S;
            return PlainVanilla.CallDelta(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Warrant gamma
        /// </summary>
        /// <returns>Warrant gamma</returns>
        public override double Gamma() {
            return PlainVanilla.CallGamma(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Update stock price and calculate warrant gamma
        /// </summary>
        /// <param name="S">Stock price</param>
        /// <returns>Warrant gamma</returns>
        public double Gamma(double S) {
            this.S = S;
            return PlainVanilla.CallGamma(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Warrant theta
        /// </summary>
        /// <returns>Warrant theta</returns>
        public override double Theta() {
            return PlainVanilla.CallTheta(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Update stock price and calculate warrant theta
        /// </summary>
        /// <param name="S">Stock price</param>
        /// <returns>Warrant theta</returns>
        public double Theta(double S) {
            this.S = S;
            return PlainVanilla.CallTheta(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Warrant vega
        /// </summary>
        /// <returns>Warrant vega</returns>
        public override double Vega() {
            return PlainVanilla.CallVega(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Update stock price and calculate warrant vega
        /// </summary>
        /// <param name="S">Stock price</param>
        /// <returns>Warrant vega</returns>
        public double Vega(double S) {
            this.S = S;
            return PlainVanilla.CallVega(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Warrant rho
        /// </summary>
        /// <returns>Warrant rho</returns>
        public override double Rho() {
            return PlainVanilla.CallRho(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Update stock price and calculate warrant rho
        /// </summary>
        /// <param name="S">Stock price</param>
        /// <returns>Warrant rho</returns>
        public double Rho(double S) {
            this.S = S;
            return PlainVanilla.CallRho(S, X, r, sigma_finRate, T) * CR;
        }
    }
    /// <summary>
    /// Put
    /// </summary>
    public class Put:Warrant
    {
        /// <summary>
        /// Constructor of Put
        /// </summary>
        /// <param name="WID">Warrant ID</param>
        /// <param name="issuer">Issuer ID of the warrant</param>
        /// <param name="traderID">Trader ID</param>
        /// <param name="UID">Underlying ID</param>
        /// <param name="X">Exercise price</param>
        /// <param name="T">Time to maturity in year</param>
        /// <param name="CR">Conversion ratio</param>
        public Put(string WID, string issuer, string traderID, string UID, double X, double T, double CR) : base(WID, issuer, traderID, UID, X, T, CR) {
        }
        /// <summary>
        /// Warrant price
        /// </summary>
        /// <returns>Warrant price</returns>
        public override double Price() {
            return PlainVanilla.PutPrice(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Update stock price and calculate warrant price
        /// </summary>
        /// <param name="S">Stock price</param>
        /// <returns>Warrant price</returns>
        public double Price(double S) {
            this.S = S;
            return PlainVanilla.PutPrice(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Warrant delta
        /// </summary>
        /// <returns>Warrant delta</returns>
        public override double Delta() {
            return PlainVanilla.PutDelta(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Update stock price and calculate warrant delta
        /// </summary>
        /// <param name="S">Stock price</param>
        /// <returns>Warrant delta</returns>
        public double Delta(double S) {
            this.S = S;
            return PlainVanilla.PutDelta(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Warrant gamma
        /// </summary>
        /// <returns>Warrant gamma</returns>
        public override double Gamma() {
            return PlainVanilla.PutGamma(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Update stock price and calculate warrant gamma
        /// </summary>
        /// <param name="S">Stock price</param>
        /// <returns>Warrant gamma</returns>
        public double Gamma(double S) {
            this.S = S;
            return PlainVanilla.PutGamma(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Warrant theta
        /// </summary>
        /// <returns>Warrant theta</returns>
        public override double Theta() {
            return PlainVanilla.PutTheta(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Update stock price and calculate warrant theta
        /// </summary>
        /// <param name="S">Stock price</param>
        /// <returns>Warrant theta</returns>
        public double Theta(double S) {
            this.S = S;
            return PlainVanilla.PutTheta(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Warrant vega
        /// </summary>
        /// <returns>Warrant vega</returns>
        public override double Vega() {
            return PlainVanilla.PutVega(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Update stock price and calculate warrant vega
        /// </summary>
        /// <param name="S">Stock price</param>
        /// <returns>Warrant vega</returns>
        public double Vega(double S) {
            this.S = S;
            return PlainVanilla.PutVega(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Warrant rho
        /// </summary>
        /// <returns>Warrant rho</returns>
        public override double Rho() {
            return PlainVanilla.PutRho(S, X, r, sigma_finRate, T) * CR;
        }
        /// <summary>
        /// Update stock price and calculate warrant rho
        /// </summary>
        /// <param name="S">Stock price</param>
        /// <returns>Warrant rho</returns>
        public double Rho(double S) {
            this.S = S;
            return PlainVanilla.PutRho(S, X, r, sigma_finRate, T) * CR;
        }
    }

    /// <summary>
    /// Down and knock out call (aka Bull)
    /// </summary>
    public class CallDownOut:BarrierWarrant
    {
        /// <summary>
        /// Constructor of down and knock out call
        /// </summary>
        /// <param name="WID">Warrant ID</param>
        /// <param name="issuer">Issuer ID of the warrant</param>
        /// <param name="traderID">Trader ID</param>
        /// <param name="UID">Underlying ID</param>
        /// <param name="X">Exercise price</param>
        /// <param name="T">Time to maturity in year</param>
        /// <param name="CR">Conversion ratio</param>
        /// <param name="H">Barrier</param>
        public CallDownOut(string WID, string issuer, string traderID, string UID, double X, double T, double CR, double H) : base(WID, issuer, traderID, UID, X, T, CR, H) {
        }

        /// <summary>
        /// Price of down and knock out call
        /// </summary>
        /// <returns>Price of down and knock out call</returns>
        public override double Price() {
            return ((S - X) + X * T * sigma_finRate) * CR;
        }
        /// <summary>
        /// Delta of down and knock out call
        /// </summary>
        /// <returns>==1</returns>
        public override double Delta() {
            return CR;//Problem found
        }
        /// <summary>
        /// Gamma of down and knock out call
        /// </summary>
        /// <returns>==0</returns>
        public override double Gamma() {
            return 0;
        }
        /// <summary>
        /// Theta of down and knock out call
        /// </summary>
        /// <returns>Theta of down and knock out call</returns>
        public override double Theta() {
            return -X * sigma_finRate * CR / 365.0;
        }
        /// <summary>
        /// Vega of down and knock out call
        /// </summary>
        /// <returns>==0</returns>
        public override double Vega() {
            return 0;
        }
        /// <summary>
        /// Rho of down and knock out call
        /// </summary>
        /// <returns>NotImplementedException</returns>
        public override double Rho() {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Up and knock out put (aka Bear)
    /// </summary>
    public class PutUpOut:BarrierWarrant
    {
        /// <summary>
        /// Constructor of down and knock out call
        /// </summary>
        /// <param name="WID">Warrant ID</param>
        /// <param name="issuer">Issuer ID of the warrant</param>
        /// <param name="traderID">Trader ID</param>
        /// <param name="UID">Underlying ID</param>
        /// <param name="X">Exercise price</param>
        /// <param name="T">Time to maturity in year</param>
        /// <param name="CR">Conversion ratio</param>
        /// <param name="H">Barrier</param>
        public PutUpOut(string WID, string issuer, string traderID, string UID, double X, double T, double CR, double H) : base(WID, issuer, traderID, UID, X, T, CR, H) {
        }
        /// <summary>
        /// Price of up and knock out put
        /// </summary>
        /// <returns>Price of up and knock out put</returns>
        public override double Price() {
            return ((X - S) + X * T * sigma_finRate) * CR;
        }
        /// <summary>
        /// Delta of up and knock out put
        /// </summary>
        /// <returns>== -1</returns>
        public override double Delta() {
            return -CR;
        }
        /// <summary>
        /// Gamma of up and knock out put
        /// </summary>
        /// <returns>== 0</returns>
        public override double Gamma() {
            return 0;
        }
        /// <summary>
        /// Theta of up and knock out put
        /// </summary>
        /// <returns>Theta of up and knock out put</returns>
        public override double Theta() {
            return -X * sigma_finRate * CR / 365.0;
        }
        /// <summary>
        /// Vega of up and knock out put
        /// </summary>
        /// <returns>== 0</returns>
        public override double Vega() {
            return 0;
        }
        /// <summary>
        /// Rho of up and knock out put
        /// </summary>
        /// <returns>NotImplementedException</returns>
        public override double Rho() {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Up and knock out call
    /// </summary>
    public class CallUpOut:BarrierWarrant
    {
        /// <summary>
        /// Constructor of up and knock out call
        /// </summary>
        /// <param name="WID">Warrant ID</param>
        /// <param name="issuer">Issuer ID of the warrant</param>
        /// <param name="traderID">Trader ID</param>
        /// <param name="UID">Underlying ID</param>
        /// <param name="X">Exercise price</param>
        /// <param name="T">Time to maturity in year</param>
        /// <param name="CR">Conversion ratio</param>
        /// <param name="H">Barrier</param>
        public CallUpOut(string WID, string issuer, string traderID, string UID, double X, double T, double CR, double H) : base(WID, issuer, traderID, UID, X, T, CR, H) {
        }
        /// <summary>
        /// Price of up and knock out call
        /// </summary>
        /// <returns>Price of up and knock out call</returns>
        public override double Price() {
            return BarrierOption.CallUpOutPrice(S, X, r, sigma_finRate, T, H) * CR;
        }
        /// <summary>
        /// Delta of up and knock out call
        /// </summary>
        /// <returns>Delta of up and knock out call</returns>
        public override double Delta() {
            return BarrierOption.CallUpOutDelta(S, X, r, sigma_finRate, T, H) * CR;
        }
        /// <summary>
        /// Gamma of up and knock out call
        /// </summary>
        /// <returns>Gamma of up and knock out call</returns>
        public override double Gamma() {
            return BarrierOption.CallUpOutGamma(S, X, r, sigma_finRate, T, H) * CR;
        }
        /// <summary>
        /// Theta of up and knock out call
        /// </summary>
        /// <returns>Theta of up and knock out call</returns>
        public override double Theta() {
            return BarrierOption.CallUpOutTheta(S, X, r, sigma_finRate, T, H) * CR;
        }
        /// <summary>
        /// Vega of up and knock out call
        /// </summary>
        /// <returns>Vega of up and knock out call</returns>
        public override double Vega() {
            return BarrierOption.CallUpOutVega(S, X, r, sigma_finRate, T, H) * CR;
        }
        /// <summary>
        /// Rho of up and knock out call
        /// </summary>
        /// <returns>Rho of up and knock out call</returns>
        public override double Rho() {
            return BarrierOption.CallUpOutRho(S, X, r, sigma_finRate, T, H) * CR;
        }
    }
    /// <summary>
    /// Down and knock out put
    /// </summary>
    public class PutDownOut:BarrierWarrant
    {
        /// <summary>
        /// Constructor of down and knock out put
        /// </summary>
        /// <param name="WID">Warrant ID</param>
        /// <param name="issuer">Issuer ID of the warrant</param>
        /// <param name="traderID">Trader ID</param>
        /// <param name="UID">Underlying ID</param>
        /// <param name="X">Exercise price</param>
        /// <param name="T">Time to maturity in year</param>
        /// <param name="CR">Conversion ratio</param>
        /// <param name="H">Barrier</param>
        public PutDownOut(string WID, string issuer, string traderID, string UID, double X, double T, double CR, double H) : base(WID, issuer, traderID, UID, X, T, CR, H) {
        }
        /// <summary>
        /// Price of down and knock out put
        /// </summary>
        /// <returns>Price of down and knock out put</returns>
        public override double Price() {
            return BarrierOption.PutDownOutPrice(S, X, r, sigma_finRate, T, H) * CR;
        }
        /// <summary>
        /// Delta of down and knock out put
        /// </summary>
        /// <returns>Delta of down and knock out put</returns>
        public override double Delta() {
            return BarrierOption.PutDownOutDelta(S, X, r, sigma_finRate, T, H) * CR;
        }
        /// <summary>
        /// Gamma of down and knock out put
        /// </summary>
        /// <returns>Gamma of down and knock out put</returns>
        public override double Gamma() {
            return BarrierOption.PutDownOutGamma(S, X, r, sigma_finRate, T, H) * CR;
        }
        /// <summary>
        /// Theta of down and knock out put
        /// </summary>
        /// <returns>Theta of down and knock out put</returns>
        public override double Theta() {
            return BarrierOption.PutDownOutTheta(S, X, r, sigma_finRate, T, H) * CR;
        }
        /// <summary>
        /// Vega of down and knock out put
        /// </summary>
        /// <returns>Vega of down and knock out put</returns>
        public override double Vega() {
            return BarrierOption.PutDownOutVega(S, X, r, sigma_finRate, T, H) * CR;
        }
        /// <summary>
        /// Rho of down and knock out put
        /// </summary>
        /// <returns>Rho of down and knock out put</returns>
        public override double Rho() {
            return BarrierOption.PutDownOutRho(S, X, r, sigma_finRate, T, H) * CR;
        }
    }
}
