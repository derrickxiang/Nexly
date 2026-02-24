using Nexly.Pipeline.News.AI;
using Nexly.Pipeline.News.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.News.Steps
{
    public class AIEnrichNewsStep
    {
        private readonly IAIClient _ai;

        public AIEnrichNewsStep(IAIClient ai)
        {
            _ai = ai;
        }

        public async Task<AIEnrichedNews> ExecuteAsync(NormalizedNews news)
        {
            var prompt = BuildPrompt(news.Content);

            var result = await _ai.GenerateJsonAsync<Nexly.Pipeline.News.AI.AIResult>(prompt);

            return new AIEnrichedNews
            {
                Title = news.Title,
                Url = news.Url,
                Source = news.Source,
                PublishedAt = news.PublishedAt,
                Content = news.Content,
                Hash = news.Hash,

                Summary = result.Summary,
                Category = result.Category,
                ImportanceScore = result.ImportanceScore,
                Companies = result.Companies,
                Tags = result.Tags
            };
        }

        private string BuildPrompt(string content)
        {
            return $"""
你是一名半导体行业专家，请分析新闻：

{content}

输出JSON：
summary
category
importance_score (1-10)
companies[]
tags[]
""";
        }
    }
}
