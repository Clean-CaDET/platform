using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETModel
    {
        public IEnumerable<CaDETDocument> WorkingTreeState { get; private set; }
        public Dictionary<CommitId, IEnumerable<CaDETDocument>> ProjectHistory { get; private set; }

        public CaDETModel(IEnumerable<CaDETDocument> activeState)
        {
            WorkingTreeState = activeState;
            ProjectHistory = new Dictionary<CommitId, IEnumerable<CaDETDocument>>();
        }

        public CaDETModel(): this(null)
        {

        }
    }
}
