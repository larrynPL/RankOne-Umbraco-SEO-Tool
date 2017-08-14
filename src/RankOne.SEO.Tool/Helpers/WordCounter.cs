﻿using RankOne.Collections;
using RankOne.ExtensionMethods;
using RankOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RankOne.Helpers
{
    public class WordCounter
    {
        public int MinimumWordLength { get; set; }

        public WordCounter()
        {
            MinimumWordLength = 4;
        }

        public IEnumerable<KeyValuePair<string, int>> GetKeywords(string text)
        {
            return CountOccurencesForText(text).OrderByDescending(x => x.Value);
        }

        public IEnumerable<KeyValuePair<string, int>> GetKeywords(HtmlResult html)
        {
            var occurences = new WordOccurenceCollection();

            var textBlocks = html.Document.SelectNodes("//*[not(self::script) and not(self::style)]//text()");

            if (textBlocks != null)
            {
                var textBlocksWithText = textBlocks.Where(x => !string.IsNullOrWhiteSpace(x.InnerText)).Select(x => x.InnerHtml);

                foreach (var text in textBlocksWithText)
                {
                    var occurencesInBlock = CountOccurencesForText(text);
                    occurences = occurences.Merge(occurencesInBlock);
                }
            }
            return occurences.OrderByDescending(x => x.Value);
        }

        public WordOccurenceCollection CountOccurencesForText(string textBlockText)
        {
            var occurences = new WordOccurenceCollection();

            // Split text to words
            var words =
                textBlockText.Split(new[] { '.', '?', '!', ' ', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => x.Length >= MinimumWordLength);

            foreach (var word in words)
            {
                var simpleWord = word.Simplify();
                if (!string.IsNullOrWhiteSpace(simpleWord) && simpleWord.Length >= MinimumWordLength)
                {
                    occurences.Add(simpleWord);
                }
            }

            return occurences;
        }
    }
}