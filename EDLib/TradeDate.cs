using System;
using System.Data.SqlClient;
using EDLib.SQL;

namespace EDLib
{
    /// <summary>
    /// Check for TradeDate
    /// </summary>
    /// <example>
    /// <code>
    /// // Print whether last nine calendar days are trade day (including today)
    /// for (int i = 0; i &lt; 10; i++)
    ///     Console.WriteLine(TradeDate.IsTradeDay(DateTime.Today.AddDays(-i)));
    /// // Print last nine trade dates
    /// for (int i = 1; i &lt; 10; i++)
    ///     Console.WriteLine(TradeDate.LastNTradeDate(i));
    /// //Print next ten trade dates
    /// DateTime[] dt = TradeDate.NextNTradeDates(10);
    /// for (int i = 0; i &lt; 10; i++)
    ///     Console.WriteLine(dt[i]);
    /// </code>
    /// </example>
    public static class TradeDate
    {
        /// <summary>
        /// Get last Nth trade day
        /// </summary>
        /// <param name="N">N trade days, N has to be > 0</param>
        /// <param name="region">The currency region: AUD CHF EUR GBP HKD JPY KRW KYD PHP SEK SGD TWD USD</param>
        /// <returns>DateTime of that day</returns>
        /// <exception cref="ArgumentOutOfRangeException">N has to be > 0</exception>
        static public DateTime LastNTradeDate(int N, string region = "TWD") {            
            return GetNTradeDate(N, region, -1);
        }

        /// <summary>
        /// Get next Nth trade day
        /// </summary>
        /// <param name="N">N trade days, N has to be > 0</param>
        /// <param name="region">The currency region: AUD CHF EUR GBP HKD JPY KRW KYD PHP SEK SGD TWD USD</param>
        /// <returns>DateTime of that day</returns>
        /// <exception cref="ArgumentOutOfRangeException">N has to be > 0</exception>
        static public DateTime NextNTradeDate(int N, string region = "TWD") {            
            return GetNTradeDate(N, region, 1);
        }

        static private DateTime GetNTradeDate(int N, string region, int nextOrLast) {
            if (N <= 0)
                throw new ArgumentOutOfRangeException("N", "N has to be > 0");
            DateTime retDate = DateTime.Today;
            using (SqlConnection conn2 = new SqlConnection(GlobalParameters.hedgeSqlConnStr)) {
                conn2.Open();

                while (N > 0) {
                    retDate = retDate.AddDays(nextOrLast);
                    string date = retDate.ToString("yyyyMMdd");

                    using (SqlCommand cmd2 = new SqlCommand("Select HOL_DATE from HOLIDAY where CCY='" + region + "' and HOL_DATE='" + date + "'", conn2))
                    using (SqlDataReader holiday = cmd2.ExecuteReader())
                        if (!holiday.HasRows)
                            N--;
                }
            }
            return retDate;
        }

        /// <summary>
        /// Get next N trade days
        /// </summary>
        /// <param name="N">N trade days, N has to be > 0</param>
        /// <param name="region">The currency region: AUD CHF EUR GBP HKD JPY KRW KYD PHP SEK SGD TWD USD</param>
        /// <returns>DateTimes of those days</returns>
        /// <exception cref="ArgumentOutOfRangeException">N has to be > 0</exception>
        static public DateTime[] NextNTradeDates(int N, string region = "TWD") {                     
            return GetNTradeDates(N, region, 1);
        }

        /// <summary>
        /// Get last N trade days
        /// </summary>
        /// <param name="N">N trade days, N has to be > 0</param>
        /// <param name="region">The currency region: AUD CHF EUR GBP HKD JPY KRW KYD PHP SEK SGD TWD USD</param>
        /// <returns>DateTimes of those days</returns>
        /// <exception cref="ArgumentOutOfRangeException">N has to be > 0</exception>
        static public DateTime[] LastNTradeDates(int N, string region = "TWD") {               
            return GetNTradeDates(N, region, -1);
        }
        static private DateTime[] GetNTradeDates(int N, string region, int nextOrLast) {
            if (N <= 0)
                throw new ArgumentOutOfRangeException("N", "N has to be > 0");
            DateTime[] retDate = new DateTime[N];
            DateTime tmpDate = DateTime.Today;

            using (SqlConnection conn2 = new SqlConnection(GlobalParameters.hedgeSqlConnStr)) {
                conn2.Open();
                int i = 0;
                while (N > i) {
                    tmpDate = tmpDate.AddDays(nextOrLast);
                    string date = tmpDate.ToString("yyyyMMdd");

                    using (SqlCommand cmd2 = new SqlCommand("Select HOL_DATE from HOLIDAY where CCY='" + region + "' and HOL_DATE='" + date + "'", conn2))
                    using (SqlDataReader holiday = cmd2.ExecuteReader())
                        if (!holiday.HasRows)
                            retDate[i++] = tmpDate;                                
                }
            }
            return retDate;
        }

        /// <summary>
        /// Is the day trade day
        /// </summary>
        /// <param name="day">The day</param>
        /// <param name="region">The currency region: AUD CHF EUR GBP HKD JPY KRW KYD PHP SEK SGD TWD USD</param>
        /// <returns>True of false</returns>
        /// <exception cref="ArgumentOutOfRangeException">day has to be >= 2008/2/2</exception>
        static public bool IsTradeDay(DateTime day, string region = "TWD") {
            if (day < new DateTime(2008, 2, 2))
                throw new ArgumentOutOfRangeException("day", "day has to be >= 2008/2/2");

            string date = day.ToString("yyyyMMdd");
            using (SqlConnection conn2 = new SqlConnection(GlobalParameters.hedgeSqlConnStr)) {
                conn2.Open();
                using (SqlCommand cmd2 = new SqlCommand("Select HOL_DATE from HOLIDAY where CCY='" + region + "' and HOL_DATE='" + date + "'", conn2))
                using (SqlDataReader holiday = cmd2.ExecuteReader())
                    if (holiday.HasRows)
                        return false;
            }
            return true;
        }
        /// <summary>
        /// Is today trade day
        /// </summary>
        /// <returns>True or false</returns>
        static public bool IsTodayTradeDay() {
            return IsTradeDay(DateTime.Now);
        }
    }
}