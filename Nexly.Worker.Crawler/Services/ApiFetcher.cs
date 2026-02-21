using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Worker.Crawler.Services
{
    public class ApiFetcher
    {
        private readonly HttpClient _http;

        public ApiFetcher(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> FetchAsync(string url)
        {
            return await _http.GetStringAsync(url);
        }
    }
}
