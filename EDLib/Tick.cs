using System;

namespace EDLib
{
    //TODO: Maybe make it un-static
    /// <summary>
    /// Price tick related function of Taiwan stock/future market
    /// </summary>
    /// <example>
    /// <code>
    /// double price = 498;
    /// for (int i = 0; i &lt; 10; i++) {
    ///     price += Tick.UpTickSize("2330", price);     
    ///     Console.Write(price + " ");
    /// }
    /// Console.WriteLine();
    /// for (int i = 0; i &lt; 10; i++) {
    ///     price += Tick.DownTickSize("2330", price);
    ///     Console.Write(price + " ");
    /// }
    /// double[] bids = Tick.GetAsks("2330", 9.19);
    /// for (int i = 0; i &lt; 6; i++)
    ///     Console.Write(bids[i] + " ");
    /// Console.WriteLine(Tick.GetTickNum("2330", 8.8, 8.76));
    /// Console.WriteLine(Tick.GetTickNum("2330", 9.24, 9.23999999));
    /// Console.WriteLine(Tick.ShiftTicks("2330", 99, 25));
    /// </code>
    /// </example>
    public static class Tick
    {
       
        /// <summary>
        /// Tick size of one tick upward.
        /// </summary>
        /// <param name="ID">Commodity ID</param>
        /// <param name="price">Current price</param>
        /// <returns>Tick size of uptick</returns>
        public static double UpTickSize(string ID, double price) {
            Utility.CommodityType type = Utility.GetCommodityType(ID);
            price = Math.Round(price, 2);
            switch (type) {

                case Utility.CommodityType.Warrant:
                    if (price < 5)
                        return 0.01;
                    if (price < 10)
                        return 0.05;
                    if (price < 50)
                        return 0.1;
                    if (price < 100)
                        return 0.5;
                    if (price < 500)
                        return 1;
                    return 5;

                case Utility.CommodityType.IndexFuture:
                    if (ID == "IX0001")
                        return 1;
                    if (ID == "IX0027")
                        return 0.05;
                    if (ID == "IX0039")
                        return 0.2;
                    return 0;

                case Utility.CommodityType.Stock:
                    if (price < 10)
                        return 0.01;
                    if (price < 50)
                        return 0.05;
                    if (price < 100)
                        return 0.1;
                    if (price < 500)
                        return 0.5;
                    if (price < 1000)
                        return 1;
                    return 5;

                case Utility.CommodityType.ETF:
                    if (price < 50)
                        return 0.01;
                    return 0.05;

                case Utility.CommodityType.Others:
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Tick size of one tick downward.
        /// </summary>
        /// <param name="ID">Commodity ID</param>
        /// <param name="price">Current price</param>
        /// <returns>Tick size of downtick</returns>
        public static double DownTickSize(string ID, double price) {
            return -UpTickSize(ID, price - 0.01);
        }

        /// <summary>
        /// Get best 5 + 1 bid prices given bid1
        /// </summary>
        /// <param name="ID">Commodity ID</param>
        /// <param name="bid1">Bid1</param>
        /// <returns>An array with 6 bid prices</returns>
        public static double[] GetBids(string ID, double bid1) {
            double[] bids = new double[6];
            bids[0] = Math.Round(bid1, 2);
            for (int i = 1; i < 6; i++)
                bids[i] = Math.Round(bids[i - 1] + DownTickSize(ID, bids[i - 1]), 2);

            return bids;
        }

        /// <summary>
        /// Get best 5 + 1 ask prices given ask1
        /// </summary>
        /// <param name="ID">Commodity ID</param>
        /// <param name="ask1">Ask1</param>
        /// <returns>An array with 6 ask prices</returns>
        public static double[] GetAsks(string ID, double ask1) {
            double[] asks = new double[6];
            asks[0] = Math.Round(ask1, 2);
            for (int i = 1; i < 6; i++)
                asks[i] = Math.Round(asks[i - 1] + UpTickSize(ID, asks[i - 1]), 2);

            return asks;
        }

        /// <summary>
        /// Shift price N ticks up. (N &lt; 0 for down tick) 
        /// </summary>
        /// <param name="ID">Commodity ID</param>
        /// <param name="price">Current price</param>
        /// <param name="N">N ticks</param>
        /// <returns>Shifted price</returns>
        public static double ShiftTicks(string ID, double price, int N) {
            price = Math.Round(price, 2);

            if (N > 0) {
                for (int i = 0; i < N; i++)
                    price = Math.Round(price + UpTickSize(ID, price), 2);
            } else if (N < 0) {
                for (int i = 0; i < -N; i++)
                    price = Math.Round(price + DownTickSize(ID, price), 2);
            }
            return price;
        }

        /// <summary>
        /// Get number of ticks between price1 and price2
        /// </summary>
        /// <param name="ID">Commodity ID</param>
        /// <param name="price1">price1</param>
        /// <param name="price2">price2</param>
        /// <returns>Number of ticks</returns>
        public static int GetTickNum(string ID, double price1, double price2) {
            int N = 0;
            price1 = Math.Round(price1, 2);
            price2 = Math.Round(price2, 2);
            while (price1 > price2) {
                price1 = Math.Round(price1 + DownTickSize(ID, price1), 2);
                N++;
            }
            while (price1 < price2) {
                price1 = Math.Round(price1 + UpTickSize(ID, price1), 2);
                N++;
            }
            return N;
        }
    }
}
