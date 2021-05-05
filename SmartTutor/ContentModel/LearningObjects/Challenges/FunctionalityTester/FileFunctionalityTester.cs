using System;
using System.Diagnostics;
using System.IO;

namespace SmartTutor.ContentModel.LearningObjects.Challenges.FunctionalityTester
{
    public class WorkspaceFunctionalityTester: IFunctionalityTester
    {
        private readonly string _workspacePath;

        public WorkspaceFunctionalityTester(string workspacePath)
        {
            _workspacePath = workspacePath;
        }

        public ChallengeEvaluation IsFunctionallyCorrect(string[] sourceCode, string testSuitePath)
        {
            var fullPath = GetFullPath(testSuitePath);

            if (sourceCode != null) SaveCodeToDirectory(sourceCode, fullPath);
            var result = RunTestingProcess(fullPath);

            return BuildEvaluation(result);
        }

        private string GetFullPath(string testSuitePath)
        {
            var fullPath = Path.Combine(_workspacePath, testSuitePath);
            if (!Directory.Exists(fullPath)) throw new InvalidOperationException("Learner workspace is not setup.");
            return fullPath;
        }

        private static void SaveCodeToDirectory(string[] sourceCode, string fullPath)
        {
            for (var i = 0; i < sourceCode.Length; i++)
            {
                File.WriteAllText(Path.Combine(fullPath, i + ".cs"), sourceCode[i]);
            }
        }

        private static string RunTestingProcess(string fullPath)
        {
            var procStartInfo = new ProcessStartInfo("dotnet", "test \"" + fullPath + "\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process {StartInfo = procStartInfo};
            process.Start();
            return process.StandardOutput.ReadToEnd();
        }

        private static ChallengeEvaluation BuildEvaluation(string result)
        {
            //TODO: Handle no test results situation.
            if (result.Contains("Failed:     0")) return null;
            
            var evaluation = new ChallengeEvaluation(0);
            evaluation.ApplicableHints.AddHint("TEST RESULTS", new ChallengeHint(0, result));
            return evaluation;
        }
    }
}
