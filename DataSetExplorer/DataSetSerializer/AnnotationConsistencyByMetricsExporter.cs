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
        private readonly string _singleAnnotatorTemplatePath = "../../../DataSetSerializer/Template/Single_Annotator_Consistency_Template.xlsx";
        private readonly string _multipleAnnotatorsTemplatePath = "../../../DataSetSerializer/Template/Consistency_Between_Annotators_Template.xlsx";
        private readonly string _exportPath;
        private ExcelPackage _excelFile;
        private ExcelWorksheet _sheet;

        public AnnotationConsistencyByMetricsExporter(string exportPath)
        {
            _exportPath = exportPath;
        }

        public void ExportAnnotationsFromAnnotator(int annotatorId, List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> dataSetInstances,
            string fileName)
        {
            InitializeExcelSheet(_singleAnnotatorTemplatePath);
            PopulateTemplateForAnnotator(annotatorId, dataSetInstances);
            Serialize(fileName + annotatorId);
        }

        public void ExportAnnotatorsForSeverity(int severity, List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> dataSetInstances,
            string fileName)
        {
            InitializeExcelSheet(_multipleAnnotatorsTemplatePath);
            PopulateTemplateForSeverity(severity, dataSetInstances);
            Serialize(fileName + severity);
        }

        private void InitializeExcelSheet(string templatePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelFile = new ExcelPackage(new FileInfo(templatePath));
            _sheet = _excelFile.Workbook.Worksheets[0];
        }

        private void PopulateTemplateForAnnotator(int annotatorId, List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> instances)
        {
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 2 + i;
                _sheet.Cells[row, 1].Value = instances[i].Item1.Annotations.First(a => a.Annotator.Id == annotatorId).Severity;
                PopulateMetrics(instances[i].Item2, row);
            }
        }

        private void PopulateTemplateForSeverity(int severity, List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> instances)
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
