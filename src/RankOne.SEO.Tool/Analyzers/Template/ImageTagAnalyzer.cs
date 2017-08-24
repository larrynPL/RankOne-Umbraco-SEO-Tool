﻿using HtmlAgilityPack;
using RankOne.ExtensionMethods;
using RankOne.Interfaces;
using RankOne.Models;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Analyzers.Template
{
    public class ImageTagAnalyzer : BaseAnalyzer
    {
        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult() { Weight = Weight };

            var imageTags = pageData.Document.GetElements("img");
            var imageTagCount = imageTags.Count();

            CheckImagesForAttribute(imageTags, imageTagCount, result, "alt");
            CheckImagesForAttribute(imageTags, imageTagCount, result, "title");

            if (!result.ResultRules.Any())
            {
                result.AddResultRule("alt_and_title_tags_present", ResultType.Success);
            }

            return result;
        }

        private void CheckImagesForAttribute(IEnumerable<HtmlNode> imageTags, int imageTagCount, AnalyzeResult result, string attributeName)
        {
            var imagesWithAttributeCount =
                imageTags.Count(x => x.GetAttribute(attributeName) != null && !string.IsNullOrWhiteSpace(x.GetAttribute(attributeName).Value));

            if (imageTagCount > imagesWithAttributeCount)
            {
                var resultRule = new ResultRule
                {
                    Alias = "missing_" + attributeName + "_tags",
                    Type = ResultType.Hint
                };
                var numberOfTagsMissingAttribute = imageTagCount - imagesWithAttributeCount;
                resultRule.Tokens.Add(numberOfTagsMissingAttribute.ToString());
                result.ResultRules.Add(resultRule);
            }
        }
    }
}