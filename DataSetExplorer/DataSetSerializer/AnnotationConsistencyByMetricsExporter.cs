using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.DataSetBuilder.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataSetExplorer.DataSetSerializer
{
    class AnnotationConsistencyByMetricsExporter
    {
        private readonly string _singleAnnotatorTemplatePath = "../../../DataSetSerializer/Template/Single_Annotator_Sanity_Check_Template.xlsx";
        private readonly string _multipleAnnotatorsTemplatePath = "../../../DataSetSerializer/Template/Between_Annotators_Sanity_Check_Template.xlsx";
        private readonly string _exportPath;
        private ExcelPackage _excelFile;
        private ExcelWorksheet _sheet;

        public AnnotationConsistencyByMetricsExporter(string exportPath)
        {
            _exportPath = exportPath;
        }

        public void ExportForSingleAnnotator(List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> dataSetInstances,
            string fileName, int annotatorId)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelFile = new ExcelPackage(new FileInfo(_singleAnnotatorTemplatePath));
            _sheet = _excelFile.Workbook.Worksheets[0];
            PopulateTemplateForSingle(dataSetInstances, annotatorId);
            Serialize(fileName + annotatorId);
        }

        public void ExportForMultipleAnnotators(List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> dataSetInstances,
            string fileName, int severity)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelFile = new ExcelPackage(new FileInfo(_multipleAnnotatorsTemplatePath));
            _sheet = _excelFile.Workbook.Worksheets[0];
            PopulateTemplateForMultiple(dataSetInstances, severity);
            Serialize(fileName + severity);
        }

        private void PopulateTemplateForSingle(List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> instances, int annotatorId)
        {
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 2 + i;
                _sheet.Cells[row, 1].Value = instances[i].Item1.Annotations.First(a => a.Annotator.Id == annotatorId).Severity;
                PopulateMetrics(instances[i].Item2, row);
            }
        }

        private void PopulateTemplateForMultiple(List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> instances, int severity)
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
