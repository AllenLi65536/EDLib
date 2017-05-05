using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

namespace EDLib.SQL
{
    /// <summary>
    /// MSSQL, MySQL, and CMoney SQL query assistant
    /// </summary>
    public static class SQL
    {                 
        /// <summary>
        /// Execute MS SQL query
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="dataTableName"></param>
        /// <returns>A DataTable</returns>
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

        /// <summary>
        /// Execute MS SQL query
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <param name="dataTableName"></param>
        /// <returns></returns>
        public static DataTable ExecSqlQry(string sql, SqlConnection conn, string dataTableName = null) {
            using (SqlCommand cmd = new SqlCommand(sql, conn)) {
                return ExecSqlQry(cmd, dataTableName);
            }
        }
        /// <summary>
        /// Execute MS SQL query
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connstr"></param>
        /// <param name="dataTableName"></param>
        /// <returns></returns>
        public static DataTable ExecSqlQry(string sql, string connstr, string dataTableName = null) {
            using (SqlConnection conn = new SqlConnection(connstr)) {
                conn.Open();
                return ExecSqlQry(sql, conn, dataTableName);
            }
        }

        /// <summary>
        /// Execute MS SQL command
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Execute MS SQL command
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connstr"></param>
        /// <returns></returns>
        public static bool ExecSqlCmd(string sql, string connstr) {
            using (SqlConnection conn = new SqlConnection(connstr)) {
                conn.Open();
                return ExecSqlCmd(sql, conn);
            }
        }

        /// <summary>
        /// Execute MYSQL query
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="dataTableName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Execute MYSQL query
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <param name="dataTableName"></param>
        /// <returns></returns>
        public static DataTable ExecMySqlQry(string sql, MySqlConnection conn, string dataTableName = null) {
            using (MySqlCommand cmd = new MySqlCommand(sql, conn)) {
                return ExecMySqlQry(cmd);
            }
        }

        /// <summary>
        /// Execute MYSQL query
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connstr"></param>
        /// <param name="dataTableName"></param>
        /// <returns></returns>
        public static DataTable ExecMySqlQry(string sql, string connstr, string dataTableName = null) {
            using (MySqlConnection conn = new MySqlConnection(connstr)) {
                conn.Open();
                return ExecMySqlQry(sql, conn, dataTableName);
            }
        }

        /// <summary>
        /// Execute CMoney query
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dataTableName"></param>
        /// <returns></returns>
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
