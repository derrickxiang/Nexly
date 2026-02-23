using Nexly.Pipeline.News.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Nexly.Pipeline.News.Steps
{
    public class NormalizeNewsStep
    {
        public Task<NormalizedNews> ExecuteAsync(RawNewsItem input)
        {
            var normalized = new NormalizedNews
            {
                Title = input.Title.Trim(),
                Url = input.Url,
                Source = input.Source,
                PublishedAt = input.PublishedAt,
                Content = CleanHtml(input.Content),
                Hash = GenerateHash(input.Title, input.Url)
            };

            return Task.FromResult(normalized);
        }

        private string CleanHtml(string html)
        {
            return html; // TODO: HTML sanitize
        }

        private string GenerateHash(string title, string url)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(title + url);
            return Convert.ToHexString(sha.ComputeHash(bytes));
        }
    }
}
