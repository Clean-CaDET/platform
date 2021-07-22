using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder.Model.Exceptions
{
    public class DataSetInstanceWithIdNotFound : Exception
    {
        public DataSetInstanceWithIdNotFound(int id) : base(
            $"DataSetInstance with id: {id} does not exist.")
        {
        }
    }
}
