using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETModel
    {
        public CaDETProject LatestSolutionState { get; private set; }
        public Dictionary<CommitId, CaDETProject> ProjectHistory { get; private set; }

        public CaDETModel(CaDETProject solution)
        {
            LatestSolutionState = solution;
            ProjectHistory = new Dictionary<CommitId, CaDETProject>();
        }

        public CaDETModel(): this(null)
        {

        }
    }
}
