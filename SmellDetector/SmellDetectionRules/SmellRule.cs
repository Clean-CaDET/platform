using SmellDetector.Controllers;

namespace SmellDetector.SmellDetectionRules
{
    public interface SmellRule
    {
        public bool isBadSmell(MetricsDTO metrics);

    }
} 

