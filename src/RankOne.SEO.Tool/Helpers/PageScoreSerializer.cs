using RankOne.Interfaces;
using RankOne.Models;
using System.Web.Script.Serialization;

namespace RankOne.Helpers
{
    public class PageScoreSerializer : JavaScriptSerializer, IPageScoreSerializer
    {
        public string Serialize(PageScore score)
        {
            return Serialize(score);
        }
    }
}
