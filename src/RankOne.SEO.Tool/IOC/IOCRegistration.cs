using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Practices.ServiceLocation;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.IOC;
using RankOne.Repositories;
using RankOne.Serializers;
using RankOne.Services;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Web;

namespace RankOne.Eventhandlers
{
    public class IOCRegistration : IApplicationEventHandler
    {
        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        { }

        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        { }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(ApplicationContext.Current).AsSelf();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Helpers
            builder.RegisterType<ByteSizeHelper>().As<IByteSizeHelper>();
            builder.RegisterType<EncodingHelper>().As<IEncodingHelper>();
            builder.RegisterType<FocusKeywordHelper>().As<IFocusKeywordHelper>();
            builder.RegisterType<Helpers.HtmlHelper>().As<IHtmlHelper>();
            builder.RegisterType<HtmlTagHelper>().As<IHtmlTagHelper>();
            builder.RegisterType<MinificationHelper>().As<IMinificationHelper>();
            builder.RegisterType<NodeReportTableHelper>().As<INodeReportTableHelper>();
            builder.RegisterType<PageScoreNodeHelper>().As<IPageScoreNodeHelper>();
            builder.RegisterType<TemplateHelper>().As<ITemplateHelper>();
            builder.RegisterType<WordCounter>().As<IWordCounter>();

            // Serializers
            builder.RegisterType<DashboardSettingsSerializer>().As<IDashboardSettingsSerializer>();
            builder.RegisterType<PageScoreSerializer>().As<IPageScoreSerializer>();

            // Repositories
            builder.RegisterType<NodeReportRepository>().As<INodeReportRepository>();
            builder.RegisterType<AnalysisCacheRepository>().As<IAnalysisCacheRepository>();

            // Service
            builder.RegisterType<AnalyzeService>().As<IAnalyzeService>();
            builder.RegisterType<PageAnalysisService>().As<IPageAnalysisService>();
            builder.RegisterType<PageInformationService>().As<IPageInformationService>();
            builder.RegisterType<DashboardDataService>().As<IDashboardDataService>();
            builder.RegisterType<ScoreService>().As<IScoreService>();

            // Umbraco
            var umbracoContext = UmbracoContext.Current;
            var umbracoHelper = new UmbracoHelper(umbracoContext);
            builder.RegisterInstance(umbracoHelper.ContentQuery).As<ITypedPublishedContentQuery>();
            builder.RegisterInstance(umbracoHelper.UmbracoComponentRenderer).As<IUmbracoComponentRenderer>();

            builder.RegisterControllers(typeof(UmbracoApplication).Assembly);
            builder.RegisterApiControllers(typeof(UmbracoApplication).Assembly);

            builder.RegisterType<RankOneContext>();
            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
        }
    }
}
