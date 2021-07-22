using DataSetExplorer.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder.Model.Repository
{
    public interface IDataSetInstanceRepository
    {
        public DataSetInstance GetDataSetInstance(int id);
        public Annotator GetAnnotator(int id);
        public void AddAnnotation(DataSetInstance instance);
    }
}
