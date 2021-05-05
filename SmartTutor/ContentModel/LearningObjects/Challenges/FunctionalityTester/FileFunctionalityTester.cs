using System;
using System.Diagnostics;
using System.IO;
using System.Text;

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
            if (result.Contains("Failed:     0")) return null;

            result = result.Contains("Failed") ? ProcessFailedTest(result) : ProcessCompilationError(result);

            var evaluation = new ChallengeEvaluation(0);
            evaluation.ApplicableHints.AddHint("FUNCTIONAL TEST RESULTS", new ChallengeHint(0, result));
            return evaluation;
        }

        private static string ProcessFailedTest(string result)
        {
            var message = result.Split("\n");
            var lastHeaderLine = 8;

            //TODO: Remove to support localization.
            var sb = new StringBuilder("Some functional tests failed. Try to determine which functionality was broken from the test name or use git checkout to restart your challenge. The following tests failed:\n");
            for (var i = lastHeaderLine; i < message.Length; i++)
            {
                var line = message[i];
                if (!line.StartsWith("  Failed")) continue;
                
                sb.Append(line).AppendLine();
            }

            return sb.ToString();
        }

        private static string ProcessCompilationError(string result)
        {
            var message = result.Split("\n");
            var lastHeaderLine = 2;

            var sb = new StringBuilder("The following compilation errors occurred:\n\r");
            for(var i = lastHeaderLine; i < message.Length; i++)
            {
                var line = message[i];
                try
                {
                    var removedTestClassSuffix = line.Split("[")[0];
                    var removedTestClassPrefixAndSuffix = removedTestClassSuffix.Split(".cs")[1];
                    sb.Append(removedTestClassPrefixAndSuffix).Append("\n\r");
                }
                catch (IndexOutOfRangeException)
                {
                    //Skip line.
                }
            }

            return sb.ToString();
        }
    }
}
