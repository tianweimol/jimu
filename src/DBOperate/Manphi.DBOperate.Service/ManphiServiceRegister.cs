using Autofac;
using Jimu.Module;
using Manphi.DbOperate.EF;
using Microsoft.Extensions.Configuration;
using Manphi.DbOperate.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manphi.DBOperate.Service
{
    public class ManphiServiceRegister: ServerModuleBase
    {
        readonly ManphiEFOptions _options;
        IContainer _container = null;
        public ManphiServiceRegister(IConfigurationRoot jimuAppSettings) : base(jimuAppSettings)
        {
            _options = jimuAppSettings.GetSection(typeof(ManphiEFOptions).Name).Get<ManphiEFOptions>();
        }

        public override void DoServiceRegister(ContainerBuilder serviceContainerBuilder)
        {
            //serviceContainerBuilder.RegisterType<BaseDBContext>().As<IBaseDbContext>().InstancePerLifetimeScope();
            //serviceContainerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            //if (_options != null && _options.Enable)
            //{
            //    //DbContextOptions dbContextOptions = new DbContextOptions
            //    //{
            //    //    ConnectionString = _options.ConnectionString,
            //    //    DbType = _options.DbType
            //    //};
            //    Action<LogLevel, string> logAction = null;
            //    if (_options.OpenLogTrace)
            //    {
            //        logAction = (level, log) =>
            //        {
            //            if (_container != null && _container.IsRegistered<Jimu.Logger.ILogger>())
            //            {
            //                Jimu.Logger.ILogger logger = _container.Resolve<Jimu.Logger.ILogger>();
            //                logger.Info($"【EF】 - LogLevel: {level} - {log}");
            //            }
            //        };
            //    }
            //}

            serviceContainerBuilder.RegisterGeneric(typeof(BaseService<>)).As(typeof(IBaseService<>))
                // .WithParameter("option", _dbContextOptions)
                // .WithParameter("dboptions", _dbContextOptions)
                .InstancePerLifetimeScope();
            base.DoServiceRegister(serviceContainerBuilder);
        }

        public override void DoInit(IContainer container)
        {
            _container = container;
            base.DoServiceInit(container);
        }
    }

}
