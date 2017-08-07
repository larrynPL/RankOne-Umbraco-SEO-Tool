using HtmlAgilityPack;
using RankOne.Models;
using Umbraco.Core.Models;
using RankOne.Interfaces;

namespace RankOne.Helpers
{
    public class HtmlHelper : IHtmlHelper
    {
        private readonly ITemplateHelper _contentHelper;

        public HtmlHelper(ITemplateHelper templateHelper)
        {
            _contentHelper = templateHelper;
        }

        public HtmlNode GetHtmlNodeFromString(string htmlString)
        {
            if (htmlString != null)
            {
                var document = new HtmlDocument();
                document.LoadHtml(htmlString);
                return document.DocumentNode;
            }
            return null;
        }

        public HtmlResult GetHtmlResult(string htmlString)
        {
            var htmlNode = GetHtmlNodeFromString(htmlString);
            var htmlResult = new HtmlResult
            {
                Html = htmlString,
                Document = htmlNode
            };
            return htmlResult;
        }

        public string GetTemplateHtml(IPublishedContent node)
        {
            return _contentHelper.GetNodeHtml(node);
        }
    }
}
