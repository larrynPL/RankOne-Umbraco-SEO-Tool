using RankOne.Interfaces;
using RankOne.Models;
using System.Collections.Generic;
using System.Web.Http;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RankOne.Controllers
{
    [PluginController("RankOne")]
    public class DashboardApiController : UmbracoAuthorizedApiController
    {
        private readonly IDashboardDataService _dashboardDataService;

        public DashboardApiController(IDashboardDataService dashboardService)
        {
            _dashboardDataService = dashboardService;
        }

        [HttpGet]
        public IEnumerable<PageScoreNode> Initialize()
        {
            _dashboardDataService.Initialize();
            return _dashboardDataService.GetHierarchy();
        }

        [HttpGet]
        public IEnumerable<PageScoreNode> GetPageHierarchy()
        {
            return _dashboardDataService.GetHierarchy();
        }

        [HttpGet]
        public IEnumerable<PageScoreNode> UpdateAllPages()
        {
            return _dashboardDataService.GetHierarchy(false);
        }
    }
}
