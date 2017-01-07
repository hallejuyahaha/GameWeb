using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Common
{
    public class DBcon
    {
        #region 连接
        public static SqlConnection Conn()
        {
            string GetUrl = Convert.ToString(HttpContext.Current.Request.Url).ToLower();
            string constring = GetConnectionString();
            SqlConnection con = new SqlConnection(constring);
            return con;
        }
        #endregion

        #region 属性
        protected static SqlConnection conn = new SqlConnection();
        protected static SqlCommand cmd = new SqlCommand();
        #endregion

        #region 内部函数 静态方法中不会执行dataaccess()构造函数

        ///获取连接字符串内容
        /// 
        public static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["ConnectionString"].ToString();
        }
        public SqlConnection createCon()
        {
            SqlConnection conn = new SqlConnection(GetConnectionString());

            return conn;
        }

        public string GetKeyWords(string strWords)
        {
            return ConfigurationManager.AppSettings[strWords].ToString();
        }
        /// <summary> 
        /// 打开数据库连接 
        /// </summary> 
        public static void openConnection()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = GetConnectionString();
                cmd.Connection = conn; ;
                try
                {
                    if (conn.State.Equals(ConnectionState.Closed))
                    {
                        conn.Open();
                    }
                    else
                    {
                        conn.Close();
                        conn.Dispose();
                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
        /// <summary> 
        /// 关闭当前数据库连接 
        /// </summary> 
        public static void closeConnection()
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Dispose();
        }
        #endregion

        #region   传入参数并且转换为SqlParameter类型
        /// <summary>
        /// 转换参数
        /// </summary>
        /// <param name="ParamName">存储过程名称或命令文本</param>
        /// <param name="DbType">参数类型</param></param>
        /// <param name="Size">参数大小</param>
        /// <param name="Value">参数值</param>
        /// <returns>新的 parameter 对象</returns>
        public static SqlParameter MakeInParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        /// <summary>
        /// 初始化参数值
        /// </summary>
        /// <param name="ParamName">存储过程名称或命令文本</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小</param>
        /// <param name="Direction">参数方向</param>
        /// <param name="Value">参数值</param>
        /// <returns>新的 parameter 对象</returns>
        public static SqlParameter MakeParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            SqlParameter param;

            if (Size > 0)
                param = new SqlParameter(ParamName, DbType, Size);
            else
                param = new SqlParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;
            return param;
        }
        #endregion

        #region   执行参数命令文本(无数据库中数据返回)
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="procName">命令文本</param>
        /// <param name="prams">参数对象</param>
        /// <returns></returns>
        public static int RunProc(string procName, SqlParameter[] prams)
        {
            SqlCommand cmd = CreateCommand(procName, prams);
            cmd.ExecuteNonQuery();
            closeConnection();
            //得到执行成功返回值
            return (int)cmd.Parameters["ReturnValue"].Value;
        }
        /// <summary>
        /// 直接执行SQL语句
        /// </summary>
        /// <param name="procName">命令文本</param>
        /// <returns></returns>
        public static int RunProc(string procName)
        {
            conn = DBcon.Conn();
            conn.Open();
            SqlCommand cmd = new SqlCommand(procName, conn);
            cmd.ExecuteNonQuery();
            closeConnection();
            return 1;
        }

        #endregion

        #region   执行参数命令文本(有返回值)
        /// <summary>
        /// 执行查询命令文本，并且返回DataSet数据集
        /// </summary>
        /// <param name="procName">命令文本</param>
        /// <param name="prams">参数对象</param>
        /// <param name="tbName">数据表名称</param>
        /// <returns></returns>
        public static DataSet RunProcReturn(string procName, SqlParameter[] prams, string tbName)
        {
            SqlDataAdapter dap = CreateDataAdaper(procName, prams);
            DataSet ds = new DataSet();
            dap.Fill(ds, tbName);

            //得到执行成功返回值
            return ds;
            closeConnection();
        }

        /// <summary>
        /// 执行命令文本，并且返回DataSet数据集
        /// </summary>
        /// <param name="procName">命令文本</param>
        /// <param name="tbName">数据表名称</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcReturn(string procName, string tbName)
        {
            SqlDataAdapter dap = CreateDataAdaper(procName, null);
            DataSet ds = new DataSet();
            dap.Fill(ds, tbName);
            closeConnection();
            //得到执行成功返回值
            return ds;
        }

        #endregion

        #region 将命令文本添加到SqlDataAdapter
        /// <summary>
        /// 创建一个SqlDataAdapter对象以此来执行命令文本
        /// </summary>
        /// <param name="procName">命令文本</param>
        /// <param name="prams">参数对象</param>
        /// <returns></returns>
        private static SqlDataAdapter CreateDataAdaper(string procName, SqlParameter[] prams)
        {
            conn = DBcon.Conn();
            conn.Open();
            SqlDataAdapter dap = new SqlDataAdapter(procName, conn);
            dap.SelectCommand.CommandType = CommandType.Text;  //执行类型：命令文本
            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                    dap.SelectCommand.Parameters.Add(parameter);
            }
            //加入返回参数
            dap.SelectCommand.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4,
                ParameterDirection.ReturnValue, false, 0, 0,
                string.Empty, DataRowVersion.Default, null));

            return dap;
            closeConnection();
        }
        #endregion

        #region   将命令文本添加到SqlCommand
        /// <summary>
        /// 创建一个SqlCommand对象以此来执行命令文本
        /// </summary>
        /// <param name="procName">命令文本</param>
        /// <param name="prams"命令文本所需参数</param>
        /// <returns>返回SqlCommand对象</returns>
        private static SqlCommand CreateCommand(string procName, SqlParameter[] prams)
        {
            // 确认打开连接
            conn = DBcon.Conn();
            conn.Open();
            SqlCommand cmd = new SqlCommand(procName, conn);
            cmd.CommandType = CommandType.Text;　　　　 //执行类型：命令文本

            // 依次把参数传入命令文本
            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                    cmd.Parameters.Add(parameter);
            }
            // 加入返回参数
            cmd.Parameters.Add(
                new SqlParameter("ReturnValue", SqlDbType.Int, 4,
                ParameterDirection.ReturnValue, false, 0, 0,
                string.Empty, DataRowVersion.Default, null));

            return cmd;
            closeConnection();
        }
        #endregion
    }
}