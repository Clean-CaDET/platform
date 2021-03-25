using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects
{
    public class Challenge : LearningObject
    {
        public string Url { get; set; }
        public List<CaDETClass> EndState { get; set; }
    }
}
