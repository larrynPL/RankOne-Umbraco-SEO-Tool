﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace RankOne.Interfaces
{
    public interface IFocusKeywordHelper
    {
        string GetFocusKeyword(IPublishedContent node);
    }
}
