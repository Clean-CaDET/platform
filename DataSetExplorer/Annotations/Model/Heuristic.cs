using System.Collections.Generic;

namespace DataSetExplorer.Annotations.Model
{
    public class Heuristic
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<CodeSmellHeuristic> CodeSmellHeuristics { get; set; }

        public Heuristic(string name, string description)
        {
            Name = name;
            Description = description;
        }

        private Heuristic()
        {
        }

        public void Update(Heuristic other)
        {
            Name = other.Name;
            Description = other.Description;
        }
    }
}