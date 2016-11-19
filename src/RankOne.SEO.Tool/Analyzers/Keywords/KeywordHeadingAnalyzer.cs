﻿using System.Linq;
using RankOne.Attributes;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;

namespace RankOne.Analyzers.Keywords
{
    [AnalyzerCategory(SummaryName = "Keywords", Alias = "keywordheadinganalyzer")]
    public class KeywordHeadingAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            var h1Tags = pageData.Document.GetDescendingElements("h1");
            var h2Tags = pageData.Document.GetDescendingElements("h2");
            var h3Tags = pageData.Document.GetDescendingElements("h3");
            var h4Tags = pageData.Document.GetDescendingElements("h4");

            var usedInHeadingCount = h1Tags.Count(x => x.InnerText.ToLower().Contains(pageData.Focuskeyword)) + 
                h2Tags.Count(x => x.InnerText.ToLower().Contains(pageData.Focuskeyword)) + 
                h3Tags.Count(x => x.InnerText.ToLower().Contains(pageData.Focuskeyword)) + 
                h4Tags.Count(x => x.InnerText.ToLower().Contains(pageData.Focuskeyword));

            if (usedInHeadingCount > 0)
            {
                var resultRule = new ResultRule
                {
                    Alias = "keyword_used_in_heading",
                    Type = ResultType.Success
                };
                resultRule.Tokens.Add(usedInHeadingCount.ToString());
                result.ResultRules.Add(resultRule);
            }
            else
            {
                result.AddResultRule("keyword_not_used_in_heading", ResultType.Hint);
            }

            return result;
        }
    }
}
