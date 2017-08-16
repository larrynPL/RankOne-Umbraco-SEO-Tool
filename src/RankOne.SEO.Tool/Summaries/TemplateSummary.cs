﻿using RankOne.Attributes;

namespace RankOne.Summaries
{
    [Summary(Alias = "templateanalyzer", SortOrder = 1)]
    public class TemplateSummary : BaseSummary
    {
        public TemplateSummary()
        {
            Name = "Template";
        }
    }
}