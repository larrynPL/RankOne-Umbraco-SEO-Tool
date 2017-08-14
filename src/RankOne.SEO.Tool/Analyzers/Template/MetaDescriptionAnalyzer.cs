﻿using HtmlAgilityPack;
using RankOne.Attributes;
using RankOne.ExtensionMethods;
using RankOne.Helpers;
using RankOne.Interfaces;
using RankOne.Models;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Analyzers.Template
{
    /// <summary>
    ///
    /// Sources: https://moz.com/learn/seo/meta-description, SEO for 2016 by Sean Odom
    ///
    /// TODO
    /// check for quotes
    ///
    /// </summary>
    [AnalyzerCategory(SummaryName = "Template", Alias = "metadescriptionanalyzer")]
    public class MetaDescriptionAnalyzer : BaseAnalyzer
    {
        private readonly HtmlTagHelper _htmlTagHelper;

        public MetaDescriptionAnalyzer()
        {
            _htmlTagHelper = new HtmlTagHelper();
        }

        public override AnalyzeResult Analyse(IPageData pageData)
        {
            var result = new AnalyzeResult();

            var metaTags = _htmlTagHelper.GetMetaTags(pageData.Document, result);

            if (metaTags.Any())
            {
                AnalyzeMetaTags(metaTags, result);
            }
            return result;
        }

        private void AnalyzeMetaTags(IEnumerable<HtmlNode> metaTags, AnalyzeResult result)
        {
            var attributeValues = from metaTag in metaTags
                                  let attribute = metaTag.GetAttribute("name")
                                  where attribute != null
                                  where attribute.Value == "description"
                                  select metaTag.GetAttribute("content");

            if (!attributeValues.Any())
            {
                result.AddResultRule("no_meta_description_tag", ResultType.Error);
            }
            else if (attributeValues.Count() > 1)
            {
                result.AddResultRule("multiple_meta_description_tags", ResultType.Error);
            }
            else
            {
                var firstMetaDescriptionAttribute = attributeValues.FirstOrDefault();
                if (firstMetaDescriptionAttribute != null)
                {
                    AnalyzeMetaDescriptionAttribute(firstMetaDescriptionAttribute, result);
                }
            }
        }

        private void AnalyzeMetaDescriptionAttribute(HtmlAttribute metaDescriptionAttribute, AnalyzeResult result)
        {
            var descriptionValue = metaDescriptionAttribute.Value;

            if (string.IsNullOrWhiteSpace(descriptionValue))
            {
                result.AddResultRule("no_description_value", ResultType.Error);
            }
            else
            {
                descriptionValue = descriptionValue.Trim();

                if (descriptionValue.Length > 150)
                {
                    result.AddResultRule("description_too_long", ResultType.Warning);
                }

                if (descriptionValue.Length < 20)
                {
                    result.AddResultRule("description_too_short", ResultType.Warning);
                }

                if (descriptionValue.Length < 50)
                {
                    result.AddResultRule("description_too_short", ResultType.Hint);
                }

                if (descriptionValue.Length <= 150 && descriptionValue.Length >= 20)
                {
                    result.AddResultRule("description_perfect", ResultType.Success);
                }
            }
        }
    }
}