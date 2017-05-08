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
        /// <param name="cmd">SQL command</param>
        /// <param name="dataTableName">Name of DataTable to be returned</param>
        /// <returns>A DataTable containing queried data</returns>
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
        /// <param name="sql">SQL command string</param>
        /// <param name="conn">SQL server connection</param>
        /// <param name="dataTableName">Name of DataTable to be returned</param>
        /// <returns>A DataTable containing queried data</returns>
        public static DataTable ExecSqlQry(string sql, SqlConnection conn, string dataTableName = null) {
            using (SqlCommand cmd = new SqlCommand(sql, conn)) {
                return ExecSqlQry(cmd, dataTableName);
            }
        }
        /// <summary>
        /// Execute MS SQL query
        /// </summary>
        /// <param name="sql">SQL command string</param>
        /// <param name="connstr">SQL server connection string</param>
        /// <param name="dataTableName">Name of DataTable to be returned</param>
        /// <returns>A DataTable containing queried data</returns>
        public static DataTable ExecSqlQry(string sql, string connstr, string dataTableName = null) {
            using (SqlConnection conn = new SqlConnection(connstr)) {
                conn.Open();
                return ExecSqlQry(sql, conn, dataTableName);
            }
        }

        /// <summary>
        /// Execute MS SQL command
        /// </summary>
        /// <param name="sql">SQL command string</param>
        /// <param name="conn">SQL server connection</param>
        /// <returns>Successful or not</returns>
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
        /// <param name="sql">SQL command string</param>
        /// <param name="connstr">SQL server connection string</param>
        /// <returns>Successful or not</returns>
        public static bool ExecSqlCmd(string sql, string connstr) {
            using (SqlConnection conn = new SqlConnection(connstr)) {
                conn.Open();
                return ExecSqlCmd(sql, conn);
            }
        }

        /// <summary>
        /// Execute MySQL query
        /// </summary>
        /// <param name="cmd">MySQL command</param>
        /// <param name="dataTableName">Name of DataTable to be returned</param>
        /// <returns>A DataTable containing queried data</returns>
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
        /// Execute MySQL query
        /// </summary>
        /// <param name="sql">MySQL command string</param>
        /// <param name="conn">MySQL server connection</param>
        /// <param name="dataTableName">Name of DataTable to be returned</param>
        /// <returns>A DataTable containing queried data</returns>
        public static DataTable ExecMySqlQry(string sql, MySqlConnection conn, string dataTableName = null) {
            using (MySqlCommand cmd = new MySqlCommand(sql, conn)) {
                return ExecMySqlQry(cmd);
            }
        }

        /// <summary>
        /// Execute MySQL query
        /// </summary>
        /// <param name="sql">MySQL command string</param>
        /// <param name="connstr">MySQL server connection string</param>
        /// <param name="dataTableName">Name of DataTable to be returned</param>
        /// <returns>A DataTable containing queried data</returns>
        public static DataTable ExecMySqlQry(string sql, string connstr, string dataTableName = null) {
            using (MySqlConnection conn = new MySqlConnection(connstr)) {
                conn.Open();
                return ExecMySqlQry(sql, conn, dataTableName);
            }
        }

        /// <summary>
        /// Execute CMoney query
        /// </summary>
        /// <param name="sql">CMoney SQL command string</param>
        /// <param name="dataTableName">Name of DataTable to be returned</param>
        /// <returns>A DataTable containing queried data</returns>
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
