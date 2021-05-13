using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.DataSetBuilder.Model;
using OfficeOpenXml;

namespace DataSetExplorer.DataSetSerializer
{
    class SingleAnnotatorSanityCheckExporter
    {
        private readonly string _templatePath = "../../../DataSetSerializer/Template/Single_Annotator_Sanity_Check_Template.xlsx";
        private readonly string _exportPath;
        private ExcelPackage _excelFile;
        private ExcelWorksheet _sheet;

        public SingleAnnotatorSanityCheckExporter(string exportPath)
        {
            _exportPath = exportPath;
        }

        public void Export(List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> dataSetInstances,
            string fileName, int annotatorId)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelFile = new ExcelPackage(new FileInfo(_templatePath));
            _sheet = _excelFile.Workbook.Worksheets[0];
            PopulateTemplate(dataSetInstances, annotatorId);
            Serialize(fileName + annotatorId);
        }

        private void PopulateTemplate(List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> instances, int annotatorId)
        {
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 2 + i;
                _sheet.Cells[row, 1].Value = instances[i].Item1.Annotations.First(a => a.Annotator.Id == annotatorId).Severity;
                PopulateMetrics(instances[i].Item2, row);
            }
        }

        private void PopulateMetrics(Dictionary<CaDETMetric, double> metrics, int row)
        {
            var i = 0;
            foreach (var key in metrics.Keys)
            {
                _sheet.Cells[1, 2 + i].Value = key;
                _sheet.Cells[row, 2 + i].Value = metrics[key];
                i++;
            }
        }

        private void Serialize(string fileName)
        {
            var filePath = _exportPath + fileName + ".xlsx";
            _excelFile.SaveAs(new FileInfo(filePath));
        }
    }
}