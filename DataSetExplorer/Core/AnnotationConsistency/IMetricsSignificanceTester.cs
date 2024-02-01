﻿using System.Collections.Generic;
using DataSetExplorer.Core.DataSets.Model;
using FluentResults;

namespace DataSetExplorer.Core.AnnotationConsistency
{
    internal interface IMetricsSignificanceTester
    {
        public Result<Dictionary<string, Dictionary<string, string>>> TestForSingleAnnotator(int annotatorId, List<SmellCandidateInstances> instancesGroupedBySmells);

        public Result<Dictionary<string, Dictionary<string, string>>> TestBetweenAnnotators(string severity, List<SmellCandidateInstances> instancesGroupedBySmells);
    }
}