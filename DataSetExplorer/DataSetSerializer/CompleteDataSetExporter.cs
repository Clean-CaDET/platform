using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.DataSetBuilder.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataSetExplorer.DataSetSerializer
{
    class CompleteDataSetExporter
    {
        private readonly string _templatePath = "../../../DataSetSerializer/Template/Complete_Dataset_Template.xlsx";
        private readonly string _exportPath;
        private ExcelPackage _excelFile;
        private ExcelWorksheet _sheet;

        public CompleteDataSetExporter(string exportPath)
        {
            _exportPath = exportPath;
        }

        public void Export(List<DataSetInstance> dataSetInstances, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelFile = new ExcelPackage(new FileInfo(_templatePath));
            _sheet = _excelFile.Workbook.Worksheets[0];
            PopulateTemplate(dataSetInstances);
            Serialize(fileName);
        }

        private void PopulateTemplate(List<DataSetInstance> instances)
        {
            PopulateHeader(instances);
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 3 + i;
                _sheet.Cells[row, 1].Value = instances[i].CodeSnippetId;
                _sheet.Cells[row, 2].Value = instances[i].Link;
                _sheet.Cells[row, 3].Value =
                    instances[i].Annotations.First().InstanceSmell.Value;
                _sheet.Cells[row, 4].Value = instances[i].ProjectLink;

                PopulateMetrics(instances[i].MetricFeatures, row);
                PopulateAnnotations(instances[i], row);
            }
        }

        private void PopulateHeader(List<DataSetInstance> instances)
        {
            var maxAnnotated = instances.OrderByDescending(i => i.Annotations.Count())
                .First().Annotations.ToList();
            var numOfMetrics = instances.First().MetricFeatures.Count;
            _sheet.Cells[1, 6 + numOfMetrics].Value = _sheet.Cells[1, 6].Value;
            _sheet.Cells[2, 5 + numOfMetrics].Value = _sheet.Cells[2, 7].Value;
            _sheet.Cells[1, 5, 1, 4 + numOfMetrics].Merge = true;
            _sheet.Cells[1, 6 + numOfMetrics, 1, 5 + numOfMetrics + maxAnnotated.Count()].Merge = true;

            for (int i = 0; i < maxAnnotated.Count(); i++)
            {
                _sheet.Cells[2, 6 + numOfMetrics + i].Value = maxAnnotated[i].Annotator.Id;
            }
        }

        private void PopulateMetrics(Dictionary<CaDETMetric, double> metrics, int row)
        {
            var i = 0;
            foreach (var key in metrics.Keys)
            {
                _sheet.Cells[2, 5 + i].Value = key;
                _sheet.Cells[row, 5 + i].Value = metrics[key];
                i++;
            }
        }

        private void PopulateAnnotations(DataSetInstance instance, int row)
        {
            var numOfMetrics = instance.MetricFeatures.Count;
            _sheet.Cells[row, 5 + numOfMetrics].Value = instance.GetFinalAnnotation();

            var i = 0;
            var annotations = instance.Annotations;
            while (_sheet.Cells[2, 6 + numOfMetrics + i].Value != null)
            {
                var annotationByCurrentAnnotator = annotations.FirstOrDefault(a => a.Annotator.Id.Equals(_sheet.Cells[2, 6 + numOfMetrics + i].Value));
                if (annotationByCurrentAnnotator != null)
                {
                    _sheet.Cells[row, 6 + numOfMetrics + i].Value = annotationByCurrentAnnotator.Severity;
                }
                else
                {
                    _sheet.Cells[row, 6 + numOfMetrics + i].Value = "/";
                }
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