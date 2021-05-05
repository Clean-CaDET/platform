namespace SmartTutor.LearnerModel.Learners
{
    public class Workspace
    {
        public string Path { get; private set; }

        public Workspace(string path)
        {
            Path = path;
        }
    }
}
