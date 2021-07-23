using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder.Model.Exceptions
{
    public class DataSetWithIdNotFound : Exception
    {
        public DataSetWithIdNotFound(int id) : base(
            $"DataSet with id: {id} does not exist.")
        {
        }
    }
}
