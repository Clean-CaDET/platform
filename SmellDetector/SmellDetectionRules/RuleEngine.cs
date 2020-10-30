using SmellDetector.Controllers;

namespace SmellDetector.SmellDetectionRules
{
    public interface RuleEngine
    {
        public bool isBadSmell(MetricsDTO metrics);

    }
} 

