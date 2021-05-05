namespace SmartTutor.ContentModel.LearningObjects.Challenges.FunctionalityTester
{
    public interface IFunctionalityTester
    {
        public ChallengeEvaluation IsFunctionallyCorrect(string[] sourceCode, string testSuitePath);
    }
}
