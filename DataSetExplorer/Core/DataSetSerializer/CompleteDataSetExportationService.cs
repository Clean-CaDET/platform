using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.AnnotationSchema.Model;
using DataSetExplorer.Core.AnnotationSchema.Repository;
using DataSetExplorer.Core.DataSets.Model;
using OfficeOpenXml;

namespace DataSetExplorer.Core.DataSetSerializer
{
    class CompleteDataSetExportationService: ICompleteDataSetExportationService
    {
        private readonly string _dataSetWithAnnotationsTemplatePath = "./Core/DataSetSerializer/Template/Complete_Dataset_With_Annotations_Template.xlsx";
        private readonly string _dataSetWithHeuristicsTemplatePath = "./Core/DataSetSerializer/Template/Complete_Dataset_With_Heuristics_Template.xlsx";
        private readonly string _dataSetWithMetricsTemplatePath = "./Core/DataSetSerializer/Template/Complete_Dataset_With_Metrics_Template.xlsx";
        private string _exportPath;
        private ExcelPackage _excelFile;
        private ExcelWorksheet _sheet;
        private readonly IAnnotationSchemaRepository _annotationSchemaRepository;

        public CompleteDataSetExportationService(IAnnotationSchemaRepository annotationSchemaRepository)
        {
            _annotationSchemaRepository = annotationSchemaRepository;
        }

        public void Export(string outputPath, List<Instance> dataSetInstances, string smell, string fileName)
        {
            _exportPath = outputPath;
            ExportDataSetWithAnnotations(dataSetInstances, fileName);
            ExportDataSetWithMetrics(dataSetInstances, fileName);
            ExportDataSetWithHeuristics(dataSetInstances, smell, fileName);
        }

        private void ExportDataSetWithAnnotations(List<Instance> dataSetInstances, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelFile = new ExcelPackage(new FileInfo(_dataSetWithAnnotationsTemplatePath));
            _sheet = _excelFile.Workbook.Worksheets[0];
            PopulateAnnotationsTemplate(dataSetInstances);
            Serialize(fileName + "_Annotations");
        }

        private void PopulateAnnotationsTemplate(List<Instance> instances)
        {
            PopulateAnnotationsHeader(instances);
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 3 + i;
                _sheet.Cells[row, 1].Value = instances[i].CodeSnippetId;
                _sheet.Cells[row, 2].Value = instances[i].Link;
                if (instances[i].Annotations.Count > 0)
                {
                    _sheet.Cells[row, 3].Value =
                    instances[i].Annotations.First().InstanceSmell.Name;
                }
                _sheet.Cells[row, 4].Value = instances[i].ProjectLink;
                PopulateAnnotations(instances[i], row);
            }
        }

        private void PopulateAnnotationsHeader(List<Instance> instances)
        {
            var allAnnotators = GetAnnotationsFromFullyAnnotatedInstance(instances).Select(a => a.Annotator).ToList();
            _sheet.Cells[1, 6, 1, 5 + allAnnotators.Count()].Merge = true;

            for (int i = 0; i < allAnnotators.Count(); i++)
            {
                _sheet.Cells[2, 6 + i].Value = allAnnotators[i].Id;
            }
        }

        private static List<Annotation> GetAnnotationsFromFullyAnnotatedInstance(List<Instance> instances)
        {
            return instances.OrderByDescending(i => i.Annotations.Count())
                            .First().Annotations.ToList();
        }

        private void PopulateAnnotations(Instance instance, int row)
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

        private void ExportDataSetWithMetrics(List<Instance> dataSetInstances, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelFile = new ExcelPackage(new FileInfo(_dataSetWithMetricsTemplatePath));
            _sheet = _excelFile.Workbook.Worksheets[0];
            PopulateMetricsTemplate(dataSetInstances);
            Serialize(fileName + "_Metrics");
        }

        private void PopulateMetricsTemplate(List<Instance> instances)
        {
            PopulateMetricsHeader(instances);
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 3 + i;
                _sheet.Cells[row, 1].Value = instances[i].CodeSnippetId;
                PopulateMetrics(instances[i].MetricFeatures, row);
            }
        }

        private void PopulateMetricsHeader(List<Instance> instances)
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

        private void ExportDataSetWithHeuristics(List<Instance> dataSetInstances, string smell, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelFile = new ExcelPackage(new FileInfo(_dataSetWithHeuristicsTemplatePath));
            _sheet = _excelFile.Workbook.Worksheets[0];
            PopulateHeuristicsTemplate(dataSetInstances, smell);
            Serialize(fileName + "_Heuristics");
        }

        private void PopulateHeuristicsTemplate(List<Instance> instances, string smell)
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

        private void PopulateHeuristicsHeader(List<Instance> instances, string smell, List<Annotator> annotators)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinitionByName(smell);
            var smellHeuristics = _annotationSchemaRepository.GetCodeSmellDefinition(codeSmellDefinition.Id).Heuristics.ToList();
            var numOfHeuristics = smellHeuristics.Count();

            _sheet.Cells[1, 2, 1, 1 + (numOfHeuristics * annotators.Count)].Merge = true;
            for (int i = 0; i < annotators.Count(); i++)
            {
                _sheet.Cells[2, 2 + (numOfHeuristics * i), 2, 1 + (numOfHeuristics * i) + numOfHeuristics].Merge = true;
                _sheet.Cells[2, 2 + (numOfHeuristics * i)].Value = annotators[i].Id;
            }
        }

        private void PopulateHeuristics(List<Annotation> annotations, int row, List<Annotator> annotators, string smell)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinitionByName(smell);
            var smellHeuristics = _annotationSchemaRepository.GetCodeSmellDefinition(codeSmellDefinition.Id).Heuristics.ToList();
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

        private void PopulateHeuristicValues(List<HeuristicDefinition> smellHeuristics, List<SmellHeuristic> applicableHeuristics, int row, int annotationNum)
        {
            var numOfHeuristics = smellHeuristics.Count();
            applicableHeuristics = GetCodeSmellHeuristicsForExport(smellHeuristics, applicableHeuristics);

            for (var i = 0; i < smellHeuristics.Count(); i++)
            {
                _sheet.Cells[4, 2 + (numOfHeuristics * annotationNum) + i].Value = smellHeuristics[i].Name;
                _sheet.Cells[3, 2 + (numOfHeuristics * annotationNum), 3, 1 + (numOfHeuristics * annotationNum) + numOfHeuristics].Merge = true;
                _sheet.Cells[3, 2 + (numOfHeuristics * annotationNum)].Value = "Heuristics";
            }

            if (applicableHeuristics.Count == 0)
            {
                for (var i = 0; i < smellHeuristics.Count(); i++)
                {
                    _sheet.Cells[row, 2 + (numOfHeuristics * annotationNum) + i].Value = "FALSE";
                }
            }

            for (var i = 0; i < applicableHeuristics.Count(); i++)
            {
                _sheet.Cells[row, 2 + (numOfHeuristics * annotationNum) + i].Value = applicableHeuristics[i].IsApplicable;
            }
        }
        
        private List<SmellHeuristic> GetCodeSmellHeuristicsForExport(List<HeuristicDefinition> heuristics, List<SmellHeuristic> applicableHeuristics)
        {
            var heuristicsForExport = new List<SmellHeuristic>();
            foreach (var heuristic in heuristics)
            {
                foreach (var applicableHeur in applicableHeuristics)
                {
                    if (heuristic.Name.Equals(applicableHeur.Description)) {
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
