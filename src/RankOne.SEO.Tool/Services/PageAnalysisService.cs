using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using RankOne.Summaries;
using System.Net;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RankOne.Services
{
    public class PageAnalysisService : IPageAnalysisService
    {       
        private readonly IScoreService _scoreService;
        private readonly IDefinitionHelper _definitionHelper;
        private readonly IHtmlHelper _htmlHelper;
        private readonly IByteSizeHelper _byteSizeHelper;

        public PageAnalysisService(IScoreService scoreService, IDefinitionHelper definitionHelper, IHtmlHelper htmlHelper, IByteSizeHelper byteSizeHelper)
        {
            _scoreService = scoreService;
            _definitionHelper = definitionHelper;
            _htmlHelper = htmlHelper;     
            _byteSizeHelper = byteSizeHelper;     
        }

        public PageAnalysis CreatePageAnalysis(IPublishedContent node, string focusKeyword)
        {
            var pageAnalysis = new PageAnalysis();

            try
            {
                var htmlString = _htmlHelper.GetTemplateHtml(node);
                var htmlResult = _htmlHelper.GetHtmlResult(htmlString);

                pageAnalysis.Url = node.UrlAbsolute();
                pageAnalysis.FocusKeyword = focusKeyword;
                pageAnalysis.Size = _byteSizeHelper.GetByteSize(htmlString);

                SetAnalyzerResults(pageAnalysis, htmlResult);
            }
            catch (WebException ex)
            {
                pageAnalysis.Status = ((HttpWebResponse)ex.Response).StatusCode;
            }

            pageAnalysis.Score = _scoreService.GetScore(pageAnalysis);

            return pageAnalysis;
        }

        private void SetAnalyzerResults(PageAnalysis pageAnalysis, HtmlResult html)
        {
            // Get all types marked with the Summary attribute
            var summaryDefinitions = _definitionHelper.GetSummaryDefinitions();

            // Instantiate the types and retrieve te results
            foreach (var summaryDefinition in summaryDefinitions)
            {
                var summary = summaryDefinition.Type.GetInstance<BaseSummary>();
                summary.FocusKeyword = pageAnalysis.FocusKeyword;
                summary.HtmlResult = html;
                summary.Url = pageAnalysis.Url;

                var analyzerResult = new AnalyzerResult
                {
                    Alias = summaryDefinition.Summary.Alias,
                    Analysis = summary.GetAnalysis()
                };

                pageAnalysis.AnalyzerResults.Add(analyzerResult);
            }
        }
    }
}
