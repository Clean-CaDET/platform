using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETModel
    {
        private readonly IEnumerable<CaDETDocument> _workingTreeState;
        private readonly Dictionary<CommitId, IEnumerable<CaDETDocument>> _projectHistory;

        public CaDETModel(IEnumerable<CaDETDocument> activeState)
        {
            _workingTreeState = activeState ?? new List<CaDETDocument>();
            _projectHistory = new Dictionary<CommitId, IEnumerable<CaDETDocument>>();
        }

        public CaDETModel(): this(null) { }

        public void AddProject(CommitId commit, IEnumerable<CaDETDocument> project)
        {
            _projectHistory.Add(commit, project);
        }
    }
}
