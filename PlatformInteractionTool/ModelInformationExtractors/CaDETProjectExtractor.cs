using System.IO;
using System.Linq;
using System.Text;
using RepositoryCompiler.CodeModel;
using RepositoryCompiler.CodeModel.CaDETModel;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;

namespace PlatformInteractionTool.ModelInformationExtractors
{
    class CaDETProjectExtractor
    {
        private readonly CaDETProject _project;
        private readonly string _resultFolder;

        public CaDETProjectExtractor(string projectPath) : this(projectPath, LanguageEnum.CSharp) {}

        public CaDETProjectExtractor(string projectPath, LanguageEnum language)
        {
            _project = new CodeModelBuilder(language).ParseFiles(projectPath);
            _resultFolder = projectPath + "extraction-results\\";
        }

        public void ExtractNamesToFile()
        {
            SaveClassNamesToFile();
            SaveMethodAndConstructorNamesToFile();
        }

        private void SaveClassNamesToFile()
        {
            var sb = new StringBuilder();
            foreach (var projectClass in _project.Classes)
            {
                sb.Append(projectClass.FullName).Append("\n");
            }
            WriteToFile(sb.ToString(), "classNames.txt");
        }
        private void SaveMethodAndConstructorNamesToFile()
        {
            var sb = new StringBuilder();
            foreach (var projectClass in _project.Classes)
            {
                foreach (var member in projectClass.Members.Where(
                    m => m.Type.Equals(CaDETMemberType.Method) || m.Type.Equals(CaDETMemberType.Constructor)))
                {
                    sb.Append(member.GetSignature()).Append("\n");
                }
            }
            WriteToFile(sb.ToString(), "methods.txt");
        }

        private void WriteToFile(string text, string fileName)
        {
            File.WriteAllText(_resultFolder + fileName, text);
        }

        private void SaveWholeProjectToFile()
        {
            var sb = new StringBuilder();
            foreach (var projectClass in _project.Classes)
            {
                sb.Append("\n\n").Append(projectClass.FullName).Append("\n");
                sb.Append("Parent: ").Append(projectClass.Parent).Append("\n");
                sb.Append("FIELDS\n");
                foreach (var field in projectClass.Fields)
                {
                    sb.Append("  ").Append(field.Name).Append(",");
                }
                sb.Append("\n\nMETHODS\n");
                foreach (var member in projectClass.Members)
                {
                    sb.Append("\n  ").Append(member).Append("\n  INVOKED METHODS\n");
                    foreach (var im in member.InvokedMethods)
                    {
                        sb.Append("    ").Append(im).Append("\n");
                    }

                    sb.Append("  ACCESSED FIELDS\n");
                    foreach (var im in member.AccessedFields)
                    {
                        sb.Append("    ").Append(im).Append("\n");
                    }
                }
            }
            WriteToFile(sb.ToString(), "whole.txt");
        }
    }
}