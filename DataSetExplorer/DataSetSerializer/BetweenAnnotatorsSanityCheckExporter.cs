using System;
using System.Collections.Generic;
using System.IO;
using DataSetExplorer.DataSetBuilder.Model;
using OfficeOpenXml;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;

namespace DataSetExplorer.DataSetSerializer
{
    class BetweenAnnotatorsSanityCheckExporter
    {
        private readonly string _templatePath = "../../../DataSetSerializer/Template/Between_Annotators_Sanity_Check_Template.xlsx";
        private readonly string _exportPath;
        private ExcelPackage _excelFile;
        private ExcelWorksheet _sheet;

        public BetweenAnnotatorsSanityCheckExporter(string exportPath)
        {
            _exportPath = exportPath;
        }

        public void Export(List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> dataSetInstances,
            string fileName, int severity)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelFile = new ExcelPackage(new FileInfo(_templatePath));
            _sheet = _excelFile.Workbook.Worksheets[0];
            PopulateTemplate(dataSetInstances, severity);
            Serialize(fileName + severity);
        }

        private void PopulateTemplate(List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> instances, int severity)
        {
            var j = 0;
            foreach (var instance in instances)
            {
                foreach (var annotation in instance.Item1.Annotations)
                {
                    if (annotation.Severity == severity)
                    {
                        _sheet.Cells[2 + j, 1].Value = annotation.Annotator.Id;
                        PopulateMetrics(instance.Item2, 2 + j);
                        j++;
                    }
                }
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