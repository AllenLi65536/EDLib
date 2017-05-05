using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

namespace EDLib
{
    public class Utility
    {
        // For reuse of SqlConnection            
        public static DataTable ExecSqlQry(SqlCommand cmd, string dataTableName = null) {
            using (SqlDataAdapter adp = new SqlDataAdapter(cmd)) {                
                DataTable dt;
                if (dataTableName == null)
                    dt = new DataTable();
                else
                    dt = new DataTable(dataTableName);
                adp.Fill(dt);
                return dt;
            }
        }

        public static DataTable ExecSqlQry(string sql, SqlConnection conn, string dataTableName = null) {
            using (SqlCommand cmd = new SqlCommand(sql, conn)) {
                return ExecSqlQry(cmd, dataTableName);
            }
        }
        public static DataTable ExecSqlQry(string sql, string connstr, string dataTableName = null) {
            using (SqlConnection conn = new SqlConnection(connstr)) {
                conn.Open();
                return ExecSqlQry(sql, conn, dataTableName);
            }
        }
        
        public static bool ExecSqlCmd(string sql, SqlConnection conn) {
            try {
                bool wasClosed = false;                
                if (conn.State == ConnectionState.Closed) {
                    conn.Open();
                    wasClosed = true;
                }
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandTimeout = 300;
                cmd.ExecuteNonQuery();
                if (wasClosed)
                    conn.Close();
                return true;
            } catch {
                return false;
            }
        }

        public static bool ExecSqlCmd(string sql, string connstr) {
            using (SqlConnection conn = new SqlConnection(connstr)) {
                conn.Open();
                return ExecSqlCmd(sql, conn);
            }
        }

        public static DataTable ExecMySqlQry(MySqlCommand cmd, string dataTableName = null) {
            using (MySqlDataAdapter adp = new MySqlDataAdapter(cmd)) {
                DataTable dt;
                if (dataTableName == null)
                    dt = new DataTable();
                else
                    dt = new DataTable(dataTableName);
                adp.Fill(dt);
                return dt;
            }
        }

        public static DataTable ExecMySqlQry(string sql, MySqlConnection conn, string dataTableName = null) {
            using (MySqlCommand cmd = new MySqlCommand(sql, conn)) {
                return ExecMySqlQry(cmd);
            }

            /*using (MySqlDataAdapter adp = new MySqlDataAdapter(sql, conn)) {
                DataTable dt;
                if (dataTableName == null)
                    dt = new DataTable();
                else
                    dt = new DataTable(dataTableName);
                adp.Fill(dt);
                return dt;
            }*/
        }

        public static DataTable ExecMySqlQry(string sql, string connstr, string dataTableName = null) {
            using (MySqlConnection conn = new MySqlConnection(connstr)) {
                conn.Open();
                return ExecMySqlQry(sql, conn, dataTableName);
            }
        }

        public static DataTable ExecCMoneyQry(string sql, string dataTableName = null) {
            CMADODB5.CMConnection conobj = new CMADODB5.CMConnection();
            ADODB.Recordset rs = new ADODB.Recordset();
            rs = conobj.CMExecute("5", "10.60.0.191", "", sql);//1433
            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter()) {
                DataTable dt;
                if (dataTableName == null)
                    dt = new DataTable();
                else
                    dt = new DataTable(dataTableName);
                dataAdapter.Fill(dt, rs);
                return dt;
            }
        }


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
    public class dMath
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
    }
}

