using System;
namespace EDLib
{
    /// <summary>
    /// Slippage Cost
    /// </summary>
    public class SlippageCost
    {
        /// <summary>
        /// Calculate slippage costs
        /// </summary>
        /// <param name="ID">Commodity ID</param>
        /// <param name="pricingBA">Pricing Bid/Ask</param>
        /// <param name="hedgeLots">Number of lots to buy/sell (&lt;0 for sell)</param>
        /// <param name="prices">Best bid1~5 (ask1~5) to sell (buy) lots to (from). Array must in decreasing (increasing) order.</param>
        /// <param name="quantities">Quantities of the best bid1~5 (ask1~5)</param>
        /// <returns>Profit or loss due to slippage</returns>
        public static double Calculate(string ID, double pricingBA, int hedgeLots, double[] prices, int[] quantities) {
            if (prices.Length != quantities.Length)
                throw new ArgumentException("prices.Length must equals quantities.Length");
            if (hedgeLots < 0 && prices[0] < prices[1])
                throw new ArgumentException("Prices are not in decreasing order. (Prices should be Bids.)");
            if (hedgeLots > 0 && prices[0] > prices[1])
                throw new ArgumentException("Prices are not in increasing order. (Prices should be Asks.)");

            if (hedgeLots == 0)
                return 0;

            double price6;
            if (hedgeLots > 0) {
                price6 = pricingBA - Tick.ShiftTicks(ID, prices[prices.Length - 1], 1);
                for (int i = 0; i < prices.Length; i++)
                    prices[i] = pricingBA - prices[i];                
            } else {
                price6 = Tick.ShiftTicks(ID, prices[prices.Length - 1], -1) - pricingBA;
                for (int i = 0; i < prices.Length; i++)
                    prices[i] -= pricingBA;                
                hedgeLots = -hedgeLots;
            }

            double PnL = 0;
            for (int i = 0; i < prices.Length; i++) {
                if (hedgeLots <= quantities[i]) {
                    PnL += hedgeLots * prices[i];
                    hedgeLots = 0;
                    break;
                } else {
                    PnL += quantities[i] * prices[i];
                    hedgeLots -= quantities[i];
                }
            }
            if (hedgeLots > 0)
                PnL += hedgeLots * price6;
                        
            return PnL;
        }
    }
}
