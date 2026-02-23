using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Worker.AI.Prompts
{
    public class SemiPrompt
    {
        public string Prompt { get; set; } = "你是一名资深半导体行业分析师。\r\n\r\n请分析以下新闻：\r\n\r\n输出JSON：\r\n1. summary (100字)\r\n2. category (Fab/Equipment/AI/Policy/Investment)\r\n3. companies (数组)\r\n4. importance_score (1-10)\r\n5. industry_impact.";
    }
}
