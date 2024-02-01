﻿using System.Collections.Generic;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.UI.Controllers.Dataset.DTOs.Summary;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public interface IDataSetRepository
    {
        void Create(DataSet dataSet);
        DatasetDetailDTO Get(int id);
        DataSet GetDataSetForExport(int id);
        DataSet GetDataSetWithProjectsAndCodeSmells(int id);
        IEnumerable<DatasetSummaryDTO> GetAll();
        IEnumerable<DataSet> GetAllByCodeSmell(string codeSmellName);
        DataSet Update(DataSet dataSet);
        List<CodeSmell> GetDataSetCodeSmells(int id);
        DataSet Delete(int id);
    }
}
