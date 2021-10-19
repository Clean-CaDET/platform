using System.Collections.Generic;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class DataSet
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public HashSet<DataSetProject> Projects { get; private set; }
        public List<CodeSmell> SupportedCodeSmells { get; private set; }

        public DataSet(string name, List<CodeSmell> codeSmells)
        {
            Name = name;
            Projects = new HashSet<DataSetProject>();
            SupportedCodeSmells = codeSmells;
        }

        private DataSet()
        {
        }

        public void AddProject(DataSetProject project)
        {
            Projects.Add(project);
        }
    }
}
