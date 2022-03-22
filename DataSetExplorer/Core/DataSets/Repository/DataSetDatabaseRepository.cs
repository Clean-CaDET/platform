using System.Collections.Generic;
using System.Linq;
using AutoMapper.Internal;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Infrastructure.Database;
using DataSetExplorer.UI.Controllers.Dataset.DTOs.Summary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public class DataSetDatabaseRepository : IDataSetRepository
    {
        private readonly DataSetExplorerContext _dbContext;

        public DataSetDatabaseRepository(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<DataSetExplorerContext>();
        }

        public void Create(DataSet dataSet)
        {
            _dbContext.Add(dataSet);
            _dbContext.SaveChanges();
        }

        public DatasetDetailDTO Get(int id)
        {
            var datasetSummary = GetDatasetSummary(id);
            var projectsSummary = GetProjectSummaries(id);
            if (datasetSummary == null || projectsSummary == null) return null;
            return new DatasetDetailDTO(datasetSummary.Id, datasetSummary.Name, datasetSummary.ProjectsCount, projectsSummary);
        }

        private List<ProjectSummaryDTO> GetProjectSummaries(int id)
        {
            var dataset = _dbContext.DataSets.FirstOrDefault(d => d.Id == id);
            if (dataset == null) return null;
            var projects = _dbContext.Entry(dataset).Collection(d => d.Projects).Query().ToList();
            
            var projectsSummary = new List<ProjectSummaryDTO>();
            projects.ForAll(p => projectsSummary.Add(GetProjectSummary(p)));
            return projectsSummary;
        }

        private ProjectSummaryDTO GetProjectSummary(DataSetProject project)
        {
            var projectInstancesCount = 0;
            var smellCandidates = _dbContext.Entry(project).Collection(d => d.CandidateInstances).Query().ToList();
            
            foreach (var candidate in smellCandidates)
            {
                projectInstancesCount += _dbContext.Entry(candidate).Collection(d => d.Instances).Query().Count();
            }
            return new ProjectSummaryDTO(project.Id, project.Name, project.Url, project.State.ToString(),
                projectInstancesCount);
        }

        public DataSet GetDataSetForExport(int id)
        {
            return _dbContext.DataSets
                .Include(d => d.SupportedCodeSmells)
                .Include(s => s.Projects).ThenInclude(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.Annotator)
                .Include(s => s.Projects).ThenInclude(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.ApplicableHeuristics)
                .Include(s => s.Projects).ThenInclude(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.InstanceSmell)
                .FirstOrDefault(s => s.Id == id);
        }

        public DataSet GetDataSetWithProjectsAndCodeSmells(int id)
        {
            return _dbContext.DataSets
                .Include(d => d.SupportedCodeSmells)
                .Include(s => s.Projects)
                .FirstOrDefault(d => d.Id == id);
        }

        public IEnumerable<DatasetSummaryDTO> GetAll()
        {
            var result = new List<DatasetSummaryDTO>();
            _dbContext.DataSets.ToList().ForEach(d => result.Add(GetDatasetSummary(d.Id)));
            return result;
        }

        private DatasetSummaryDTO GetDatasetSummary(int id)
        {
            var dataset = _dbContext.DataSets.FirstOrDefault(d => d.Id == id);
            if (dataset == null) return null;
            var projectsCount = _dbContext.Entry(dataset).Collection(d => d.Projects).Query().Count();
            return new DatasetSummaryDTO(dataset.Id, dataset.Name, projectsCount);
        }

        public DataSet Update(DataSet dataSet)
        {
            var updatedDataset = _dbContext.Update(dataSet).Entity;
            _dbContext.SaveChanges();
            return updatedDataset;
        }

        public Dictionary<string, List<string>> GetDataSetCodeSmells(int id)
        {
            var result = new Dictionary<string, List<string>>();
            var dataSet = _dbContext.DataSets.Include(d => d.SupportedCodeSmells).FirstOrDefault(s => s.Id == id);
            if (dataSet == null) return null;

            foreach (var smell in dataSet.SupportedCodeSmells)
            {
                result.Add(smell.Name, smell.RelevantSnippetTypes().Select(t => t.ToString()).ToList());
            }
            return result;
        }

        public DataSet Delete(int id)
        {
            var deletedDataset = _dbContext.DataSets.Remove(_dbContext.DataSets.Find(id)).Entity;
            _dbContext.SaveChanges();
            return deletedDataset;
        }
    }
}
