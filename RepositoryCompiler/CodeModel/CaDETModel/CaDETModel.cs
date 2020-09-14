using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETModel
    {
        public CaDETProject LatestState { get; private set; }
        public Dictionary<CommitId, CaDETProject> ProjectHistory { get; private set; }

        public CaDETModel(CaDETProject project)
        {
            LatestState = project;
            ProjectHistory = new Dictionary<CommitId, CaDETProject>();
        }

        public CaDETModel(): this(null)
        {

        }
    }
}
