using System;

namespace RepositoryCompiler.Controllers.DTOs
{
    public class ClassQualityAnalysisResponse
    {
        public Guid Id { get; set; }
        public ClassMetricsDTO Metrics { get; set; }
        public EducationalContentDTO Content { get; set; }
    }
}
