using System;

namespace DataSetExplorer.Core.Annotations.Model
{
    public class Annotator
    {
        public int Id { get; private set; }
        public int YearsOfExperience { get; private set; }
        public int Ranking { get; private set; }

        public Annotator(int id)
        {
            Id = id;
        }

        public Annotator(int id, int yearsOfExperience, int ranking)
        {
            Id = id;
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
                   YearsOfExperience == other.YearsOfExperience &&
                   Ranking == other.Ranking;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, YearsOfExperience, Ranking);
        }
    }
}