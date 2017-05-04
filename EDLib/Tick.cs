using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDLib
{
    public class Tick
    {
        public enum CommodityType {Stock, Future, Warrant, ETF, Others};
        public static double UpTickSize(string ID, double price) {
            CommodityType type = GetCommodityType(ID);
            price = Math.Round(price, 2);
            switch (type) {

                case CommodityType.ETF:
                    if (price < 50)
                        return 0.01;
                    return 0.05;               

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

                case CommodityType.Future:
                    if (ID == "IX0001")
                        return 1;
                    if (ID == "IX0027")
                        return 0.05;
                    if (ID == "IX0039")
                        return 0.2;
                    return 0;                 
                case CommodityType.Others:
                default:
                    return 0;
            }            
        }

        public static double DownTickSize(string ID, double price) {
            return UpTickSize(ID, price - 0.01);
        }
              

        public static CommodityType GetCommodityType(string ID) {
            if (ID.StartsWith("IX"))
                return CommodityType.Future;
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
