using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder.Model.Repository
{
    public interface IDataSetAnnotationRepository
    {
        Annotation GetDataSetAnnotation(int id);
        Annotator GetAnnotator(int id);
        CodeSmell GetCodeSmell(string name);
        void Update(Annotation annotation);
    }
}
