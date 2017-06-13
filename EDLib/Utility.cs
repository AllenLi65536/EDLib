using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace EDLib
{
    /// <summary>
    /// Miscellaneous utility functions
    /// </summary>   
    public static class Utility
    {
        /// <summary>
        /// Commodity type
        /// </summary>
        public enum CommodityType
        {
            ///<summary>Stock</summary>
            Stock,
            ///<summary>Index Future</summary>
            IndexFuture,
            ///<summary>Warrant</summary>
            Warrant,
            ///<summary>ETF</summary>
            ETF,
            ///<summary>Others</summary>
            Others
        };

        /// <summary>
        /// Get the commodity type of ID
        /// </summary>
        /// <param name="ID">Commodity ID</param>
        /// <returns>CommodityType</returns>
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

        /// <summary>
        /// Get response string from the url
        /// </summary>
        /// <param name="url">The URL</param>
        /// <param name="encode">Encoding</param>
        /// <returns>Response from the webpage request</returns>
        public static string GetHtml(string url, Encoding encode) {
            try {                
                using (Stream dataStream = WebRequest.Create(url).GetResponse().GetResponseStream()) {
                    using (StreamReader reader = new StreamReader(dataStream, encode)) {
                        return reader.ReadToEnd();
                    }
                }
            } catch (Exception err) {
                Console.WriteLine(err);
                return err.ToString();
            }
        }

        /// <summary>
        /// Save DataTable into .csv file
        /// </summary>
        /// <param name="dt">DataTable to be saved</param>
        /// <param name="filePath">File path</param>
        /// <param name="containHeader">Should the file contains header row or not</param>
        public static void SaveToCSV(DataTable dt, string filePath, bool containHeader = false) {
            StringBuilder sb = new StringBuilder();

            if (containHeader) {
                IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);                
                sb.AppendLine(string.Join(",", columnNames));
            }

            foreach (DataRow row in dt.Rows) {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.Default);
        }
        /*
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
        }*/
    }
    /*
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

