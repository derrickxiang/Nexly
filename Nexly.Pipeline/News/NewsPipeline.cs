using Nexly.Pipeline.News.Models;
using Nexly.Pipeline.News.Steps;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.News
{
    public class NewsPipeline
    {
        private readonly NormalizeNewsStep _normalize;
        private readonly DeduplicateNewsStep _deduplicate;
        private readonly AIEnrichNewsStep _ai;
        private readonly PersistNewsStep _persist;

        public NewsPipeline(
            NormalizeNewsStep normalize,
            DeduplicateNewsStep deduplicate,
            AIEnrichNewsStep ai,
            PersistNewsStep persist)
        {
            _normalize = normalize;
            _deduplicate = deduplicate;
            _ai = ai;
            _persist = persist;
        }

        public async Task ProcessAsync(RawNewsItem raw)
        {
            var normalized = await _normalize.ExecuteAsync(raw);

            var isNew = await _deduplicate.ExecuteAsync(normalized);

            if (!isNew)
                return;

            var enriched = await _ai.ExecuteAsync(normalized);

            await _persist.ExecuteAsync(enriched);
        }
    }
}
