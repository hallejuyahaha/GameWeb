using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Common
{
    public class Excute
    {
        //***************************************************************************************************************************
        public static DataSet SelectData(string Select_str)
        {
            SqlConnection con = DBcon.Conn();
            SqlDataAdapter sda = new SqlDataAdapter(Select_str, con);
            DataSet ds = new DataSet();
            try
            {
                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
                sda.Dispose();
            }
            return ds;
        }
        public static SqlDataAdapter SelectSql_AspNetPager(string Select_str)
        {
            SqlConnection con = DBcon.Conn();
            SqlDataAdapter sda = new SqlDataAdapter(Select_str, con);
            con.Close();
            con.Dispose();
            return sda;
            sda.Dispose();
        }
        public static int UpdateData(string sql)
        {
            int action = -1;
            SqlConnection con = DBcon.Conn();
            SqlCommand cmd = new SqlCommand(sql, con);
            try
            {
                con.Open();
                action = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
                cmd.Dispose();
                con.Dispose();
            }
            return action;
        }
        public static void DeleteData(string sql)
        {
            int action = -1;
            SqlConnection con = DBcon.Conn();
            SqlCommand cmd = new SqlCommand(sql, con);
            try
            {
                con.Open();
                action = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
                cmd.Dispose();
                con.Dispose();
            }
        }
        public static void ExcuteCount(string sql)
        {
            SqlConnection con = DBcon.Conn();
            SqlCommand cmd = new SqlCommand(sql, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
            }

        }
        public static void addData(string sql)
        {
            // int action = -1;
            SqlConnection con = DBcon.Conn();
            SqlCommand cmd = new SqlCommand(sql, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
                cmd.Dispose();
                con.Dispose();
            }
        }

        #region 执行SQL语句从而影响数据表内容（多用于添加、修改、删除语句）
        /// <summary>
        /// 执行SQL语句从而影响数据表内容（多用于添加、修改、删除语句）
        /// </summary>
        /// <param name="sql"></param>
        public static void MysqlExecute(string sql)
        {
            try
            {
                SqlConnection conn = new SqlConnection();
                conn = DBcon.Conn();
                conn.Open();

                SqlCommand comm = new SqlCommand(sql, conn);
                comm.ExecuteNonQuery();
                comm.Dispose();
                conn.Close();
                conn.Dispose();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion


        #region 返回DataReader（多用于查询语句）
        public static SqlDataReader MyDataReader(string sql)
        {
            try
            {
                SqlConnection conn = new SqlConnection();
                conn = DBcon.Conn();
                conn.Open();

                SqlCommand comm = new SqlCommand(sql, conn);
                SqlDataReader dr = comm.ExecuteReader(CommandBehavior.CloseConnection);

                return dr;
                comm.Dispose();
                conn.Close();
                conn.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        #endregion

        //**************************************************************************************
        public DataSet databind(string sql, string table)//数据绑定
        {
            DataSet dataset = new DataSet();
            SqlConnection conn = DBcon.Conn();
            conn.Open();
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, conn);
            dataadapter.Fill(dataset, table);
            conn.Close();
            dataadapter.Dispose();
            conn.Dispose();
            return dataset;

        }

        public static DataTable ExecuteQuery(string sql)
        {
            SqlConnection con = DBcon.Conn();
            SqlDataAdapter sda = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            try
            {
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
                sda.Dispose();
            }
            return dt;
        }

        public static DataTable ExecuteQuery(string sql, params SqlParameter[] paras)
        {
            SqlConnection con = DBcon.Conn();
            SqlCommand cmd = new SqlCommand(sql, con);
            if (paras != null && paras.Length > 0)
            {
                cmd.Parameters.AddRange(paras);
            }
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        public static DataTable ExecuteQuery(int startRowIndex, int rowNum, string sql, params SqlParameter[] paras)
        {
            SqlConnection con = DBcon.Conn();
            SqlCommand cmd = new SqlCommand(sql, con);
            if (paras != null && paras.Length > 0)
            {
                cmd.Parameters.AddRange(paras);
            }
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                string tableName = Guid.NewGuid().ToString();
                sda.Fill(ds, startRowIndex, rowNum, tableName);
                return ds.Tables[tableName];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public static DataTable ExecuteQuery(SqlConnection con, SqlTransaction tran, string sql, params SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Transaction = tran;
            cmd.Parameters.AddRange(paras);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(BuildExecuteEorror(sql, paras), ex);
            }
            return dt;
        }

        public static bool Exists(string sql, params SqlParameter[] paras)
        {
            SqlConnection con = DBcon.Conn();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                if (paras.Length > 0)
                {
                    cmd.Parameters.AddRange(paras);
                }
                object value = cmd.ExecuteScalar();
                int rows = 0;
                if (value != null && int.TryParse(value.ToString(), out rows))
                {
                    return rows > 0;
                }
            }
            finally
            {
                con.Close();
            }
            return false;
        }

        public static int Execute(string sql, params SqlParameter[] paras)
        {
            SqlConnection con = DBcon.Conn();
            SqlCommand cmd = new SqlCommand(sql, con);
            if (paras.Length > 0)
            {
                cmd.Parameters.AddRange(paras);
            }
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(BuildExecuteEorror(sql, paras), ex);
            }
            finally
            {
                con.Close();
            }
        }

        public static int Execute(SqlConnection con, SqlTransaction tran, string sql, params SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Transaction = tran;
            if (paras.Length > 0)
            {
                cmd.Parameters.AddRange(paras);
            }
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(BuildExecuteEorror(sql, paras), ex);
            }

        }

        private static string BuildExecuteEorror(string sql, params SqlParameter[] paras)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("SQL语句：" + sql);
            sb.AppendLine("参数：");
            foreach (SqlParameter para in paras)
            {
                sb.AppendFormat("{0}={1}", para.ParameterName, para.Value);
            }
            return sb.ToString();
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] paras)
        {
            SqlConnection con = DBcon.Conn();
            SqlCommand cmd = new SqlCommand(sql, con);
            if (paras != null && paras.Length > 0)
            {
                cmd.Parameters.AddRange(paras);
            }
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("数据库执行发生错误\r\n{0}", BuildExecuteEorror(sql, paras)), ex);
            }
            finally
            {
                con.Close();
            }
        }

        public static object ExecuteScalar(SqlConnection con, SqlTransaction tran, string sql, params SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Transaction = tran;
            if (paras.Length > 0)
            {
                cmd.Parameters.AddRange(paras);
            }
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(BuildExecuteEorror(sql, paras), ex);
            }
        }
    }
}