using System;
using System.Collections.Generic;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class CodeSmell
    {
        public CodeSmell(string smell)
        {
            Name = smell;
            Validate();
        }

        private CodeSmell()
        {
        }

        public int Id { get; private set; }
        public string Name { get; private set; }

        public void Validate()
        {
            switch (Name)
            {
                case "Large Class":
                case "Long Method":
                    return;
                default:
                    throw new InvalidOperationException("Unsupported code smell type.");
            }
        }

        public List<SnippetType> RelevantSnippetTypes()
        {
            return Name switch
            {
                "Large Class" => new List<SnippetType> {SnippetType.Class},
                "Long Method" => new List<SnippetType> {SnippetType.Function},
                _ => null
            };
        }
    }
}