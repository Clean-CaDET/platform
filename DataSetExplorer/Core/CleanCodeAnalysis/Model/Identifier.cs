namespace DataSetExplorer.Core.CleanCodeAnalysis.Model
{
    public class Identifier
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public IdentifierType Type { get; private set; }

        public Identifier(string name, IdentifierType type)
        {
            Name = name;
            Type = type;
        }

        private Identifier() { }

    }

    public enum IdentifierType
    {
        Class,
        Field,
        Member,
        Variable,
        Parameter
    }
}