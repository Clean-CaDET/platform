namespace DataSetExplorer.DataSetBuilder.Model
{
    public class Annotator
    {
        public int Id { get; set; }
        public int YearsOfExperience { get; set; }
        public int Ranking { get; set; }
        public Annotator(int id, int yearsOfExperience, int ranking)
        {
            Id = id;
            YearsOfExperience = yearsOfExperience;
            Ranking = ranking;
        }
    }
}