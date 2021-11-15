using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer.ViewModel;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataSetExplorer.DataSetSerializer
{
    class DraftDataSetExporter
    {
        private readonly string _templatePath = "./DataSetSerializer/Template/New_Dataset_Template.xlsx";
        private string _exportPath;
        private ExcelPackage _excelFile;
        private ExcelWorksheet _sheet;
        private readonly ColumnHeuristicsModel _requiredSmells = new();

        public DraftDataSetExporter(string exportPath)
        {
            _exportPath = exportPath;
        }

        public string Export(int annotatorId, DataSet dataSet)
        {
            _exportPath += dataSet.Name + "/";
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

        private List<string> PopulateSmellHeuristics(SmellCandidateInstances candidate)
        {
            var smellHeuristics = _requiredSmells.GetHeuristicsByCodeSmellName(candidate.CodeSmell.Name);
            for (var i = 0; i < smellHeuristics.Count; i++)
            {
                _sheet.Cells[2, 4 + (2 * i)].Value = smellHeuristics[i];
            }
            return smellHeuristics;
        }

        private void PopulateAnnotatedInstances(SmellCandidateInstances candidate, List<string> smellHeuristics)
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

        private void PopulateAnnotations(List<string> smellHeuristics, int row, Instance instance)
        {
            foreach (var annotation in instance.Annotations)
            {
                _sheet.Cells[row, 3].Value = annotation.Severity;
                foreach (var applicableHeuristic in annotation.ApplicableHeuristics)
                {
                    var index = smellHeuristics.FindIndex(h => h.Equals(applicableHeuristic.Description));
                    _sheet.Cells[row, 4 + (2 * index)].Value = "Yes";
                }
                PopulateNotApplicableAnnotations(smellHeuristics, row, annotation);
            }
        }

        private void PopulateNotApplicableAnnotations(List<string> smellHeuristics, int row, Annotation annotation)
        {
            var notApplicableHeuristics =
                smellHeuristics.Except(annotation.ApplicableHeuristics.ConvertAll(h => h.Description));
            foreach (var heuristic in notApplicableHeuristics)
            {
                var index = smellHeuristics.FindIndex(h => h.Equals(heuristic));
                _sheet.Cells[row, 4 + (2 * index)].Value = "No";
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
