namespace RepositoryCompiler.CodeParsers.Data
{
    public class CommitId
    {
        public string Hash { get; set; }

        public CommitId(string hash)
        {
            Hash = hash;
        }

        internal static CommitId Create(string commitHash)
        {
            return commitHash == null ? null : new CommitId(commitHash);
        }
    }
}
