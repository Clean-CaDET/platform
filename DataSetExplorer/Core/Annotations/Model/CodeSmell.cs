using DataSetExplorer.Core.DataSets.Model;

namespace DataSetExplorer.Core.Annotations.Model
{
    public class CodeSmell
    {
        public CodeSmell(string smell, SnippetType snippetType)
        {
            Name = smell;
            SnippetType = snippetType;
        }

        public CodeSmell(string smell)
        {
            Name = smell;
        }

        private CodeSmell()
        {
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public SnippetType SnippetType { get; private set; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object other)
        {
            return other is CodeSmell smell && Name.Equals(smell.Name);
        }
    }
}