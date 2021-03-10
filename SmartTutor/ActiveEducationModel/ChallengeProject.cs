using RepositoryCompiler.CodeModel.CaDETModel;

namespace SmartTutor.ActiveEducationModel
{
    public class ChallengeProject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public double Points { get; set; }
        public string GitURL { get; set; }
        public CaDETProject StartState { get; set; }
        public CaDETProject EndState { get; set; }
        //public List<ProjectHint> Hints { get; set; }
    }
}
