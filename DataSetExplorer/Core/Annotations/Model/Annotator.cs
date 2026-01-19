using System;

namespace DataSetExplorer.Core.Annotations.Model
{
    public class Annotator
    {
        public int Id { get; set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public int YearsOfExperience { get; private set; }
        public int Ranking { get; private set; }

        public Annotator() {}

        public Annotator(int id)
        {
            Id = id;
        }

        public Annotator(int id, string name, string email, int yearsOfExperience, int ranking)
        {
            Id = id;
            Name = name;
            Email = email;
            YearsOfExperience = yearsOfExperience;
            Ranking = ranking;
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
                   Email == other.Email &&
                   YearsOfExperience == other.YearsOfExperience &&
                   Ranking == other.Ranking;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Email, YearsOfExperience, Ranking);
        }
    }
}