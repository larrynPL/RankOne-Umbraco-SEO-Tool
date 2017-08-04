using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.IOC;
using RankOne.Repositories;
using RankOne.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Web;

namespace RankOne.Eventhandlers
{
    public class IOCRegistration : IApplicationEventHandler
    {
        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(ApplicationContext.Current).AsSelf();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Helpers
            builder.RegisterType<PageScoreSerializer>().As<IPageScoreSerializer>();
            
            // Repositories
            builder.RegisterType<NodeReportRepository>().As<INodeReportRepository>();
            builder.RegisterType<AnalysisCacheRepository>().As<IAnalysisCacheRepository>();

            // Service
            builder.RegisterType<AnalyzeService>().As<IAnalyzeService>();

            builder.RegisterType<RankOneContext>();
            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        { }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        { }
    }
}
