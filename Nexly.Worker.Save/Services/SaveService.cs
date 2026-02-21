using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Worker.Save.Services
{
    public class SaveService
    {
        private readonly ILogger<SaveService> _logger;

        public SaveService(ILogger<SaveService> logger)
        {
            _logger = logger;
        }

        public Task SaveAsync(string json)
        {
            // TODO: 调用 Infrastructure Repository 保存数据库

            _logger.LogInformation("Saving result: {json}", json);

            return Task.CompletedTask;
        }
    }
}
