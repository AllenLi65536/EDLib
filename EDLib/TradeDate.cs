using System;
using System.Data.SqlClient;

namespace EDLib
{
    /// <summary>
    /// Check for TradeDate
    /// </summary>
    public static class TradeDate
    {
        /// <summary>
        /// Get last Nth trade day
        /// </summary>
        /// <param name="N">N trade days, N has to be >= 0</param>
        /// <returns>DateTime of that day</returns>
        /// <exception cref="ArgumentOutOfRangeException">N has to be >= 0</exception>
        static public DateTime LastNTradeDate(int N) {
            if (N < 0) 
                throw new ArgumentOutOfRangeException("N", "N has to be >= 0");            

            //Get Last Trading Date          
            SqlConnection conn2 = new SqlConnection(GlobalParameters.HEDGE);
            conn2.Open();

            int nDays = 0;
            DateTime retDate;
            SqlDataReader holiday = null;
            //DataTable holiday;
            if (N == 0)
                return DateTime.Today;

            do {                
                retDate = DateTime.Today.AddDays(--nDays);
                string date = retDate.ToString("yyyyMMdd");
                using (SqlCommand cmd2 = new SqlCommand("Select HOL_DATE from HOLIDAY where CCY='TWD' and HOL_DATE='" + date + "'", conn2)) {
                    //holiday = SQL.SQL.ExecSqlQry(cmd2);
                    if (holiday != null)
                        holiday.Close();
                    holiday = cmd2.ExecuteReader();
                }
                if (!holiday.HasRows)
                    N--;
            } while (N > 0 || holiday.HasRows);

            conn2.Close();
            holiday.Close();
            return retDate;
        }
        /// <summary>
        /// Is the day trade day
        /// </summary>
        /// <param name="day">The day</param>
        /// <returns>True of false</returns>
        /// <exception cref="ArgumentOutOfRangeException">day has to be >= 2008/2/2</exception>
        static public bool IsTradeDay(DateTime day) {
            if (day < new DateTime(2008, 2, 2))
                throw new ArgumentOutOfRangeException("day", "day has to be >= 2008/2/2");

            string date = day.ToString("yyyyMMdd");
            SqlConnection conn2 = new SqlConnection(GlobalParameters.HEDGE);
            conn2.Open();
            SqlCommand cmd2 = new SqlCommand("Select HOL_DATE from HOLIDAY where CCY='TWD' and HOL_DATE='" + date + "'", conn2);
            SqlDataReader holiday = cmd2.ExecuteReader();
            if (holiday.HasRows)
                return false;
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