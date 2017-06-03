using System;
using System.Text;
using EDLib;
using EDLib.SQL;
using EDLib.TIBCORV;
using System.Data;
using TIBCO.Rendezvous;
using System.Data.SqlClient;
using System.Configuration;

namespace TestLib
{
    class Program
    {
        static HeartbeatMonitor hm = new HeartbeatMonitor(5, myAction);
        static void Main(string[] args) {

            #region TickExample
            /*double price = 498;
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
            }
            double[] bids = Tick.GetAsks("2330", 9.19);
            for (int i = 0; i < 6; i++)
                Console.Write(bids[i] + " ");
            Console.WriteLine(Tick.GetTickNum("2330", 8.8, 8.76));
            Console.WriteLine(Tick.GetTickNum("2330", 9.24, 9.23999999));
            Console.WriteLine(Tick.ShiftTicks("2330", 99, 25));*/
            #endregion

            #region SQLExample
            /*string LastTDate = "20170504";
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
            DataTable WarrantMM = CMoney.ExecCMoneyQry(SQLStr, "WarrantMM");// = new DataTable("WarrantMM");
            Console.WriteLine("CMoneyCount:" + WarrantMM.Rows.Count);*/
            #endregion

            #region TradeDayExample
            /*for (int i = 0; i < 10; i++)
                Console.WriteLine(TradeDate.IsTradeDay(DateTime.Today.AddDays(-i)));
            for (int i = 1; i < 10; i++)
                Console.WriteLine(TradeDate.LastNTradeDate(i));
            for (int i = 1; i < 10; i++)
                Console.WriteLine(TradeDate.NextNTradeDate(i));
            DateTime[] dt = TradeDate.NextNTradeDates(10);
            for (int i = 0; i < 10; i++)
                Console.WriteLine(dt[i]);*/
            #endregion


            //new SleepToTarget(new DateTime(2017, 5, 9), null); //Throws ArgumentNullException
            //new HeartbeatMonitor(1, null); //Throws ArgumentNullException

            //MailService ms = new MailService();
            //ms.SendMail("allen.li@kgi.com", "Test", null, null, new string[] { "allen.li@kgi.com" }, "Test", "test", false, new string[] { "D:\\Document\\ED_NAS_Warrant.bat" });
            //ms.SendMail("kgiBulletin@kgi.com", "內網公告", new string[] { "judy.lu@kgi.com" }, null, new string[] { "allen.li@kgi.com", "andrea.chang@kgi.com" }, "Hello", "ㄋ好", false, null);

            //SleepToTarget st = new SleepToTarget(new DateTime(2017, 5, 15, 11, 58, 0), myAction);
            //st.Start();


            //TIBCORVListener listener = new TIBCORVListener(null, "172.31.2;239.16.1.72", "10.60.0.128:7500", "TWSE.MarketDataSnapshotFullRefresh");

            /*hm.Start();
            TIBCORVListener listener = new TIBCORVListener(GlobalParameters.ExecutionReport);
            //ListenerFunc[] callback = new ListenerFunc[1];
            //callback[0] = new ListenerFunc(OnMessageReceived2);
            ListenerFunc callback = new ListenerFunc(OnMessageReceived2);
            listener.Listen(callback);            */


            /*var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 100000; i++) {
                SlippageCost.Calculate("2330", 50, -30, new double[] { 50, 49, 48, 47, 46 }, new int[] { 12, 11, 1, 1, 1 });
                SlippageCost.Calculate("2330", 100, 30, new double[] { 100, 101, 102, 103, 104 }, new int[] { 12, 11, 1, 1, 1 });
            }
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);*/
            //Console.WriteLine(SlippageCost.Calculate("2330", 50, -10, new double[] {50, 49, 48, 47, 46}, new int[] {4, 1, 1, 1, 1 }));

            /*
            System.Windows.Controls.TextBox txtUserId = new System.Windows.Controls.TextBox();
            System.Windows.Controls.PasswordBox txtPwd = new System.Windows.Controls.PasswordBox();

            Configuration config = Configuration.WebConfigurationManager.OpenWebConfiguration(Null);
            ConnectionStringSettings connString = config.ConnectionStrings.ConnectionString[“MyConnString”];

            using (SqlConnection conn = new SqlConnection(connString.ConnectionString)) {
                SecureString pwd = txtPwd.SecurePassword;
                pwd.MakeReadOnly();
                SqlCredential cred = new SqlCredential(txtUserId.Text, pwd);
                conn.Credential = cred;
                conn.Open();
            }
            */
            Console.WriteLine(Utility.GetHtml("http://warrant.kgi.com/EDWEB/DefaultNew.aspx", Encoding.UTF8));
            //Console.WriteLine(TradeDate.LastNTradeDate(1));
            Console.WriteLine("press any key to exit");
            Console.ReadKey();
        }
        static void myAction() {
            Console.WriteLine("myAction");
            return;
        }
        static void OnMessageReceived2(object listener, MessageReceivedEventArgs messageReceivedEventArgs) {
            Message message = messageReceivedEventArgs.Message;
            //hm.Heartbeat();

            Console.WriteLine(message);
        }
    }
}
