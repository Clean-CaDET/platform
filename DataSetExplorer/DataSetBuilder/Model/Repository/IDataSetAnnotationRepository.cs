using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder.Model.Repository
{
    public interface IDataSetAnnotationRepository
    {
        DataSetAnnotation GetDataSetAnnotation(int id);
        Annotator GetAnnotator(int id);
        void Update(DataSetAnnotation annotation);
    }
}
