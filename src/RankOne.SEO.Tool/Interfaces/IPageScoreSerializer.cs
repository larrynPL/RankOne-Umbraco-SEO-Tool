using RankOne.Models;

namespace RankOne.Interfaces
{
    public interface IPageScoreSerializer
    {
        string Serialize(PageScore score);
    }
}
