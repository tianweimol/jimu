namespace Manphi.DbOperate.EF
{
    /*
     {
   "MiniDDDEFOptions": {
     "ConnectionString": "server=localhost;database=grissom_dev;user=root;pwd=root;",
     "DbType": "MySql", //数据库类型，支持： MySQL,SQLServer,Oracle,SQLite, PostgreSQL
     "TableModelAssemblyName": "",//EF对应的表的实体类dll, Server 项目引用了则不需要设置
     "OpenLogTrace":false, //是否开启 sql 日志,一般 debug 时开启方面查看生成的 sql
     "LogLevel":"Debug" //日志级别： Debug,Information,Warning,Error
   }
}
         */
    public class ManphiEFOptions
    {
        public bool Enable { get; set; } = true;
        public string ConnectionString { get; set; }
        /// <summary>
        /// 数据库类型，支持： MySQL,SQLServer,Oracle,SQLite, PostgreSQL
        /// </summary>
        public DbType DbType { get; set; }

        /// <summary>
        /// EF对应的表的实体类dll, Server 项目引用了则不需要设置
        /// </summary>
        public string TableModelAssemblyName { get; set; }

        /// <summary>
        /// 是否开启 sql 日志,一般 debug 时开启方面查看生成的 sql
        /// </summary>
        public bool OpenLogTrace { get; set; }

        /// <summary>
        /// log level, default debug 
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Debug;
    }
}
