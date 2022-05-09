using System.Collections.Generic;
using DataSetExplorer.Core.DataSets.Model;

namespace DataSetExplorer.Core.AnnotationSchema.Model
{
    public class CodeSmellDefinition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SnippetType SnippetType { get; set; }
        public SeverityRange SeverityRange { get; set; }
        public List<string> SeverityValues { get; set;  }
        public List<HeuristicDefinition> Heuristics { get; set; }

        public CodeSmellDefinition(string name, string description, SnippetType snippetType)
        {
            Name = name;
            Description = description;
            SnippetType = snippetType;
            Heuristics = new List<HeuristicDefinition>();
        }

        private CodeSmellDefinition()
        {
        }

        public void Update(CodeSmellDefinition other)
        {
            Name = other.Name;
            Description = other.Description;
            SnippetType = other.SnippetType;
            SeverityRange = other.SeverityRange;
            SeverityValues = other.SeverityValues;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object other)
        {
            return other is CodeSmellDefinition smell && Name.Equals(smell.Name);
        }
    }
}