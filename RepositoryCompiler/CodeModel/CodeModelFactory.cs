using RepositoryCompiler.CodeModel.CaDETModel;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompiler.CodeModel.CodeParsers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace RepositoryCompiler.CodeModel
{
    public class CodeModelFactory
    {
        private readonly LanguageEnum _language;

        public CodeModelFactory(LanguageEnum language)
        {
            _language = language;
        }

        public List<CaDETClass> CreateClassModel(IEnumerable<string> multipleClassSourceCode)
        {
            ICodeParser codeParser = SimpleParserFactory.CreateParser(_language);
            return codeParser.GetParsedClasses(multipleClassSourceCode);
        }

        public CaDETClass CreateClassModel(string classSourceCode)
        {
            return CreateClassModel(new List<string> { classSourceCode }).First();
        }

        public CaDETProject CreateProject(string sourceCodeLocation)
        {
            var allFiles = GetCodeFiles(sourceCodeLocation);
            return new CaDETProject(LanguageEnum.CSharp, CreateClassModel(allFiles.Select(File.ReadAllText)));
        }

        private string[] GetCodeFiles(string sourceCodeLocation)
        {
            return Directory.GetFiles(sourceCodeLocation, GetLanguageExtension(), SearchOption.AllDirectories);
        }

        public CaDETProject CreateProjectWithCodeFileLinks(string sourceCodeLocation)
        {
            var project = CreateProject(sourceCodeLocation);
            project.CodeLinks = PopulateCodeLinks(sourceCodeLocation, project.Classes);
            return project;
        }

        public Dictionary<string, CodeLocationLink> PopulateCodeLinks(string basePath, List<CaDETClass> projectClasses)
        {
            var codeLinks = new Dictionary<string, CodeLocationLink>();

            var allFiles = GetCodeFiles(basePath);
            foreach (var file in allFiles)
            {
                var fileText = File.ReadAllText(file);
                var classesInFile = GetClassesInFile(projectClasses, fileText);
                if(classesInFile.Count == 0) continue;

                var relativePath = GetRelativePath(basePath, file);
                foreach (var c in classesInFile)
                {
                    codeLinks.Add(c.FullName, GetSnippetLocationLink(c.SourceCode, fileText, relativePath));
                    foreach (var member in c.Members)
                    {
                        codeLinks.Add(member.Signature(), GetSnippetLocationLink(member.SourceCode, fileText, relativePath));
                    }
                }
            }

            return codeLinks;
        }

        private List<CaDETClass> GetClassesInFile(List<CaDETClass> projectClasses, string fileText)
        {
            return projectClasses.Where(projectClass => FileContainsOuterMostClass(projectClass, fileText)).ToList();
        }

        private bool FileContainsOuterMostClass(CaDETClass projectClass, string fileText)
        {
            var c = projectClass;
            while(c.IsInnerClass) c = c.OuterClass;
            if (c.ContainerName.Equals("")) return fileText.Contains(c.SourceCode); //When namespace is not defined (global).
            var namespaceName = GetLanguagePackageName() + " " + c.ContainerName;
            return fileText.Contains(c.SourceCode)
                   && fileText.Contains(namespaceName)
                   && !fileText.Contains(namespaceName + "."); //To avoid subpackages.
        }

        private static string GetRelativePath(string basePath, string fullPath)
        {
            return fullPath.Replace(basePath, "");
        }

        private static CodeLocationLink GetSnippetLocationLink(string sourceCode, string fileText, string relativePath)
        {
            var textBeforeCode = fileText.Split(sourceCode)[0];
            var startLine = CountLines(textBeforeCode);
            return new CodeLocationLink(relativePath, startLine, startLine + CountLines(sourceCode) - 1);
        }

        private static int CountLines(string text)
        {
            return text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
        }

        private string GetLanguageExtension()
        {
            return _language switch
            {
                LanguageEnum.CSharp => "*.cs",
                _ => throw new InvalidEnumArgumentException()
            };
        }

        private string GetLanguagePackageName()
        {
            return _language switch
            {
                LanguageEnum.CSharp => "namespace",
                _ => throw new InvalidEnumArgumentException()
            };
        }
    }
}
