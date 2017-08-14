﻿using HtmlAgilityPack;
using RankOne.Attributes;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System.Linq;

namespace RankOne.Analyzers.Template
{
    [AnalyzerCategory(SummaryName = "Template", Alias = "deprecatedtaganalyzer")]
    public class DeprecatedTagAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            CheckTag(pageData.Document, "acronym", result);
            CheckTag(pageData.Document, "applet", result);
            CheckTag(pageData.Document, "basefont", result);
            CheckTag(pageData.Document, "big", result);
            CheckTag(pageData.Document, "center", result);
            CheckTag(pageData.Document, "dir", result);
            CheckTag(pageData.Document, "font", result);
            CheckTag(pageData.Document, "frame", result);
            CheckTag(pageData.Document, "frameset", result);
            CheckTag(pageData.Document, "noframes", result);
            CheckTag(pageData.Document, "strike", result);
            CheckTag(pageData.Document, "tt", result);

            if (!result.ResultRules.Any())
            {
                result.AddResultRule("no_deprecated_tags_found", ResultType.Success);
            }

            return result;
        }

        private void CheckTag(HtmlNode htmlNode, string tagname, AnalyzeResult result)
        {
            var acronymTags = htmlNode.GetElements(tagname);

            if (acronymTags.Any())
            {
                result.AddResultRule(string.Format("{0}_tag_found", tagname), ResultType.Warning);
            }
        }
    }
}