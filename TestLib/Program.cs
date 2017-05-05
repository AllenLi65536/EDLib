using System;
using EDLib;
namespace TestLib
{
    class Program
    {
        static void Main(string[] args) {
            /* Console.WriteLine(Tick.UpTickSize("2330", 1));
             Console.WriteLine(Tick.UpTickSize("2330", 5));
             Console.WriteLine(Tick.UpTickSize("2330", 10));
             Console.WriteLine(Tick.UpTickSize("2330", 20));
             Console.WriteLine(Tick.UpTickSize("2330", 50));
             Console.WriteLine(Tick.UpTickSize("2330", 80));
             Console.WriteLine(Tick.UpTickSize("2330", 100));
             Console.WriteLine(Tick.UpTickSize("2330", 200));
             Console.WriteLine(Tick.UpTickSize("2330", 500));
             Console.WriteLine(Tick.UpTickSize("2330", 800));
             Console.WriteLine(Tick.UpTickSize("2330", 1000));
             Console.WriteLine(Tick.UpTickSize("2330", 1100));
             double a = 5.45;
             double b = 10.45;
             double c = 0.0450000000100000001;
             Console.WriteLine(a);
             Console.WriteLine(b);
             Console.WriteLine(c);
             Console.WriteLine();
             Console.WriteLine(Tick.DownTickSize("2330", 1));
             Console.WriteLine(Tick.DownTickSize("2330", 5));
             Console.WriteLine(Tick.DownTickSize("2330", 10));
             Console.WriteLine(Tick.DownTickSize("2330", 20));
             Console.WriteLine(Tick.DownTickSize("2330", 50));
             Console.WriteLine(Tick.DownTickSize("2330", 80));
             Console.WriteLine(Tick.DownTickSize("2330", 100));
             Console.WriteLine(Tick.DownTickSize("2330", 200));
             Console.WriteLine(Tick.DownTickSize("2330", 500));
             Console.WriteLine(Tick.DownTickSize("2330", 800));
             Console.WriteLine(Tick.DownTickSize("2330", 1000));
             Console.WriteLine(Tick.DownTickSize("2330", 1100));*/

            /*double price = 9;
            for (int i = 0; i < 30; i++) {
                price += Tick.DownTickSize("2330", price);
                // price = Math.Round(price, 2);
                Console.Write(price + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < 30; i++) {
                price += Tick.UpTickSize("2330", price);
                //price = Math.Round(price, 2);
                Console.Write(price + " ");
            }
           
            

            Console.WriteLine();
            price = 49;
            for (int i = 0; i < 30; i++) {
                price += Tick.UpTickSize("2330", price);
                //price = Math.Round(price, 2);
                Console.Write(price + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < 30; i++) {
                price += Tick.DownTickSize("2330", price);
                //price = Math.Round(price, 2);
                Console.Write(price + " ");
            }

            Console.WriteLine();
            price = 98;
            for (int i = 0; i < 30; i++) {
                price += Tick.UpTickSize("2330", price);
                //price = Math.Round(price, 2);
                Console.Write(price + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < 30; i++) {
                price += Tick.DownTickSize("2330", price);
                //price = Math.Round(price, 2);
                Console.Write(price + " ");
            }

            Console.WriteLine();
            price = 498;
            for (int i = 0; i < 10; i++) {
                price += Tick.UpTickSize("2330", price);
                //price = Math.Round(price, 2);
                Console.Write(price + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < 10; i++) {
                price += Tick.DownTickSize("2330", price);
                //price = Math.Round(price, 2);
                Console.Write(price + " ");
            }*/
            double[] bids = Tick.GetAsks("2330", 9.19);
            for (int i = 0; i < 6; i++)
                Console.Write(bids[i] +" ");
            Console.WriteLine(Tick.GetTickNum("2330", 8.8, 8.76));
            Console.WriteLine(Tick.ShiftTicks("2330", 99, 25));

            //Console.WriteLine(TradeDate.LastNTradeDate(1));
            Console.ReadKey();
        }
    }
}
