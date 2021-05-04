using System;
using System.IO;

namespace SmartTutor.ContentModel.LearningObjects.Challenges.FunctionalityTester
{
    internal class FileFunctionalityTester: IFunctionalityTester
    {
        private readonly string _workspacePath;

        public FileFunctionalityTester(string workspacePath)
        {
            _workspacePath = workspacePath;
        }

        public ChallengeEvaluation IsFunctionallyCorrect(string[] sourceCode)
        {
            //if (!Directory.Exists(_workspacePath)) throw new InvalidOperationException("Learner workspace is not setup.");


            return null;
        }
    }
}
