using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace EDLib
{
    /// <summary>
    /// Manipulate bitmask
    /// </summary>
    public static class FlagsHelper
    {
        /// <summary>
        /// Is flag set in flags
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="flags">Flags</param>
        /// <param name="flag">Flag</param>
        /// <returns>Is set or not</returns>
        public static bool IsSet<T>(T flags, T flag) where T : struct {
            int flagsValue = (int) (object) flags;
            int flagValue = (int) (object) flag;

            return (flagsValue & flagValue) != 0;
        }

        /// <summary>
        /// Set the flag in flags
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="flags">Flags</param>
        /// <param name="flag">Flag</param>
        public static void Set<T>(ref T flags, T flag) where T : struct {
            int flagsValue = (int) (object) flags;
            int flagValue = (int) (object) flag;

            flags = (T) (object) (flagsValue | flagValue);
        }
        /// <summary>
        /// Unset the flag in flags
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="flags">Flags</param>
        /// <param name="flag">Flag</param>
        public static void Unset<T>(ref T flags, T flag) where T : struct {
            int flagsValue = (int) (object) flags;
            int flagValue = (int) (object) flag;

            flags = (T) (object) (flagsValue & (~flagValue));
        }
    }

    /// <summary>
    /// Permutations and Combinations
    /// </summary>
    public static class PermutationsAndCombinations
    {
        /// <summary>
        /// Combination
        /// </summary>
        /// <param name="n">n items to choose from</param>
        /// <param name="r">Choose r items</param>
        /// <returns>Number of combinations</returns>
        public static long nCr(int n, int r) {
            // naive: return Factorial(n) / (Factorial(r) * Factorial(n - r));
            return nPr(n, r) / Factorial(r);
        }

        /// <summary>
        /// Permutation
        /// </summary>
        /// <param name="n">n items to choose from</param>
        /// <param name="r">Permute r items</param>
        /// <returns>Number of permutations</returns>
        public static long nPr(int n, int r) {
            // naive: return Factorial(n) / Factorial(n - r);
            return FactorialDivision(n, n - r);
        }

        private static long FactorialDivision(int topFactorial, int divisorFactorial) {
            long result = 1;
            for (int i = topFactorial; i > divisorFactorial; i--)
                result *= i;
            return result;
        }

        private static long Factorial(int i) {
            if (i <= 1)
                return 1;
            return i * Factorial(i - 1);
        }
    }

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
                using (Stream dataStream = WebRequest.Create(url).GetResponse().GetResponseStream())
                using (StreamReader reader = new StreamReader(dataStream, encode))
                    return reader.ReadToEnd();

            } catch (Exception err) {
                Console.WriteLine(err);
                return err.ToString();
            }
        }
        /// <summary>
        /// Get local IP address that begins with 10.*
        /// </summary>
        /// <returns>IP address</returns>
        /// <exception cref="Exception">Local IP Address Not Found!</exception>
        public static string GetLocalIPAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString().StartsWith("10"))
                    return ip.ToString();
            throw new Exception("Local IP Address Not Found!");
        }

        /// <summary>
        /// Save DataTable into .csv file
        /// </summary>
        /// <param name="dt">DataTable to be saved</param>
        /// <param name="filePath">File path</param>
        /// <param name="containHeader">Should the file contains header row or not</param>
        public static void SaveToCSV(DataTable dt, string filePath, bool containHeader = false) {
            SaveToCSV(dt, filePath, Encoding.Default, containHeader);
        }

        /// <summary>
        /// Save DataTable into .csv file
        /// </summary>
        /// <param name="dt">DataTable to be saved</param>
        /// <param name="filePath">File path</param>
        /// <param name="encoding">Encoding of file</param>
        /// <param name="containHeader">Should the file contains header row or not</param>
        public static void SaveToCSV(DataTable dt, string filePath, Encoding encoding, bool containHeader = false) {
            StringBuilder sb = new StringBuilder();

            if (containHeader) {
                IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
                sb.AppendLine(string.Join(",", columnNames));
            }

            foreach (DataRow row in dt.Rows) {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(filePath, sb.ToString(), encoding);
        }

        /// <summary>
        /// Read from csv file and store values into a datatable
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="containsHeaders">Did the file contains header row or not</param>
        /// <returns>DataTable contains the contents of CSV file</returns>
        public static DataTable CSVtoDataTable(string filePath, bool containsHeaders = false) {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(filePath)) {
                string[] headers = sr.ReadLine().Split(',');
                if (containsHeaders) {
                    foreach (string header in headers)
                        dt.Columns.Add(header);
                } else {
                    for (int i = 0; i < headers.Length; i++)
                        dt.Columns.Add();
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                        dr[i] = headers[i];
                    dt.Rows.Add(dr);
                }
                while (!sr.EndOfStream) {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                        dr[i] = rows[i];
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        /// <summary>
        /// Is the directory empty
        /// </summary>
        /// <param name="path">Path of directory</param>
        /// <returns>Is the directory empty</returns>
        public static bool IsDirectoryEmpty(string path) {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        /// <summary>
        /// Get ID of nearby futures contract
        /// </summary>
        /// <param name="nDays">Number of days to shift</param>
        /// <returns>A letter and a number that represent the expirary month and year.(e.g. H7 for Aug. 2017)</returns>
        public static string GetFutureContractID(int nDays = 0) {
            //DateTime today = DateTime.Today.AddDays(nDays);// new DateTime(int.Parse(Date.Substring(0, 4)), int.Parse(Date.Substring(4, 2)), int.Parse(Date.Substring(6, 2)));
            return GetFutureContractID(DateTime.Today.AddDays(nDays));
        }
        /// <summary>
        /// Get ID of nearby futures contract of the date
        /// </summary>
        /// <param name = "today" >The date</param>
        /// <returns>A letter and a number that represent the expirary month and year.(e.g. H7 for Aug. 2017)</returns>
        public static string GetFutureContractID(DateTime today) {
            //DateTime today = DateTime.Today.AddDays(nDays);// new DateTime(int.Parse(Date.Substring(0, 4)), int.Parse(Date.Substring(4, 2)), int.Parse(Date.Substring(6, 2)));
            DateTime dt = today.AddDays(1 - today.Day);
            int iNth = 0;
            while (dt <= today) {
                if (dt.DayOfWeek == DayOfWeek.Wednesday)
                    iNth++;
                dt = dt.AddDays(1);
            }
            if (iNth >= 3)
                return (char) (today.AddMonths(1).Month + 64) + Convert.ToString(today.AddMonths(1).Year % 10);
            else
                return (char) (today.Month + 64) + Convert.ToString(today.Year % 10);
        }

        /// <summary>
        /// Send Wake On Lan packet.
        /// </summary>
        /// <param name="MAC_ADDRESS">MAC Address</param>
        /// <param name="IPBCast">Broadcast IP address, usually 255.255.255.255 will work</param>
        public static void WakeUp(string MAC_ADDRESS, IPAddress IPBCast) {

            using (UdpClient UDP = new UdpClient()) {
                UDP.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

                int offset = 0;
                byte[] buffer = new byte[512];   // more than enough :-)

                //first 6 bytes should be 0xFF
                for (int y = 0; y < 6; y++)
                    buffer[offset++] = 0xFF;

                //now repeate MAC 16 times
                for (int y = 0; y < 16; y++) {
                    int i = 0;
                    for (int z = 0; z < 6; z++) {
                        buffer[offset++] =
                            byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                        i += 2;
                    }
                }

                UDP.EnableBroadcast = true;
                UDP.Send(buffer, 512, new IPEndPoint(IPBCast, 0x1));
            }
        }


        /// <summary>
        /// Unzip .gz file to designated filepath
        /// </summary>
        /// <param name="source">Source filepath</param>
        /// <param name="dest">Destination filepath</param>
        public static void GZipDecompress(string source, string dest) {
            using (FileStream sourceFile = File.OpenRead(source))
            using (FileStream destFile = File.Create(dest))
            using (GZipStream Gzip = new GZipStream(sourceFile, CompressionMode.Decompress, true)) {
                int theByte = Gzip.ReadByte();
                while (theByte != -1) {
                    destFile.WriteByte((byte) theByte);
                    theByte = Gzip.ReadByte();
                }
            }
        }


        /// <summary>
        /// Returns all combinations of an enumerable item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> DifferentCombinations<T>(this IEnumerable<T> elements, int k) {
            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).DifferentCombinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }

        public static IEnumerable<IEnumerable<T>> GetPermutationsWithRept<T>(IEnumerable<T> list, int length) {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutationsWithRept(list, length - 1)
                .SelectMany(t => list,
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length) {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(o => !t.Contains(o)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public static IEnumerable<IEnumerable<T>> GetKCombsWithRept<T>(IEnumerable<T> list, int length) where T : IComparable {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetKCombsWithRept(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) >= 0),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public static IEnumerable<IEnumerable<T>> GetKCombs<T>(IEnumerable<T> list, int length) where T : IComparable {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
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

