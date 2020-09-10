namespace RepositoryCompiler.CodeModel.CaDETModel
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

        public bool Equals(CommitId c)
        {
            if (ReferenceEquals(c, null)) return false;
            if (ReferenceEquals(this, c)) return true;
            if (GetType() != c.GetType()) return false;

            return Hash.Equals(c.Hash);
        }
    }
}
