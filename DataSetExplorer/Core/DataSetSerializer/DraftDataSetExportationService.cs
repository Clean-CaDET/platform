using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.AnnotationSchema.Model;
using DataSetExplorer.Core.AnnotationSchema.Repository;
using DataSetExplorer.Core.DataSets.Model;
using OfficeOpenXml;

namespace DataSetExplorer.Core.DataSetSerializer
{
    public class DraftDataSetExportationService : IDraftDataSetExportationService
    {
        private readonly string _templatePath = "./Core/DataSetSerializer/Template/New_Dataset_Template.xlsx";
        private string _exportPath;
        private ExcelPackage _excelFile;
        private ExcelWorksheet _sheet;
        private readonly IAnnotationSchemaRepository _annotationSchemaRepository;

        public DraftDataSetExportationService(IAnnotationSchemaRepository annotationSchemaRepository)
        {
            _annotationSchemaRepository = annotationSchemaRepository;
        }

        public string Export(string exportPath, int annotatorId, DataSet dataSet)
        {
            _exportPath = exportPath + dataSet.Name + "/";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            foreach (var project in dataSet.Projects)
            {
                CreateExcelFile(project);
                PopulateSheets(annotatorId, project);
            }
            return _exportPath;
        }

        private void CreateExcelFile(DataSetProject project)
        {
            _excelFile = new ExcelPackage(new FileInfo(_templatePath));
            for (var i = 0; i < project.CandidateInstances.Count - 1; i++)
            {
                _excelFile.Workbook.Worksheets.Add(i.ToString(), _excelFile.Workbook.Worksheets[0]);
            }
        }

        private void PopulateSheets(int annotatorId, DataSetProject project)
        {
            var i = 0;
            foreach (var candidate in project.CandidateInstances)
            {
                _sheet = _excelFile.Workbook.Worksheets[i];
                PopulateBasicInfo(annotatorId, project, candidate);
                var smellHeuristics = PopulateSmellHeuristics(candidate);
                PopulateAnnotatedInstances(candidate, smellHeuristics);
                Serialize(project.Name + annotatorId);
                i++;
            }
        }

        private void PopulateBasicInfo(int annotatorId, DataSetProject project, SmellCandidateInstances candidate)
        {
            _sheet.Name = candidate.CodeSmell.Name;
            _sheet.Cells[2, 1].Value = project.Url;
            _sheet.Cells[2, 2].Value = candidate.CodeSmell.Name;
            _sheet.Cells[2, 3].Value = annotatorId;
        }

        private List<HeuristicDefinition> PopulateSmellHeuristics(SmellCandidateInstances candidate)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinitionByName(candidate.CodeSmell.Name);
            var smellHeuristics = _annotationSchemaRepository.GetCodeSmellDefinition(codeSmellDefinition.Id).Heuristics.ToList();
            for (var i = 0; i < smellHeuristics.Count; i++)
            {
                _sheet.Cells[2, 4 + (2 * i)].Value = smellHeuristics[i].Name;
            }
            _sheet.Cells[3, 4 + (smellHeuristics.Count * 2)].Value = "Note";
            return smellHeuristics;
        }

        private void PopulateAnnotatedInstances(SmellCandidateInstances candidate, List<HeuristicDefinition> smellHeuristics)
        {
            var row = 4;
            foreach (var instance in candidate.Instances)
            {
                if (instance.Annotations.Count == 0) continue;
                _sheet.Cells[row, 1].Value = instance.CodeSnippetId;
                _sheet.Cells[row, 2].Value = instance.Link;
                PopulateAnnotations(smellHeuristics, row, instance);
                row++;
            }
        }

        private void PopulateAnnotations(List<HeuristicDefinition> smellHeuristics, int row, Instance instance)
        {
            foreach (var annotation in instance.Annotations)
            {
                _sheet.Cells[row, 3].Value = annotation.Severity;
                foreach (var applicableHeuristic in annotation.ApplicableHeuristics)
                {
                    var index = smellHeuristics.FindIndex(h => h.Name.Equals(applicableHeuristic.Description));
                    _sheet.Cells[3, 4 + (2 * index)].Value = "Applicable?";
                    _sheet.Cells[row, 4 + (2 * index)].Value = "Yes";
                    _sheet.Cells[3, 4 + (2 * index) + 1].Value = "Reasoning";
                    _sheet.Cells[row, 4 + (2 * index) + 1].Value = applicableHeuristic.ReasonForApplicability;
                    
                }
                PopulateNotApplicableAnnotations(smellHeuristics, row, annotation);
                _sheet.Cells[row, 4 + (smellHeuristics.Count * 2)].Value = annotation.Note;
            }
        }

        private void PopulateNotApplicableAnnotations(List<HeuristicDefinition> smellHeuristics, int row, Annotation annotation)
        {
            var notApplicableHeuristics =
                smellHeuristics.ConvertAll(h => h.Name).Except(annotation.ApplicableHeuristics.ConvertAll(h => h.Description));
            foreach (var heuristic in notApplicableHeuristics)
            {
                var index = smellHeuristics.FindIndex(h => h.Name.Equals(heuristic));
                _sheet.Cells[row, 4 + (2 * index)].Value = "No";
                _sheet.Cells[3, 4 + (2 * index)].Value = "Applicable?";
                _sheet.Cells[3, 4 + (2 * index) + 1].Value = "Reasoning";
            }
        }

        private void Serialize(string fileName)
        {
            _exportPath = Directory.CreateDirectory(_exportPath).FullName;
            var filePath = _exportPath + fileName + ".xlsx";
            _excelFile.SaveAs(new FileInfo(filePath));
        }
    }
}
