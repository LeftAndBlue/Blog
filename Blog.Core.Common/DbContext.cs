using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Common
{
    public class DbContext
    {
        public static SqlSugarClient db = new(
                new ConnectionConfig()
                {
                    ConnectionString = "server = 127.0.0.1; Database = test; Uid = root; Pwd = root; AllowLoadLocalInfile = true;",
                    DbType = SqlSugar.DbType.MySql,//设置数据库类型
                    IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                });

    }
}
