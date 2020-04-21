using Autofac;
using Jimu.Module;
using Jimu.Server.ORM.Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mol.DBOperate.EF
{
    public class MolEFServerModule : ServerModuleBase
    {
        readonly MolEFOptions _options;
        //private readonly DbContextOptions _dbContextOptions;
        private readonly DapperOptions _dapperOptions;
        IContainer _container = null;
        public MolEFServerModule(IConfigurationRoot jimuAppSettings) : base(jimuAppSettings)
        {
            _options = jimuAppSettings.GetSection(typeof(MolEFOptions).Name).Get<MolEFOptions>();
            //_dbContextOptions = jimuAppSettings.GetSection(typeof(DbContextOptions).Name).Get<DbContextOptions>();
            _dapperOptions = jimuAppSettings.GetSection(typeof(DapperOptions).Name).Get<DapperOptions>();
        }

        public override void DoServiceRegister(ContainerBuilder serviceContainerBuilder)
        {
            serviceContainerBuilder.RegisterGeneric(typeof(BaseContext<>)).As(typeof(IBaseContext<>))
                // .WithParameter("option", _dbContextOptions)
                // .WithParameter("dboptions", _dbContextOptions)
                .InstancePerLifetimeScope();

            if (_options != null && _options.Enable)
            {
                //DbContextOptions dbContextOptions = new DbContextOptions
                //{
                //    ConnectionString = _options.ConnectionString,
                //    DbType = _options.DbType
                //};
                Action<LogLevel, string> logAction = null;
                if (_options.OpenLogTrace)
                {
                    logAction = (level, log) =>
                    {
                        if (_container != null && _container.IsRegistered<Jimu.Logger.ILogger>())
                        {
                            Jimu.Logger.ILogger logger = _container.Resolve<Jimu.Logger.ILogger>();
                            logger.Info($"【EF】 - LogLevel: {level} - {log}");
                        }
                    };
                }
            }
            base.DoServiceRegister(serviceContainerBuilder);
        }

        public override void DoInit(IContainer container)
        {
            _container = container;
            base.DoServiceInit(container);
        }
    }
}
