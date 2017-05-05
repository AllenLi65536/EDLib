using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

namespace EDLib.SQL
{
    public class SQL
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
    }
}
