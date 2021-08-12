using DataSetExplorer.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder.Model.Repository
{
    public interface IDataSetInstanceRepository
    {
        DataSetInstance GetDataSetInstance(int id);
        Annotator GetAnnotator(int id);
        void Update(DataSetInstance instance);
    }
}
