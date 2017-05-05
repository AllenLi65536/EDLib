using System;

namespace EDLib
{
    public class Tick
    {
        public enum CommodityType { Stock, IndexFuture, Warrant, ETF, Others };

        public static double UpTickSize(string ID, double price) {
            CommodityType type = GetCommodityType(ID);
            price = Math.Round(price, 2);
            switch (type) {

                case CommodityType.Warrant:
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

                case CommodityType.IndexFuture:
                    if (ID == "IX0001")
                        return 1;
                    if (ID == "IX0027")
                        return 0.05;
                    if (ID == "IX0039")
                        return 0.2;
                    return 0;

                case CommodityType.Stock:
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

                case CommodityType.ETF:
                    if (price < 50)
                        return 0.01;
                    return 0.05;

                case CommodityType.Others:
                default:
                    return 0;
            }
        }

        public static double DownTickSize(string ID, double price) {
            return -UpTickSize(ID, price - 0.01);
        }

        public static double[] GetBids(string ID, double bid1) {
            double[] bids = new double[6];
            bids[0] = Math.Round(bid1, 2);
            for (int i = 1; i < 6; i++)
                bids[i] = Math.Round(bids[i - 1] + DownTickSize(ID, bids[i - 1]), 2);

            return bids;
        }

        public static double[] GetAsks(string ID, double ask1) {
            double[] asks = new double[6];
            asks[0] = Math.Round(ask1, 2);
            for (int i = 1; i < 6; i++)
                asks[i] = Math.Round(asks[i - 1] + UpTickSize(ID, asks[i - 1]), 2);

            return asks;
        }

        public static double ShiftTicks(string ID, double price, int N) {
            price = Math.Round(price, 2);

            if (N > 0) {
                for (int i = 0; i < N; i++)
                    price = Math.Round(price + UpTickSize(ID, price), 2);
            } else if (N < 0) {
                for (int i = 0; i < N; i++)
                    price = Math.Round(price + DownTickSize(ID, price), 2);
            }
            return price;
        }

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

        public static CommodityType GetCommodityType(string ID) {
            if (ID.StartsWith("IX"))
                return CommodityType.IndexFuture;
            if (ID.StartsWith("00"))
                return CommodityType.ETF;
            if (ID.Length == 4)
                return CommodityType.Stock;
            if (ID.Length == 6)
                return CommodityType.Warrant;
            return CommodityType.Others;
        }

    }
}
