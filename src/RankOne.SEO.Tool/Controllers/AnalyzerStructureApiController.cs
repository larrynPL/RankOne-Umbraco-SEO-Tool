using System.Collections.Generic;
using System.Linq;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Summaries;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RankOne.Controllers
{
    [PluginController("RankOne")]
    public class AnalyzerStructureApiController : UmbracoAuthorizedApiController
    {
        private readonly IDefinitionHelper _defintionHelper;

        public AnalyzerStructureApiController(IDefinitionHelper definitionHelper)
        {
            _defintionHelper = definitionHelper;
        }

        public IEnumerable<AnalyzerStructure> GetStructure()
        {
            var summaries = _defintionHelper.GetSummaryDefinitions();
            var analyzers = _defintionHelper.GetAnalyzerDefintions();

            var structure = new List<AnalyzerStructure>();
            foreach (var summary in summaries)
            {
                var summaryInstance = summary.Type.GetInstance<BaseSummary>();
                var analyzersForSummary = analyzers.Where(
                    x => x.AnalyzerCategory.SummaryName == summaryInstance.Name)
                    .Select(x => x.AnalyzerCategory.Alias);

                structure.Add(new AnalyzerStructure
                {
                    Name = summary.Summary.Alias,
                    Analyzers = analyzersForSummary
                });

            }
            return structure;
        }
    }
}
