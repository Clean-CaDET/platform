namespace RepositoryCompiler.CodeParsers.Data
{
    public class CommitId
    {
        public string Hash { get; set; }

        public CommitId(string hash)
        {
            Hash = hash;
        }

        public override string ToString()
        {
            return Hash;
        }
    }
}
