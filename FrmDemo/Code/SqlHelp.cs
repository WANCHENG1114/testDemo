using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FrmDemo.Code
{
    /// <summary>
    /// sql辅助类
    /// </summary>
    public class SqlHelp
    {
        /*<appSettings>
    <add key="connStr" value=" Server=.;Database=TestDemo;User ID=sa; Password=qwe123$$"/>
  </appSettings>
         */

        //public static string connstring = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        public static string connstring = ConfigurationManager.AppSettings["connStr"];
        //public static string connstring = "Data Source=ZZ-PC;Initial Catalog=IPTVDB;User ID=sa;Password=sa";  
        /// <summary>  
        /// 执行非查询，返回受影响行数,异常返回-1；  
        /// </summary>  
        /// <param name="sql"></param>  
        /// <param name="type"></param>  
        /// <param name="pars"></param>  
        /// <returns></returns>  
        public static bool ExceNonQuery(string sql, CommandType type, IDataParameter[] pars)
        {

            SqlConnection con = new SqlConnection(connstring);
            SqlCommand com = new SqlCommand(sql, con);

            if (pars != null && pars.Length > 0)
            {
                foreach (SqlParameter pp in pars)//把参数集全部加进去  
                    com.Parameters.Add(pp); 
            }
            try
            {
                con.Open();
                int t = com.ExecuteNonQuery();
                if (t > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e) { return false; }
            finally
            {
                com.Parameters.Clear();
                com.Dispose();
                con.Close();
            }
        }
        /// <summary>  
        /// 执行sql语句的查询，返回查询的数量。异常返回-1.  
        /// </summary>  
        /// <param name="sql"></param>  
        /// <param name="type"></param>  
        /// <param name="pars"></param>  
        /// <returns></returns>  
        public static int ExceQuery(string sql, CommandType type, IDataParameter[] pars)
        {
            SqlConnection con = new SqlConnection(connstring);
            SqlCommand com = new SqlCommand(sql, con);
            com.CommandType = type;
            if (pars != null && pars.Length > 0)
            {
                foreach (SqlParameter pp in pars)//把参数集全部加进去  
                    com.Parameters.Add(pp);
            }
            try
            {
                con.Open();
                if (com.ExecuteScalar() != null)//查询结果为空时返回0  
                {
                    int t = (int)com.ExecuteScalar();

                    return t;
                }
                else
                    return -1;
            }
            catch (Exception e) { return -1; }
            finally
            {
                com.Parameters.Clear();
                com.Dispose();
                con.Close();
            }
        }
        /// <summary>  
        /// 执行查询，返回一个数据集  
        /// </summary>  
        /// <param name="sql"></param>  
        /// <param name="pars"></param>  
        /// <returns></returns>  
        public static DataSet ExcueReturnDataset(string sql, IDataParameter[] pars)
        {
            SqlConnection con = new SqlConnection(connstring);
            DataSet set = new DataSet();
            SqlCommand com = new SqlCommand(sql, con);
            if (pars != null && pars.Length > 0)
            {
                foreach (SqlParameter pp in pars)//把参数集全部加进去  
                    com.Parameters.Add(pp);
            }

            SqlDataAdapter adpter = new SqlDataAdapter(com);

            try
            {
                set.Clear();
                adpter.Fill(set);
                return set;
            }
            catch (Exception ex) { return null; }
            finally
            {
                com.Parameters.Clear();
                com.Dispose();
                con.Close();
            }

        }
        //        public static DataSet ExcueReturnDataset(string sql, con);
        //        com.CommandType = type;  
        //                if (pars != null && pars.Length > 0)  
        //                {  
        //                    foreach (SqlParameter pp in pars)//把参数集全部加进去  
        //                        com.Parameters.Add(pp);  
        //                }

        //    SqlDataAdapter adpter = new SqlDataAdapter(com);  

        //                try  
        //                {  
        //                    set.Clear();  
        //                    adpter.Fill(set);  
        //                    return set;  
        //                }  
        //                catch (Exception ex) { return null; }  
        //                finally  
        //                {  
        //                    com.Parameters.Clear();  
        //                    com.Dispose();  
        //                    con.Close();  
        //                }  

        //            }  
        //            public static IDataReader ExcueReturnDataReader(string sql, con);

        //SqlDataReader reader;  
        //                if (pars != null && pars.Length > 0)  
        //                {  
        //                    foreach (SqlParameter pp in pars)//把参数集全部加进去  
        //                        com.Parameters.Add(pp);  
        //                }  
        //                try  
        //                {  
        //                    con.Open();  
        //                    reader = com.ExecuteReader(CommandBehavior.CloseConnection);  
        //                    return reader;  
        //                }  
        //                catch (Exception ex)  
        //                {   

        //                    return null;   
        //                }  
        //                finally {  
        //                    com.Parameters.Clear();  
        //                    com.Dispose();  
        //                    //con.Close();  
        //                }  

        //            }  
        /// <summary>  
        /// 执行存储过程，返回影响的行数        
        /// </summary>  
        /// <param name="storedProcName">存储过程名</param>  
        /// <param name="parameters">存储过程参数</param>  
        /// <param name="rowsAffected">影响的行数</param>  
        /// <returns></returns>  
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (SqlConnection connection = new SqlConnection(connstring))
            {
                int result;
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcName, connection);
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null && parameters.Length > 0)
                {
                    foreach (SqlParameter pp in parameters)//把参数集全部加进去  
                        command.Parameters.Add(pp);
                }
                command.Parameters.Add("@return", "").Direction = ParameterDirection.ReturnValue;
                rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["@return"].Value;
                connection.Close();
                return result;
            }
        }
        /// <summary>  
        /// 执行存储过程  
        /// </summary>  
        /// <param name="storedProcName">存储过程名</param>  
        /// <param name="parameters">存储过程参数</param>  
        /// <param name="tableName">DataSet结果中的表名</param>  
        /// <returns>DataSet</returns>  
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connstring))
            {
                DataSet dataSet = new DataSet();
                SqlCommand com = new SqlCommand(storedProcName, connection);
                com.CommandType = CommandType.StoredProcedure;
                if (parameters != null && parameters.Length > 0)
                {
                    foreach (SqlParameter pp in parameters)//把参数集全部加进去  
                        com.Parameters.Add(pp);
                }

                SqlDataAdapter adpter = new SqlDataAdapter(com);
                adpter.Fill(dataSet, tableName);
                return dataSet;
            }
        }
        /// <summary>  
        /// 执行查询语句，返回DataSet  
        /// </summary>  
        /// <param name="SQLString">查询语句</param>  
        /// <returns>DataSet</returns>  
        public DataSet Query(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connstring))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        
        /// <summary>
        /// 查
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <returns>返回DataTable</returns>
        public DataTable ExecuteQuery(string sqlStr)      
        {
            SqlConnection con = new SqlConnection(@connstring);
            try
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(sqlStr, con); 
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
            finally
            {
                con.Close();
            }
           
        }
        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <returns>返回受影响行数</returns>
        public int ExecuteUpdate(string sqlStr)      //用于增删改;
        {
            SqlConnection con = new SqlConnection(@connstring);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sqlStr, con);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
                throw ex;
            }
            finally
            { con.Close(); }
        }
    }
}

