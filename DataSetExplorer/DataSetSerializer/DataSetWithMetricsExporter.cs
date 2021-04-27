using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataSetExplorer.DataSetBuilder.Model;
using OfficeOpenXml;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;

namespace DataSetExplorer.DataSetSerializer
{
    class DataSetWithMetricsExporter
    {
        private readonly string _templatePath = "../../../DataSetSerializer/Template/Dataset_Metrics_Template.xlsx";
        private readonly string _exportPath;
        private ExcelPackage _excelFile;
        private ExcelWorksheet _sheet;

        public DataSetWithMetricsExporter(string exportPath)
        {
            _exportPath = exportPath;
        }

        public void Export(List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> dataSetInstances,
            string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelFile = new ExcelPackage(new FileInfo(_templatePath));
            _sheet = _excelFile.Workbook.Worksheets[0];
            PopulateTemplate(dataSetInstances);
            Serialize(fileName);
        }

        private void PopulateTemplate(List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> instances)
        {
            PopulateHeader(instances);
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 3 + i;
                _sheet.Cells[row, 1].Value = instances[i].Item1.CodeSnippetId;
                _sheet.Cells[row, 2].Value = instances[i].Item1.Link;
                _sheet.Cells[row, 3].Value =
                    instances[i].Item1.Annotations.First().InstanceSmell.Value;
                _sheet.Cells[row, 4].Value = instances[i].Item1.ProjectLink;

                PopulateMetrics(instances[i].Item2, row);
                PopulateAnnotations(instances[i], row);
            }
        }

        private void PopulateHeader(List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> instances)
        {
            var maxAnnotated = instances.Select(i => i.Item1)
                .OrderByDescending(i => i.Annotations.Count())
                .First().Annotations.ToList();
            var numOfMetrics = instances.First().Item2.Count;
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

        private void PopulateAnnotations(Tuple<DataSetInstance, Dictionary<CaDETMetric, double>> instance, int row)
        {
            var numOfMetrics = instance.Item2.Count;
            _sheet.Cells[row, 5 + numOfMetrics].Value = instance.Item1.GetFinalAnnotation();

            var i = 0;
            var annotations = instance.Item1.Annotations;
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