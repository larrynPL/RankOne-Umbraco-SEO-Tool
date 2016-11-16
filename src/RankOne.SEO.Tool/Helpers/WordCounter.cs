﻿using System;
using System.Collections.Generic;
using System.Linq;
using RankOne.Collections;
using RankOne.ExtensionMethods;
using RankOne.Models;

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

            var textBlocks = html.Document.SelectNodes("//text()");

            if (textBlocks != null)
            {
                foreach (var textBlock in textBlocks)
                {
                    var textBlockText = textBlock.InnerText;
                    var occurencesInBlock = CountOccurencesForText(textBlockText);
                    occurences.Merge(occurencesInBlock);
                }
            }
            return occurences.OrderByDescending(x => x.Value);
        }

        public WordOccurenceCollection CountOccurencesForText(string textBlockText)
        {
            var occurences = new WordOccurenceCollection();

            // Split text to words
            var words =
                textBlockText.Split(new[] {'.', '?', '!', ' ', ':', ','}, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => x.Length >= MinimumWordLength);

            foreach (var word in words)
            {
                var simpleWord = word.Simplify();
                occurences.Add(simpleWord);
            }

            return occurences;
        }
    }
}
