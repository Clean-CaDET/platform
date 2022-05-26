using System;

namespace DataSetExplorer.Core.Annotations.Model
{
    public class Annotator
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int YearsOfExperience { get; private set; }
        public int Ranking { get; private set; }

        public Annotator(int id)
        {
            Id = id;
        }

        public Annotator(int id, string name, int yearsOfExperience, int ranking)
        {
            Id = id;
            Name = name;
            YearsOfExperience = yearsOfExperience;
            Ranking = ranking;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Annotator);
        }

        public bool Equals(Annotator other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   YearsOfExperience == other.YearsOfExperience &&
                   Ranking == other.Ranking;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, YearsOfExperience, Ranking);
        }
    }
}