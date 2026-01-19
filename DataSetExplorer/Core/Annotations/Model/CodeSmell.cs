using System;
using System.Collections.Generic;
using DataSetExplorer.Core.DataSets.Model;

namespace DataSetExplorer.Core.Annotations.Model
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
                case "Large_Class":
                case "Long_Method":
                case "Feature_Envy":
                case "Data_Class":
                case "Refused_Bequest":
                    return;
                default:
                    throw new InvalidOperationException("Unsupported code smell type.");
            }
        }

        public List<SnippetType> RelevantSnippetTypes()
        {
            return Name switch
            {
                "Large_Class" => new List<SnippetType> {SnippetType.Class},
                "Long_Method" => new List<SnippetType> {SnippetType.Function},
                "Feature_Envy" => new List<SnippetType> { SnippetType.Function },
                "Data_Class" => new List<SnippetType> { SnippetType.Class },
                "Refused_Bequest" => new List<SnippetType> { SnippetType.Class },
                _ => null
            };
        }

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