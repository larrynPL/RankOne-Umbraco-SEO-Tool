﻿using System.Collections.Generic;

namespace RankOne.Models
{
    public class SeoNode
    {
        public string Url { get; set; }

        public List<SeoNode> Children { get; set; }

        public SeoNode()
        {
            Children = new List<SeoNode>();
        }
    }
}