using CodeModel.CaDETModel;
using CodeModel.CaDETModel.CodeItems;
using CodeModel.CodeParsers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace CodeModel
{
    public class CodeModelFactory
    {
        private readonly LanguageEnum _language;
        private readonly bool _includePartial;

        public CodeModelFactory(LanguageEnum language = LanguageEnum.CSharp, bool includePartial = false)
        {
            _language = language;
            _includePartial = includePartial;
        }

        public CaDETProject CreateProject(IEnumerable<string> multipleClassSourceCode)
        {
            ICodeParser codeParser = SimpleParserFactory.CreateParser(_language);
            return _includePartial ? codeParser.ParseWithPartial(multipleClassSourceCode) : codeParser.Parse(multipleClassSourceCode);
        }

        public CaDETProject CreateProject(string sourceCodeLocation, List<string> ignoredFolders)
        {
            var allFiles = GetCodeFiles(sourceCodeLocation, ignoredFolders);
            return CreateProject(allFiles.Select(File.ReadAllText));
        }

        public CaDETProject CreateProject(string sourceCodeLocation)
        {
            return CreateProject(sourceCodeLocation, new List<string>());
        }

        private string[] GetCodeFiles(string sourceCodeLocation, List<string> ignoredFolders)
        {
            var allFiles = Directory.GetFiles(sourceCodeLocation, GetLanguageExtension(), SearchOption.AllDirectories).ToList();
            if (ignoredFolders != null) ignoredFolders.ForEach(folder => allFiles.RemoveAll(f => f.ToLower().Contains("\\" + folder.ToLower() + "\\")));
            if (ignoredFolders != null) ignoredFolders.ForEach(folder => allFiles.RemoveAll(f => f.ToLower().Contains("/" + folder.ToLower() + "/")));
            return allFiles.ToArray();
        }
        
        public CaDETProject CreateProjectWithCodeFileLinks(string sourceCodeLocation, List<string> ignoredFolders)
        {
            var project = CreateProject(sourceCodeLocation, ignoredFolders);
            project.CodeLinks = PopulateCodeLinks(sourceCodeLocation, project.Classes, ignoredFolders);
            return project;
        }
        
        public CaDETProject CreateProjectWithCodeFileLinks(string repoFolder)
        {
            return CreateProjectWithCodeFileLinks(repoFolder, new List<string>());
        }

        public Dictionary<string, CodeLocationLink> PopulateCodeLinks(string basePath, List<CaDETClass> projectClasses, List<string> ignoredFolders)
        {
            var codeLinks = new Dictionary<string, CodeLocationLink>();

            var allFiles = GetCodeFiles(basePath, ignoredFolders);
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
            return fullPath.Replace(basePath, "").Replace("\\", "/");
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
