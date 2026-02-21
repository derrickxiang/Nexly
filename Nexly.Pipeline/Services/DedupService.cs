using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Nexly.Pipeline.Services
{
    public class DedupService
    {
        private static readonly HashSet<string> Cache = new();

        public bool IsDuplicate(string title, string content)
        {
            var hash = ComputeHash(title + content);

            if (Cache.Contains(hash))
                return true;

            Cache.Add(hash);

            return false;
        }

        private string ComputeHash(string text)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            return Convert.ToHexString(bytes);
        }
    }
}
