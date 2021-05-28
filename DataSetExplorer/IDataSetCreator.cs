﻿using DataSetExplorer.DataSetSerializer.ViewModel;
using FluentResults;

namespace DataSetExplorer
{
    public interface IDataSetCreator
    {
        public Result<string> CreateDataSetSpreadsheet(string projectName, string projectAndCommitUrl);
        public Result<string> CreateDataSetSpreadsheet(string projectName, string projectAndCommitUrl, NewSpreadSheetColumnModel columnModel);
    }
}
