using System.Collections.Generic;

namespace PlatformInteractionTool.DataSet.Model
{
    internal class DataSetProject
    {
        internal List<DataSetClass> Classes;
        internal List<DataSetFunction> Functions;
        internal DataSetProject()
        {
            Classes = new List<DataSetClass>();
            Functions = new List<DataSetFunction>();
        }
    }
}