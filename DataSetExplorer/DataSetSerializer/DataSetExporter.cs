using System.Collections.Generic;
using System.IO;
using DataSetExplorer.DataSetBuilder.Model;
using OfficeOpenXml;

namespace DataSetExplorer.DataSetSerializer
{
    class DataSetExporter
    {
        private readonly string _templatePath = "../../../DataSetSerializer/Template/Dataset Template.xlsx";
        private readonly string _exportPath;
        private ExcelPackage template;
        private ExcelWorksheet sheet;

        public DataSetExporter(string exportPath)
        {
            _exportPath = exportPath;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            template = new ExcelPackage(new FileInfo(_templatePath));
            sheet = template.Workbook.Worksheets[0];
        }

        public void Export(List<ExportedDataSetInstance> dataSetInstances, string fileName)
        {
            PopulateTemplate(dataSetInstances);
            Serialize(fileName);
        }

        private void PopulateTemplate(List<ExportedDataSetInstance> instances)
        {
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 2 + i;
                sheet.Cells[row, 1].Value = instances[i].CodeSnippetId;
                sheet.Cells[row, 2].Value = instances[i].CodeSnippetLink;
                sheet.Cells[row, 3].Value = instances[i].CodeSmellType;
                sheet.Cells[row, 4].Value = instances[i].ProjectLink;

                PopulateMetrics(instances[i], row);
                PopulateAnnotations(instances[i], row);
            }
        }
        
        private void PopulateMetrics(ExportedDataSetInstance instance, int row)
        {
            var i = 0;
            foreach (var key in instance.Metrics.Keys)
            {
                sheet.Cells[1, 5 + i].Value = key;
                sheet.Cells[row, 5 + i].Value = instance.Metrics[key];
                i++;
            }
        }

        private void PopulateAnnotations(ExportedDataSetInstance instance, int row)
        {
            var numOfMetrics = instance.Metrics.Count;
            var maxNumberOfAnnotations = ExportedDataSetInstance.MaxNumberOfAnnotations;

            for (var i = 0; i < maxNumberOfAnnotations; i++)
            {
                var annotatorId = i + 1;
                sheet.Cells[1, 5 + numOfMetrics + i].Value = "Annotator " + annotatorId;

                if (instance.IsAnnotatedBy(annotatorId))
                {
                    sheet.Cells[row, 5 + numOfMetrics + i].Value = instance.GetAnnotationFromAnnotator(annotatorId);
                }
                else
                {
                    sheet.Cells[row, 5 + numOfMetrics + i].Value = "/";
                }
            }
            
            sheet.Cells[1, 5 + numOfMetrics + maxNumberOfAnnotations].Value = "Final annotation";
            sheet.Cells[row, 5 + numOfMetrics + maxNumberOfAnnotations].Value = GetFinalAnnotation(instance);
        }

        private static int GetFinalAnnotation(ExportedDataSetInstance instance)
        {
            if (instance.IsMaxAnnotated()) return instance.GetMajorityAnnotation();
            if (instance.IsAnnotatedByMostExperienced()) return instance.GetAnnotationFromMostExperiencedAnnotator();
            return instance.GetAnnotationFromAnnotator(2);
        }

        private void Serialize(string fileName)
        {
            var filePath = _exportPath + fileName + ".xlsx";
            template.SaveAs(new FileInfo(filePath));
        }
    }
}
