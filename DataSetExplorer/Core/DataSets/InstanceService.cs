using AutoMapper;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Core.DataSets.Repository;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using FluentResults;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace DataSetExplorer.Core.DataSets
{
    public class InstanceService : IInstanceService
    {
        private readonly IInstanceRepository _instanceRepository;
        private readonly IProjectRepository _projectRepository;

        public InstanceService(IMapper mapper, IInstanceRepository instanceRepository, IProjectRepository projectRepository)
        {
            _instanceRepository = instanceRepository;
            _projectRepository = projectRepository;
        }

        public Result<InstanceDTO> GetInstanceWithRelatedInstances(int id)
        {
            var instance = _instanceRepository.GetInstanceWithRelatedInstances(id);
            if (instance == default) return Result.Fail($"Instance with id: {id} does not exist.");
            return Result.Ok(instance);
        }

        public Result<Instance> GetInstanceWithAnnotations(int id)
        {
            var instance = _instanceRepository.GetInstanceWithAnnotations(id);
            if (instance == default) return Result.Fail($"Instance with id: {id} does not exist.");
            return Result.Ok(instance);
        }

        public string GetFileFromGit(string url)
        {
            string rawUrl = "https://raw.githubusercontent.com/" + url.Split("https://github.com/")[1];
            rawUrl = rawUrl.Replace("/tree", "");

            var client = new HttpClient();
            using HttpResponseMessage response = client.GetAsync(rawUrl).Result;
            using HttpContent content = response.Content;
            var data = content.ReadAsStringAsync().Result;

            return data;
        }
    }
}