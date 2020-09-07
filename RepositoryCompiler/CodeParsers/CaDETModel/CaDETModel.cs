using System.Collections.Generic;
using RepositoryCompiler.CodeParsers.Data;

namespace RepositoryCompiler.CodeParsers.CaDETModel
{
    class CaDETModel
    {
        public CaDETProject LatestSolutionState { get; private set; }
        public Dictionary<CommitId, CaDETProject> ProjectHistory { get; private set; }

        public CaDETModel(CaDETProject solution)
        {
            this.LatestSolutionState = solution;
            ProjectHistory = new Dictionary<CommitId, CaDETProject>();
        }
    }
}
