using System;
using EDLib;
using EDLib.SQL;
using System.Data;

namespace TestLib
{
    class Program
    {
        static void Main(string[] args) {

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
            /*double[] bids = Tick.GetAsks("2330", 9.19);
            for (int i = 0; i < 6; i++)
                Console.Write(bids[i] +" ");
            Console.WriteLine(Tick.GetTickNum("2330", 8.8, 8.76));
            Console.WriteLine(Tick.GetTickNum("2330", 9.24, 9.23999999));
            Console.WriteLine(Tick.ShiftTicks("2330", 99, 25));

            string LastTDate = "20170504";       
            string SQLStr = "SELECT A.股票代號 ,E.總公司代號 as Name1 , Sum(A.買張)-isnull(C.買張,0) as buy , Sum(A.賣張)-isnull(C.賣張,0) as sell, Sum(A.張增減)-isnull(C.張增減,0) as netbs "
           + " ,Sum(A.[買金額(千)])-isnull(C.[買金額(千)],0) as buyA, Sum(A.[賣金額(千)])-isnull(C.[賣金額(千)],0) as sellA, Sum(A.[金額增減(千)])-isnull(C.[金額增減(千)],0) as netbsA "
           + " , B.權證成交量,B.[權證成交金額(千)], D.發行機構代號, left(D.發行機構名稱,2) as Name2, D.名稱 "
           + " from 個股券商分點進出明細 as A "
           + " left join 權證評估表 as B on A.股票代號=B.代號 and A.日期=B.日期 "
           + " left join 權證基本資料表 as D on D.代號=A.股票代號 "
           + " left join 券商公司基本資料表 as E on A.券商代號=E.代號 "
           + " left join 個股券商進出明細 as C on C.股票代號=A.股票代號 and E.總公司代號=C.券商代號 and A.日期=C.日期"
           + " where A.日期='" + LastTDate + "' and B.權證成交量 is not null and D.年度='" + LastTDate.Substring(0, 4) + "' and E.年度='" + LastTDate.Substring(0, 4) + "'"
           + " group by A.股票代號, E.總公司代號 ,B.權證成交量,B.[權證成交金額(千)], D.發行機構代號, left(D.發行機構名稱,2), D.名稱 , C.買張 , C.賣張, C.張增減, C.[買金額(千)], C.[賣金額(千)], C.[金額增減(千)] "
           + " having Sum(A.買張)-isnull(C.買張,0)<>0 or Sum(A.賣張)-isnull(C.賣張,0)<>0 or Sum(A.張增減)-isnull(C.張增減,0)<>0 or Sum(A.[買金額(千)])-isnull(C.[買金額(千)],0)<>0 or Sum(A.[賣金額(千)])-isnull(C.[賣金額(千)],0)<>0 or Sum(A.[金額增減(千)])-isnull(C.[金額增減(千)],0) <>0"
           + " order by A.股票代號 ";
            DataTable WarrantMM = SQL.ExecCMoneyQry(SQLStr, "WarrantMM");// = new DataTable("WarrantMM");
            Console.WriteLine("CMoneyCount:" + WarrantMM.Rows.Count);*/
            for (int i = 0; i < 10; i++)           
                Console.WriteLine(TradeDate.IsTradeDay(DateTime.Today.AddDays(-i)));
            for (int i = 0; i < 10; i++)
                Console.WriteLine(TradeDate.LastNTradeDate(i));
            Console.WriteLine(TradeDate.LastNTradeDate(-1));

            //Console.WriteLine(TradeDate.LastNTradeDate(1));
            Console.ReadKey();
        }
    }
}
