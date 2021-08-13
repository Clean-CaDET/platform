using System;
using System.IO;

namespace SmartTutor.Utils
{
    public static class Util
    {
        public static string GetSecret(string secretName)
        {
            var secretPath = Environment.GetEnvironmentVariable($"{secretName}_FILE") ?? "";
            return File.Exists(secretPath) ? 
                File.ReadAllText(secretPath) : 
                Environment.GetEnvironmentVariable(secretName);
        }
    }
}