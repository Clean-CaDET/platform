﻿using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer.ViewModel;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataSetExplorer.DataSetSerializer
{
    class CompleteDataSetExporter
    {
        private readonly string _dataSetWithAnnotationsTemplatePath = "../../../DataSetSerializer/Template/Complete_Dataset_With_Annotations_Template.xlsx";
        private readonly string _dataSetWithHeuristicsTemplatePath = "../../../DataSetSerializer/Template/Complete_Dataset_With_Heuristics_Template.xlsx";
        private readonly string _dataSetWithMetricsTemplatePath = "../../../DataSetSerializer/Template/Complete_Dataset_With_Metrics_Template.xlsx";
        private readonly string _exportPath;
        private ExcelPackage _excelFile;
        private ExcelWorksheet _sheet;
        private readonly ColumnHeuristicsModel _requiredSmells = new ColumnHeuristicsModel();

        public CompleteDataSetExporter(string exportPath)
        {
            _exportPath = exportPath;
        }

        public void Export(List<DataSetInstance> dataSetInstances, string smell, string fileName)
        {
            ExportDataSetWithAnnotations(dataSetInstances, fileName);
            ExportDataSetWithMetrics(dataSetInstances, fileName);
            ExportDataSetWithHeuristics(dataSetInstances, smell, fileName);
        }

        private void ExportDataSetWithAnnotations(List<DataSetInstance> dataSetInstances, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelFile = new ExcelPackage(new FileInfo(_dataSetWithAnnotationsTemplatePath));
            _sheet = _excelFile.Workbook.Worksheets[0];
            PopulateAnnotationsTemplate(dataSetInstances);
            Serialize(fileName + "_Annotations");
        }

        private void PopulateAnnotationsTemplate(List<DataSetInstance> instances)
        {
            PopulateAnnotationsHeader(instances);
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 3 + i;
                _sheet.Cells[row, 1].Value = instances[i].CodeSnippetId;
                _sheet.Cells[row, 2].Value = instances[i].Link;
                _sheet.Cells[row, 3].Value =
                    instances[i].Annotations.First().InstanceSmell.Value;
                _sheet.Cells[row, 4].Value = instances[i].ProjectLink;
                PopulateAnnotations(instances[i], row);
            }
        }

        private void PopulateAnnotationsHeader(List<DataSetInstance> instances)
        {
            var allAnnotators = GetAnnotationsFromFullyAnnotatedInstance(instances).Select(a => a.Annotator).ToList();
            _sheet.Cells[1, 6, 1, 5 + allAnnotators.Count()].Merge = true;

            for (int i = 0; i < allAnnotators.Count(); i++)
            {
                _sheet.Cells[2, 6 + i].Value = allAnnotators[i].Id;
            }
        }

        private static List<DataSetAnnotation> GetAnnotationsFromFullyAnnotatedInstance(List<DataSetInstance> instances)
        {
            return instances.OrderByDescending(i => i.Annotations.Count())
                            .First().Annotations.ToList();
        }

        private void PopulateAnnotations(DataSetInstance instance, int row)
        {
            _sheet.Cells[row, 5].Value = instance.GetFinalAnnotation();

            var i = 0;
            var annotations = instance.Annotations;
            while (GetAnnotatorFromHeader(i) != null)
            {
                var currentAnnotatorId = int.Parse(GetAnnotatorFromHeader(i).ToString());
                var annotationByCurrentAnnotator = annotations.FirstOrDefault(a => a.Annotator.Id == currentAnnotatorId);
                if (annotationByCurrentAnnotator != null)
                {
                    _sheet.Cells[row, 6 + i].Value = annotationByCurrentAnnotator.Severity;
                }
                else
                {
                    _sheet.Cells[row, 6 + i].Value = "/";
                }
                i++;
            }
        }

        private object GetAnnotatorFromHeader(int i)
        {
            return _sheet.Cells[2, 6 + i].Value;
        }

        private void ExportDataSetWithMetrics(List<DataSetInstance> dataSetInstances, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelFile = new ExcelPackage(new FileInfo(_dataSetWithMetricsTemplatePath));
            _sheet = _excelFile.Workbook.Worksheets[0];
            PopulateMetricsTemplate(dataSetInstances);
            Serialize(fileName + "_Metrics");
        }

        private void PopulateMetricsTemplate(List<DataSetInstance> instances)
        {
            PopulateMetricsHeader(instances);
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 3 + i;
                _sheet.Cells[row, 1].Value = instances[i].CodeSnippetId;
                PopulateMetrics(instances[i].MetricFeatures, row);
            }
        }

        private void PopulateMetricsHeader(List<DataSetInstance> instances)
        {
            var numOfMetrics = instances.First().MetricFeatures.Count;
            _sheet.Cells[1, 2, 1, 1 + numOfMetrics].Merge = true;
        }

        private void PopulateMetrics(Dictionary<CaDETMetric, double> metrics, int row)
        {
            var i = 0;
            foreach (var key in metrics.Keys)
            {
                _sheet.Cells[2, 2 + i].Value = key;
                _sheet.Cells[row, 2 + i].Value = metrics[key];
                i++;
            }
        }

        private void ExportDataSetWithHeuristics(List<DataSetInstance> dataSetInstances, string smell, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelFile = new ExcelPackage(new FileInfo(_dataSetWithHeuristicsTemplatePath));
            _sheet = _excelFile.Workbook.Worksheets[0];
            PopulateHeuristicsTemplate(dataSetInstances, smell);
            Serialize(fileName + "_Heuristics");
        }

        private void PopulateHeuristicsTemplate(List<DataSetInstance> instances, string smell)
        {
            var allAnnotators = GetAnnotationsFromFullyAnnotatedInstance(instances).Select(a => a.Annotator).ToList();
            PopulateHeuristicsHeader(instances, smell, allAnnotators);
            
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 5 + i;
                _sheet.Cells[row, 1].Value = instances[i].CodeSnippetId;
                PopulateHeuristics(instances[i].Annotations.ToList(), row, allAnnotators, smell);
            }
        }

        private void PopulateHeuristicsHeader(List<DataSetInstance> instances, string smell, List<Annotator> annotators)
        {
            var smellHeuristics = _requiredSmells.GetHeuristicsByCodeSmellName(smell);
            var numOfHeuristics = smellHeuristics.Count();

            _sheet.Cells[1, 2, 1, 1 + (numOfHeuristics * annotators.Count)].Merge = true;
            for (int i = 0; i < annotators.Count(); i++)
            {
                _sheet.Cells[2, 2 + (numOfHeuristics * i), 2, 1 + (numOfHeuristics * i) + numOfHeuristics].Merge = true;
                _sheet.Cells[2, 2 + (numOfHeuristics * i)].Value = annotators[i].Id;
            }
        }

        private void PopulateHeuristics(List<DataSetAnnotation> annotations, int row, List<Annotator> annotators, string smell)
        {
            var smellHeuristics = _requiredSmells.GetHeuristicsByCodeSmellName(smell);
            var numOfHeuristics = smellHeuristics.Count();
            for (int i = 0; i < annotations.Count(); i++)
            {
                for (int j = 0; j < annotators.Count(); j++)
                {
                    var annotatorId = int.Parse(_sheet.Cells[2, 2 + (numOfHeuristics * j)].Value.ToString());
                    if (annotatorId == annotations[i].Annotator.Id)
                    {
                        PopulateHeuristicValues(smellHeuristics, annotations[i].ApplicableHeuristics, row, j);
                    }
                }
            }
        }

        private void PopulateHeuristicValues(List<string> smellHeuristics, List<SmellHeuristic> applicableHeuristics, int row, int annotationNum)
        {
            var numOfHeuristics = smellHeuristics.Count();
            applicableHeuristics = GetCodeSmellHeuristicsForExport(smellHeuristics, applicableHeuristics);

            for (var i = 0; i < applicableHeuristics.Count(); i++)
            {
                _sheet.Cells[4, 2 + (numOfHeuristics * annotationNum) + i].Value = applicableHeuristics[i].Description;
                _sheet.Cells[3, 2 + (numOfHeuristics * annotationNum), 3, 1 + (numOfHeuristics * annotationNum) + numOfHeuristics].Merge = true;
                _sheet.Cells[3, 2 + (numOfHeuristics * annotationNum)].Value = "Heuristics";
                _sheet.Cells[row, 2 + (numOfHeuristics * annotationNum) + i].Value = applicableHeuristics[i].IsApplicable;
            }
        }
        
        private List<SmellHeuristic> GetCodeSmellHeuristicsForExport(List<string> heuristics, List<SmellHeuristic> applicableHeuristics)
        {
            var heuristicsForExport = new List<SmellHeuristic>();
            foreach (var heuristic in heuristics)
            {
                foreach (var applicableHeur in applicableHeuristics)
                {
                    if (heuristic.Equals(applicableHeur.Description)) {
                        heuristicsForExport.Add(applicableHeur);
                        break;
                    }
                }
            }
            return heuristicsForExport;
        }

        private void Serialize(string fileName)
        {
            var filePath = _exportPath + fileName + ".xlsx";
            _excelFile.SaveAs(new FileInfo(filePath));
        }
    }
}
