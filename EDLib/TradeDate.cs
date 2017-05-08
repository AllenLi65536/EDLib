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
        /// <param name="N">N trade days</param>
        /// <returns>string of that day with format "yyyyMMdd"</returns>
        static public string LastNTradeDate(int N) {
            return LastNTradeDateDT(N).ToString("yyyyMMdd");
        }

        /// <summary>
        /// Get last Nth trade day
        /// </summary>
        /// <param name="N">N trade days</param>
        /// <returns>DateTime of that day</returns>
        static public DateTime LastNTradeDateDT(int N) {
            //Get Last Trading Date
            int nDays = -1;
            DateTime retDate = DateTime.Today.AddDays(nDays);
            string date = retDate.ToString("yyyyMMdd");

            SqlConnection conn2 = new SqlConnection("Data Source=10.101.10.5;Initial Catalog=HEDGE;User ID=hedgeuser;Password=hedgeuser");
            conn2.Open();
            SqlCommand cmd2 = new SqlCommand("Select * from HOLIDAY where CCY='TWD' and HOL_DATE='" + date + "'", conn2);
            SqlDataReader holiday = cmd2.ExecuteReader();
            if (!holiday.HasRows)
                N--;

            while (holiday.HasRows || N != 0) {
                nDays--;

                retDate = DateTime.Today.AddDays(nDays);
                date = retDate.ToString("yyyyMMdd");

                cmd2 = new SqlCommand("Select * from HOLIDAY where CCY='TWD' and HOL_DATE='" + date + "'", conn2);
                holiday.Close();
                holiday = cmd2.ExecuteReader();
                if (!holiday.HasRows)
                    N--;
            }

            conn2.Close();
            cmd2.Dispose();
            holiday.Close();
            return retDate;
        }
        /// <summary>
        /// Is the day trade day
        /// </summary>
        /// <param name="day">The day</param>
        /// <returns>True of false</returns>
        static bool IsTradeDay(DateTime day) {
            SqlConnection sqlConn = new SqlConnection("Server=10.19.1.20;DataBase=VOLDB;Uid=sa;pwd=dw910770;");
            sqlConn.Open();
            SqlCommand sqlCmd = new SqlCommand("", sqlConn);
            SqlDataReader reader = null;
            if (day.DayOfWeek == DayOfWeek.Sunday)
                return false;
            if (day.DayOfWeek == DayOfWeek.Saturday) {
                //檢查星期六補假
                sqlCmd.CommandText = "SELECT COUNT(*) FROM OutWeekend WHERE TDate='" + day.ToString("yyyy-MM-dd") + "'";
                reader = sqlCmd.ExecuteReader();
                if (reader.Read()) {
                    if (Convert.ToInt32(reader[0].ToString()) == 0)
                        return false;
                } else
                    return false;
            } else {
                //檢查今天是否放假
                sqlCmd.CommandText = "SELECT COUNT(*) FROM Holiday WHERE Date='" + day.ToString("yyyy-MM-dd") + "'";
                reader = sqlCmd.ExecuteReader();
                if (reader.Read()) {
                    if (Convert.ToInt32(reader[0].ToString()) == 1)
                        return false;
                } else
                    return true;
            }
            if (reader != null)
                reader.Close();
            sqlConn.Close();
            return true;
        }
        /// <summary>
        /// Is today trade day
        /// </summary>
        /// <returns>True or false</returns>
        static bool IsTodayTradeDay() {
            return IsTradeDay(DateTime.Now);
        }
    }
}