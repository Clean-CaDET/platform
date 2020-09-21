using System;
using System.Collections.Generic;
using RepositoryCompiler.RepositoryAdapters;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETModel
    {
        public Guid Id { get; }
        private readonly Dictionary<CommitId, CaDETProject> _projectHistory;

        public CaDETModel(): this(new Guid()) {}

        public CaDETModel(Guid id)
        {
            Id = id;
            _projectHistory = new Dictionary<CommitId, CaDETProject>();
        }

        public void AddProject(CommitId commit, CaDETProject project)
        {
            _projectHistory.Add(commit, project);
        }
    }
}
