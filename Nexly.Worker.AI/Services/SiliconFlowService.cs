using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Nexly.Worker.AI.Services
{
    public class SiliconFlowService
    {
        private readonly HttpClient _httpClient;

        public SiliconFlowService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> ChatAsync(string prompt)
        {
            var request = new
            {
                model = "Pro/zai-org/GLM-4.7",
                messages = new[]
                {
                new { role = "user", content = prompt }
            }
            };

            var response = await _httpClient.PostAsJsonAsync(
                "https://api.siliconflow.cn/v1/chat/completions",
                request);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
