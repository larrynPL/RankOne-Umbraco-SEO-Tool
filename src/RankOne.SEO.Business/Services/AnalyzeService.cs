﻿using System.Diagnostics;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using RankOne.Business.Models;
using RankOne.Business.Summaries;

namespace RankOne.Business.Services
{
    public class AnalyzeService
    {
        private readonly HtmlDocument _htmlParser;

        public AnalyzeService()
        {
            _htmlParser = new HtmlDocument();
        }

        public PageAnalysis AnalyzeWebPage(string url)
        {
            var webpage = new PageAnalysis
            {
                Url = url,
            };

            try
            {
                webpage.HtmlResult = GetHtml(url);

                var htmlAnalyzer = new HtmlSummary(webpage.HtmlResult);
                webpage.AnalyzerResults.Add(new AnalyzerResult
                {
                    Alias = "htmlanalyzer",
                    Title = "htmlanalyzer_title",
                    Analysis = htmlAnalyzer.GetAnalysis()
                });

                var keywordAnalyzer = new KeywordSummary(webpage.HtmlResult);
                webpage.AnalyzerResults.Add(new AnalyzerResult
                {
                    Alias = "keywordanalyzer",
                    Title = "keywordanalyzer_title",
                    Analysis = keywordAnalyzer.GetAnalysis()
                });

                var speedAnalyzer = new SpeedSummary(webpage.HtmlResult);
                webpage.AnalyzerResults.Add(new AnalyzerResult
                {
                    Alias = "speedanalyzer",
                    Title = "speedanalyzer_title",
                    Analysis = speedAnalyzer.GetAnalysis()
                });
            }
            catch (WebException ex)
            {
                webpage.Status = ((HttpWebResponse)ex.Response).StatusCode;
            }
            return webpage;
        }

        private HtmlResult GetHtml(string url)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var html = new WebClient().DownloadString(url);

            stopwatch.Stop();

            _htmlParser.LoadHtml(html);
            var xDocument = _htmlParser.DocumentNode;

            return new HtmlResult
            {
                Url = url,
                Html = html,
                Size = Encoding.ASCII.GetByteCount(html),
                ServerResponseTime = stopwatch.ElapsedMilliseconds,
                Document = xDocument
            };
        }
    }
}
