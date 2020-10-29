using SmellDetector.Controllers;
using SmellDetector.SmellDetectionRules;

namespace SmellDetector.Services
{
    public class DetectionService
    {
        public DetectionService() { }

        public SmellType classCheck(MetricsDTO metrics)
        {
            // TODO: Add check for class
            return SmellType.WITHOUT_BAD_SMELL;
        }

        public SmellType functionCheck(MetricsDTO metrics)
        {
            LongMethod longMethod = new LongMethod();
            LongParamLists longParam = new LongParamLists();

            if (longMethod.isBadSmell(metrics)) return SmellType.LONG_METHOD;
            if (longParam.isBadSmell(metrics)) return SmellType.LONG_PARAM_LISTS;

            return SmellType.WITHOUT_BAD_SMELL;
        }

    }
}



