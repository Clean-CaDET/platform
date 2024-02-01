﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CodeModel.CodeParsers.CSharp.Exceptions;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.CommunityDetection.Model;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Core.DataSets.Repository;
using DataSetExplorer.Core.DataSetSerializer;
using DataSetExplorer.Core.DataSetSerializer.ViewModel;
using DataSetExplorer.Infrastructure.RepositoryAdapters;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using DataSetExplorer.UI.Controllers.Dataset.DTOs.Summary;
using FluentResults;
using LibGit2Sharp;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DataSetExplorer.Core.DataSets
{
    public class DataSetCreationService : IDataSetCreationService
    {
        private readonly ICodeRepository _codeRepository;
        private readonly IDataSetRepository _dataSetRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IConfiguration _configuration;

        public DataSetCreationService(ICodeRepository codeRepository, IDataSetRepository dataSetRepository, IProjectRepository projectRepository, IConfiguration configuration)
        {
            _codeRepository = codeRepository;
            _dataSetRepository = dataSetRepository;
            _projectRepository = projectRepository;
            _configuration = configuration;
        }

        public Result<DataSet> CreateEmptyDataSet(string dataSetName, List<CodeSmell> codeSmells)
        {
            var dataSet = new DataSet(dataSetName, codeSmells);
            _dataSetRepository.Create(dataSet);
            return Result.Ok(dataSet);
        }

        public Result<DataSetProject> AddProjectToDataSet(int dataSetId, string basePath, DataSetProject project, List<SmellFilter> smellFilters, ProjectBuildSettingsDTO projectBuildSettings)
        {
            var initialDataSet = _dataSetRepository.GetDataSetWithProjectsAndCodeSmells(dataSetId);
            if (initialDataSet == default) return Result.Fail($"DataSet with id: {dataSetId} does not exist.");

            Task.Run(() => ProcessInitialDataSetProject(basePath, project, initialDataSet.SupportedCodeSmells, smellFilters, projectBuildSettings));
            initialDataSet.AddProject(project);
            
            _dataSetRepository.Update(initialDataSet);
            return Result.Ok(project);
        }

        public Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects, List<CodeSmell> codeSmells)
        {
            return CreateDataSetSpreadsheet(dataSetName, basePath, projects, codeSmells, new NewSpreadSheetColumnModel());
        }

        public Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects, List<CodeSmell> codeSmells, NewSpreadSheetColumnModel columnModel)
        {
            var dataSet = new DataSet(dataSetName, codeSmells);
            foreach(var projectName in projects.Keys)
            {
                // TODO: Update console app and send metrics thresholds to CreateDataSetProject method
                var dataSetProject = CreateDataSetProject(basePath, projectName, projects[projectName], codeSmells, null, null);
                dataSet.AddProject(dataSetProject);
            }

            var excelFileName = ExportToExcel(basePath, dataSetName, columnModel, dataSet);
            return Result.Ok("Data set created: " + excelFileName);
        }

        public Result<DatasetDetailDTO> GetDataSet(int id)
        {
            var dataSet = _dataSetRepository.Get(id);
            if (dataSet == default) return Result.Fail($"DataSet with id: {id} does not exist.");
            return Result.Ok(dataSet);
        }

        public Result<IEnumerable<DatasetSummaryDTO>> GetAllDataSets()
        {
            var dataSets = _dataSetRepository.GetAll();
            return Result.Ok(dataSets);
        }

        public Result<IEnumerable<DataSet>> GetDataSetsByCodeSmell(string codeSmellName)
        {
            var dataSets = _dataSetRepository.GetAllByCodeSmell(codeSmellName);
            return Result.Ok(dataSets);
        }

        public Result<DataSetProject> GetDataSetProject(int id)
        {
            var project = _projectRepository.Get(id);
            if (project == default) return Result.Fail($"DataSetProject with id: {id} does not exist.");
            return Result.Ok(project);
        }

        private DataSetProject CreateDataSetProject(string basePath, string projectName, string projectAndCommitUrl, List<CodeSmell> codeSmells, List<SmellFilter> smellFilters, ProjectBuildSettingsDTO projectBuildSettings)
        {
            var gitFolderPath = basePath + projectName + Path.DirectorySeparatorChar + "git";
            _codeRepository.SetupRepository(projectAndCommitUrl, gitFolderPath);
            return CreateDataSetProjectFromRepository(projectAndCommitUrl, projectName, gitFolderPath, codeSmells, smellFilters, projectBuildSettings);
        }

        private static DataSetProject CreateDataSetProjectFromRepository(string projectAndCommitUrl, string projectName, string projectPath, List<CodeSmell> codeSmells, List<SmellFilter> smellFilters, ProjectBuildSettingsDTO projectBuildSettings)
        {
            //TODO: Introduce Director as a separate class and insert through DI.
            var builder = new CaDETToDataSetProjectBuilder(new InstanceFilter(smellFilters), projectAndCommitUrl, projectName, projectPath, projectBuildSettings.IgnoredFolders, codeSmells);
            if (projectBuildSettings.RandomizeClassSelection) builder = builder.RandomizeClassSelection();
            if (projectBuildSettings.RandomizeMemberSelection) builder = builder.RandomizeMemberSelection();
            if (projectBuildSettings.NumOfInstancesType.Equals("Percentage")) return builder.SetProjectExtractionPercentile(projectBuildSettings.NumOfInstances).Build();
            return builder.SetProjectInstancesExtractionNumber(projectBuildSettings.NumOfInstances).Build();
        }

        private void ProcessInitialDataSetProject(string basePath, DataSetProject initialProject, List<CodeSmell> codeSmells, List<SmellFilter> smellFilters, ProjectBuildSettingsDTO projectBuildSettings)
        {
            try
            {
                var project = CreateDataSetProject(basePath, initialProject.Name, initialProject.Url, codeSmells, smellFilters, projectBuildSettings);
                initialProject.CandidateInstances = project.CandidateInstances;
                initialProject.GraphInstances = project.GraphInstances;
                initialProject.Processed();
                _projectRepository.Update(initialProject);
            }
            catch (Exception e) when (e is LibGit2SharpException || e is NonUniqueFullNameException)
            {
                initialProject.Failed();
                _projectRepository.Update(initialProject);
            }
        }

        private string ExportToExcel(string basePath, string projectName, NewSpreadSheetColumnModel columnModel, DataSet dataSet)
        {
            var sheetFolderPath = basePath + projectName + Path.DirectorySeparatorChar + "sheets" + Path.DirectorySeparatorChar;
            if(!Directory.Exists(sheetFolderPath)) Directory.CreateDirectory(sheetFolderPath);
            var exporter = new NewDataSetExporter(sheetFolderPath, columnModel);

            var fileName = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
            exporter.Export(dataSet, fileName);
            return fileName;
        }

        public Result<List<CodeSmell>> GetDataSetCodeSmells(int id)
        {
            var codeSmells = _dataSetRepository.GetDataSetCodeSmells(id);
            if (codeSmells == default) return Result.Fail($"DataSet with id: {id} does not exist.");
            return Result.Ok(codeSmells);
        }

        public Result<DataSet> DeleteDataSet(int id)
        {
            var dataset = _dataSetRepository.Delete(id);
            return Result.Ok(dataset);
        }

        public Result<DataSet> UpdateDataSet(DataSet dataset)
        {
            var updatedDataset = _dataSetRepository.Update(dataset);
            return Result.Ok(updatedDataset);
        }

        public Result<DataSetProject> DeleteDataSetProject(int id)
        {
            var project = _projectRepository.Delete(id);
            return Result.Ok(project);
        }

        public Result<DataSetProject> UpdateDataSetProject(DataSetProject project)
        {
            var updatedProject = _projectRepository.Update(project);
            return Result.Ok(updatedProject);
        }

        public Result<Dictionary<string, int>> ExportCommunities(Graph Graph)
        {
            string python = _configuration.GetValue<string>("CommunityDetection:PythonPath");
            string script = _configuration.GetValue<string>("CommunityDetection:CommunityScriptPath");
            string nodesAndLinksPath = _configuration.GetValue<string>("CommunityDetection:CommunityNodesAndLinksPath");
            SerializeNodesAndLinks(Graph, nodesAndLinksPath);
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = python,
                Arguments = $"{script} {nodesAndLinksPath}/nodes.json {nodesAndLinksPath}/links.json {Graph.Algorithm}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            Process process = Process.Start(startInfo);
            StreamReader reader = process.StandardOutput;
            return Result.Ok(ExtractCommunitiesFromScript(reader));
        }

        private void SerializeNodesAndLinks(Graph Graph, string nodesAndLinksPath)
        {
            var nodes = new { nodes = Graph.Nodes };
            var links = new { links = Graph.Links };
            var nodesSerialized = JsonConvert.SerializeObject(nodes);
            var linksSerialized = JsonConvert.SerializeObject(links);
            File.WriteAllText($"{nodesAndLinksPath}/nodes.json", nodesSerialized);
            File.WriteAllText($"{nodesAndLinksPath}/links.json", linksSerialized);
        }

        private Dictionary<string, int> ExtractCommunitiesFromScript(StreamReader reader)
        {
            Dictionary<string, int> communities = new Dictionary<string, int>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                int idx = line.LastIndexOf(" ");
                if (idx != -1)
                {
                    try
                    {
                        communities.Add(line[..idx].ToString(), int.Parse(line[(idx + 1)..].ToString()));
                    }
                    catch (Exception e) {}
                }
            }
            return communities;
        }
    }
}
