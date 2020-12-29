using System;
using System.Collections.Generic;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class CodeSmell
    {
        public CodeSmell(string smell)
        {
            Value = smell;
            Validate();
        }

        public string Value { get; }

        public void Validate()
        {
            switch (Value)
            {
                case "Large Class":
                case "Long Method":
                    return;
                default:
                    throw new InvalidOperationException("Unsupported code smell type.");
            }
        }

        public List<SnippetType> RelevantSnippetType()
        {
            return Value switch
            {
                "Large Class" => new List<SnippetType> {SnippetType.Class},
                "Long Method" => new List<SnippetType> {SnippetType.Function},
                _ => null
            };
        }
    }
}