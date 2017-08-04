using RankOne.Interfaces;
using RankOne.Models;
using System;

namespace RankOne.Repositories
{
    public class AnalysisCacheRepository : IAnalysisCacheRepository
    {
        private readonly INodeReportRepository _nodeReportRepository;
        private readonly IPageScoreSerializer _javaScriptSerializer;

        public AnalysisCacheRepository(INodeReportRepository nodeReportRepository, IPageScoreSerializer pageScoreSerializer)
        {
            _nodeReportRepository = nodeReportRepository;
            _javaScriptSerializer = pageScoreSerializer;
        }

        public void Save(int id, PageAnalysis pageAnalysis)
        {
            var scoreReport = _javaScriptSerializer.Serialize(pageAnalysis.Score);

            var nodeReport = new NodeReport
            {
                Id = id,
                FocusKeyword = pageAnalysis.FocusKeyword,
                Report = scoreReport,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            CreateOrUpdateNodeReport(nodeReport);
        }

        private void CreateOrUpdateNodeReport(NodeReport nodeReport)
        {
            var dbNodeReport = _nodeReportRepository.GetById(nodeReport.Id);
            if (dbNodeReport == null)
            {
                _nodeReportRepository.Insert(nodeReport);
            }
            else
            {
                dbNodeReport.FocusKeyword = nodeReport.FocusKeyword;
                dbNodeReport.Report = nodeReport.Report;
                dbNodeReport.UpdatedOn = nodeReport.UpdatedOn;

                _nodeReportRepository.Update(nodeReport);
            }
        }
    }
}
