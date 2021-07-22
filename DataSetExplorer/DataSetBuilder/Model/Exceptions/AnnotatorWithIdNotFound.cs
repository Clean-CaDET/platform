using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder.Model.Exceptions
{
    public class AnnotatorWithIdNotFound : Exception
    {
        public AnnotatorWithIdNotFound(int id) : base(
            $"Annotator with id: {id} does not exist.")
        {
        }
    }
}
