using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace EDLib
{
    /// <summary>
    /// Miscellaneous classes
    /// </summary>
    internal class NamespaceDoc { }

    /// <summary>
    /// TIBCO Rendezvous parameters
    /// </summary>
    public class RVParameters
    {
        /// <summary>
        /// RV service parameter
        /// </summary>
        public readonly string service;
        /// <summary>
        /// RV network parameter
        /// </summary>
        public readonly string network;
        /// <summary>
        /// RV daemon parameter
        /// </summary>
        public readonly string daemon;
        /// <summary>
        /// RV topic parameter
        /// </summary>
        public readonly string topic;
        /// <summary>
        /// TIBCO Rendezvous parameters
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="network">network</param>
        /// <param name="daemon">daemon</param>
        /// <param name="topic">topic</param>
        public RVParameters(string service, string network, string daemon, string topic) {
            this.service = service;
            this.network = network;
            this.daemon = daemon;
            this.topic = topic;
        }

    }
    /// <summary>
    /// Global variables and parameters
    /// </summary>
    public static class GlobalParameters
    {
        /// <summary>
        /// SQL server connection string of HEDGE
        /// </summary>
        public static readonly string HEDGE = "Data Source=10.101.10.5;Initial Catalog=HEDGE;User ID=hedgeuser;Password=hedgeuser";
        /// <summary>
        /// SQL server connection string of WMM3
        /// </summary>
        public static readonly string WMM3 = "Data Source=10.101.10.5;Initial Catalog=WMM3;User ID=hedgeuser;Password=hedgeuser";

        /// <summary>
        /// WMM3 Log RV parameters
        /// </summary>
        public static readonly RVParameters WMMLog = new RVParameters("9082", ";239.16.1.6", "10.60.0.128:7500", "TW.ED.WMM3.CLIENT.LOG");
        /// <summary>
        /// PM RV parameters
        /// </summary>
        public static readonly RVParameters PM = new RVParameters("9013", "172.31.2;239.16.1.72", "10.60.0.101:7500", "TW.WMM3.PM.PositionReport.>");
        /// <summary>
        /// Market liquidity RV parameters
        /// </summary>
        public static readonly RVParameters Liquidity = new RVParameters(null, "172.31.2;239.16.1.72", "10.60.0.128:7500", "MarketLiquidityInfo.*");
        /// <summary>
        /// TWSE quotes RV parameters
        /// </summary>
        public static readonly RVParameters TWSE = new RVParameters(null, "172.31.2;239.16.1.72", "10.60.0.128:7500", "TWSE.MarketDataSnapshotFullRefresh");
        /// <summary>
        /// Warrant execution report for calculating Slippage cost RV parameters
        /// </summary>
        public static readonly RVParameters Slippage = new RVParameters(null, "172.31.2;239.16.1.72", "10.60.0.101:7500", "TW.WMM3.SlippageCost.HedgeInfo.PROD");
        /// <summary>
        /// Warrant execution report for calculating Slippage cost RV parameters
        /// </summary>
        public static readonly RVParameters ExecutionReport = new RVParameters(null, "172.31.2;239.16.1.72", "10.60.0.129:7500", "TW.WMM3.FilledReportRelayService.ExecutionReport.PROD");
               
    }

    /// <summary>
    /// Miscellaneous
    /// </summary>
    /*public static class Utility
    {       
        public static bool FunChechSum(byte[] nCheckByte, byte nCheckSum) {
            int XORMask = 0;

            //以Bit處理結果和以Byte相同
            //nCheckByte = Big5.GetBytes(sChechSum);
            try {
                for (int nCount = 0; nCount < nCheckByte.Length; nCount++)
                    XORMask ^= nCheckByte[nCount];

                if (XORMask != (int) nCheckSum)
                    return false;
                return true;
            } catch {
                return false;
            }
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static void WriteIniString(string section, string key, string val, string filePath) {
            WritePrivateProfileString(section, key, val, filePath);
        }
        public static string ReadIniString(string section, string key, string filePath) {
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", temp, 255, filePath);
            return temp.ToString();
        }
        public static bool IsNaturalNumber(string str) {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
            return reg1.IsMatch(str);
        }
    }
    public static class dMath
    {
        //Returns the standard deviation of data array
        private static double StandardDeviation(double[] data) {
            double DataAverage = 0;
            double TotalVariance = 0;
            int Max = data.Length;

            if (Max == 0)
                return 0;

            DataAverage = data.Average();//Average(data);

            for (int i = 0; i < Max; i++)
                TotalVariance += (data[i] - DataAverage) * (data[i] - DataAverage); //Better performance than Math.pow

            return Math.Sqrt(TotalVariance / Max);
        }


        private static double SafeDivide(double value1, double value2) {
            if (value2 != 0)
                return value1 / value2;
            return double.PositiveInfinity;
        }
    }*/
}

