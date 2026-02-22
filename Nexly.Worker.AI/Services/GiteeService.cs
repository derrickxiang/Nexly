using Azure;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Nexly.Worker.AI.Services
{
    public class GiteeService: IAiService
    {

        private readonly HttpClient _http;

        private const string ApiKey = "S4WJKANARCH827JEZMFM34NBIZOHTKEPRKVJ3U1W";
        private const string Endpoint = "https://ai.gitee.com/v1/chat/completions";

        public GiteeService(HttpClient http)
        {
            _http = http;
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
        }

        public async Task<AiResult> ProcessAsync(string content)
        {
            //var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://ai.gitee.com/v1/chat/completions"),
                Content = new StringContent("{\n  \"model\": \"Qwen3-235B-A22B-Instruct-2507\",\n  \"messages\": [\n    {\"content\": \"\",\n      \"role\": \"developer\",\n      \"name\": null}\n  ],\n  \"stream\": true,\n  \"max_tokens\": 0,\n  \"frequency_penalty\": 0,\n  \"presence_penalty\": 0,\n  \"stop\": null,\n  \"temperature\": 1,\n  \"top_p\": 1,\n  \"top_logprobs\": 0,\n  \"response_format\": null,\n  \"seed\": 0,\n  \"n\": 1,\n  \"logit_bias\": {\"1000\": 99.9,\n    \"1001\": -99.9},\n  \"user\": null,\n  \"tools\": [\n    {\n      \"type\": \"function\",\n      \"function\": {\"name\": \"\",\n        \"description\": null,\n        \"parameters\": null}\n    }\n  ],\n  \"guided_json\": null,\n  \"guided_choice\": [\n    null\n  ]\n}")
                {
                    Headers =
        {
            ContentType = new MediaTypeHeaderValue("application/json")
        }
                }
            };

            using (var response = await _http.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);

                var json = await response.Content.ReadFromJsonAsync<JsonElement>();

                var text = json
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                Console.WriteLine(text);

                return new AiResult
                {
                    Summary = text ?? "",
                    Chinese = text ?? ""
                };
            }
        }
    }

}
