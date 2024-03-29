﻿using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Threading.Tasks;

namespace EDLib.SQL
{
    /// <summary>
    /// Some useful SQL helper functions
    /// </summary>
    internal class NamespaceDoc { }

    /// <summary>
    /// MSSQL query assistant
    /// </summary>
    /// <example>
    /// <code> 
    /// //ExecSqlQry example
    /// DataTable Warrants = MSSQL.ExecSqlQry("select distinct TraderId,StkId,WId from Warrants where (MarketDate &lt;= CONVERT(varchar(10), GETDATE(), 111) and CONVERT(varchar(10), GETDATE(), 111) &lt;= LastTradeDate) and kgiwrt='自家'", GlobalParameters.WMM3);
    /// Console.WriteLine("Warrants:" + Warrants.Rows.Count);
    /// 
    /// //ExecSqlCmd example
    /// SqlConnection conn = new SqlConnection("Data Source=server;Initial Catalog=DB;User ID=user;Password=password");               
    /// conn.Open();
    /// MSSQL.ExecSqlCmd("DELETE FROM Table1 WHERE TDate ='20170508' ", conn); 
    /// conn.Close();
    /// </code>
    /// </example>
    public static class MSSQL
    {
        /// <summary>
        /// Execute MS SQL query
        /// </summary>
        /// <param name="cmd">SQL command</param>
        /// <param name="dataTableName">Name of DataTable to be returned</param>
        /// <returns>A DataTable containing queried data</returns>
        public static DataTable ExecSqlQry(SqlCommand cmd, string dataTableName = null) {
            using (SqlDataAdapter adp = new SqlDataAdapter(cmd)) {
                DataTable dt = new DataTable();
                if (dataTableName != null)
                    dt.TableName = dataTableName;
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
        /// <param name="timeout">SQL server timeout seconds</param>
        /// <returns>Number of data rows affected</returns>
        public static int ExecSqlCmd(string sql, SqlConnection conn, int timeout = 300) {
            bool wasClosed = false;
            if (conn.State == ConnectionState.Closed) {
                conn.Open();
                wasClosed = true;
            }
            int ret;
            using (SqlCommand cmd = new SqlCommand(sql, conn)) {
                cmd.CommandTimeout = timeout;
                ret = cmd.ExecuteNonQuery();
            }
            if (wasClosed)
                conn.Close();
            return ret;
        }

        /// <summary>
        /// Execute MS SQL command
        /// </summary>
        /// <param name="sql">SQL command string</param>
        /// <param name="connstr">SQL server connection string</param> 
        /// <param name="timeout">SQL server timeout seconds</param>
        /// <returns>Number of data rows affected</returns>
        public static int ExecSqlCmd(string sql, string connstr, int timeout = 300) {
            using (SqlConnection conn = new SqlConnection(connstr)) {
                conn.Open();
                return ExecSqlCmd(sql, conn, timeout);
            }
        }

        /// <summary>
        /// Execute MS SQL command
        /// </summary>
        /// <param name="sql">SQL command string</param>
        /// <param name="conn">SQL server connection</param>
        /// <param name="timeout">SQL server timeout seconds</param>
        /// <returns>Number of data rows affected</returns>
       /* public static async Task<int> ExecSqlCmdAsync(string sql, SqlConnection conn, int timeout = 300) {
            bool wasClosed = false;
            if (conn.State == ConnectionState.Closed) {
                conn.Open();
                wasClosed = true;
            }
            int ret;
            using (SqlCommand cmd = new SqlCommand(sql, conn)) {
                cmd.CommandTimeout = timeout;
                ret = await cmd.ExecuteNonQueryAsync();
            }
            if (wasClosed)
                conn.Close();
            return ret;
        }

        /// <summary>
        /// Execute MS SQL command
        /// </summary>
        /// <param name="sql">SQL command string</param>
        /// <param name="connstr">SQL server connection string</param> 
        /// <param name="timeout">SQL server timeout seconds</param>
        /// <returns>Number of data rows affected</returns>
        public static async Task<int> ExecSqlCmdAsync(string sql, string connstr, int timeout = 300) {
            using (SqlConnection conn = new SqlConnection(connstr)) {
                conn.Open();
                return await ExecSqlCmdAsync(sql, conn, timeout);
            }
        }*/

    }
    /// <summary>
    /// MySQL query assistant
    /// </summary>
    public static class MySQL
    {
        /// <summary>
        /// Execute MySQL command
        /// </summary>
        /// <param name="sql">SQL command string</param>
        /// <param name="conn">SQL server connection</param>
        /// <param name="timeout">SQL server timeout seconds</param>
        /// <returns>Number of data rows affected</returns>
        public static int ExecMySqlCmd(string sql, MySqlConnection conn, int timeout = 300) {
            bool wasClosed = false;
            if (conn.State == ConnectionState.Closed) {
                conn.Open();
                wasClosed = true;
            }
            int ret;
            using (MySqlCommand cmd = new MySqlCommand(sql, conn)) {
                cmd.CommandTimeout = timeout;
                ret = cmd.ExecuteNonQuery();
            }
            if (wasClosed)
                conn.Close();
            return ret;
        }

        /// <summary>
        /// Execute MySQL command
        /// </summary>
        /// <param name="sql">SQL command string</param>
        /// <param name="connstr">SQL server connection string</param> 
        /// <param name="timeout">SQL server timeout seconds</param>
        /// <returns>Number of data rows affected</returns>
        public static int ExecMySqlCmd(string sql, string connstr, int timeout = 300) {
            using (MySqlConnection conn = new MySqlConnection(connstr)) {
                conn.Open();
                return ExecMySqlCmd(sql, conn, timeout);
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
                DataTable dt = new DataTable();
                if (dataTableName != null)
                    dt.TableName = dataTableName;
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
    }
    /// <summary>
    /// CMoney query assistant
    /// </summary>
    /// <example>
    /// <code>
    /// //ExecCmoneyQry example
    /// string SQLStr = "SELECT 代號 ,isnull(權證成交量,0),isnull([權證成交金額(千)],0) from 權證評估表 where 日期='20170511'";
    /// DataTable WarrantMM = CMoney.ExecCMoneyQry(SQLStr, "WarrantMM");
    /// Console.WriteLine("CMoneyCount:" + WarrantMM.Rows.Count);
    /// </code>
    /// </example>
    public static class CMoney
    {
        /// <summary>
        /// Execute CMoney query
        /// </summary>
        /// <param name="sql">CMoney SQL command string</param>
        /// <param name="dataTableName">Name of DataTable to be returned</param>
        /// <returns>A DataTable containing queried data</returns>
        public static DataTable ExecCMoneyQry(string sql, string dataTableName = null) {
            CMADODB5.CMConnection conobj = new CMADODB5.CMConnection();
            ADODB.Recordset rs = conobj.CMExecute("5", "10.60.0.191", "", sql);//1433
            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter()) {
                DataTable dt = new DataTable();
                if (dataTableName != null)
                    dt.TableName = dataTableName;
                dataAdapter.Fill(dt, rs);
                return dt;
            }
        }

    }

}
