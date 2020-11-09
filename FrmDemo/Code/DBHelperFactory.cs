using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrmDemo.Code
{
    public static class DBHelperFactory
    {
        //命名空间：System.Configuration
        // 程序集： System.Configuration.dll

        /*<connectionStrings>
    <add name="myconn" connectionString="Data Source=.;Initial Catalog=TestDB;
         Persist Security Info=True;User ID=sa;Password=123456"/>
  </connectionStrings>
         */
        /// <summary>
        /// 工厂类
        /// 从配置文件中读取数据库类型和连接字符串，通过反射构造数据库辅助类对象
        /// </summary>
        /// <returns></returns>
        public static DBHelper CreateDBhelper()
        {
            var dbType = ConfigurationManager.AppSettings["database"];
            var connectionString = ConfigurationManager.ConnectionStrings[dbType].ConnectionString;
            try
            {
                DBHelper dbHelper = Activator.CreateInstance(Type.GetType(dbType), new object[] { connectionString }) as DBHelper;
                return dbHelper;
            }
            catch (Exception ex)
            {
                throw new Exception("数据库类型有误，请修改配置文件的数据库类型", ex);
            }
        }
    }
}
