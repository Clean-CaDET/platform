using SmartTutor.ContentModel.LearningObjects.ChallengeModel;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.MetricChecker;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.MetricHints;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.Repository
{
    public class LearningObjectInMemoryRepository : ILearningObjectRepository
    {
        //Key is mapped to LearningObjectSummaryId.
        private readonly Dictionary<int, List<LearningObject>> _learningObjectCache;

        public LearningObjectInMemoryRepository()
        {
            //TODO: Will be updated from DB on App startup.
            _learningObjectCache = new Dictionary<int, List<LearningObject>>();
            _learningObjectCache.Add(111, new List<LearningObject>
            {
                new Text
                {
                    Id = 1111, LearningObjectSummaryId = 111,
                    Content =
                        "Naming is the process of determining and assigning identifier names. Identifiers are everywhere in code. We name files, folders, classes, methods, and variables. In Java we name packages and JARs, while C# requires naming namespaces and DLLs. A good name reveals the purpose of a thing – why it exists, what does it do, and how it is used."
                }
            });
            _learningObjectCache.Add(112, new List<LearningObject>
            {
                new Image {Id = 1121, LearningObjectSummaryId = 112, Url = "https://i.ibb.co/7tzVd6Z/simple-names.png"}
            });
            _learningObjectCache.Add(121, new List<LearningObject>
            {
                new Text
                {
                    Id = 1211, LearningObjectSummaryId = 121,
                    Content =
                        "When naming things, consider the following steps:\n1. Describe the thing in a sentence or two (easier to do in Serbian, then translate). This would be the text you would write in a comment that describes the thing.\n2. Remove any words that are not crucial to convey the meaning.\n3. Remove any words that can be inferred from the Type, containing module, function parameters, etc.\n4. See if what remains sufficiently describes the thing.\n5. Rename."
                }
            });


            _learningObjectCache.Add(331, new List<LearningObject>
            {
                new Text
                {
                    Id = 3311, LearningObjectSummaryId = 331,
                    Content =
                        "Cohesion determines the degree to which a part of a codebase forms a meaningful atomic module. The elements of a highly cohesive module work together towards a common, well-defined goal and have a clear (single) responsibility. This responsibility is defined by the module’s name and described by its interface that sets its inputs and outputs."
                }
            });

            _learningObjectCache.Add(332, new List<LearningObject>
            {
                new Image
                {
                    Id = 3321, LearningObjectSummaryId = 332,
                    Url = "https://miro.medium.com/max/2400/1*3jfye6OQFu_dROKb14BhaQ.png",
                    Caption =
                        "The left class is playing with a few responsibilities, more than its name suggests anyway…"
                }
            });

            _learningObjectCache.Add(333, new List<LearningObject>
            {
                new Text
                {
                    Id = 3331, LearningObjectSummaryId = 333,
                    Content =
                        "Structural cohesion is a metric that is calculated based on the number of connections between a module’s elements."
                }
            });

            _learningObjectCache.Add(334, new List<LearningObject>
            {
                new Image
                {
                    Id = 3341, LearningObjectSummaryId = 334,
                    Url = "https://miro.medium.com/max/700/1*OF4xmCDnuV_VRDcqiLp46Q.png",
                    Caption =
                        "How does this formula hold for data transfer object classes? What about classes without fields?"
                }
            });

            _learningObjectCache.Add(335, new List<LearningObject>
            {
                new Text
                {
                    Id = 3351, LearningObjectSummaryId = 335,
                    Content =
                        "Semantic cohesion determines the degree to which the elements of a module are semantically related."
                }
            });

            _learningObjectCache.Add(336, new List<LearningObject>
            {
                new Video
                {
                    Id = 3361, LearningObjectSummaryId = 336, Url = "https://www.youtube.com/watch?v=qE-Gmu_YuQE"
                }
            });

            AddChallenge();

        }

        public List<LearningObject> GetLearningObjectsForSummary(int summaryId)
        {
            return _learningObjectCache[summaryId];
        }

        public List<LearningObject> GetFirstLearningObjectsForSummaries(List<int> summaries)
        {
            return summaries.Select(id => _learningObjectCache[id].First()).ToList();
        }

        public Challenge GetChallenge(int challengeId)
        {
            return _learningObjectCache.SelectMany(LOs => LOs.Value.Where(LO => LO.Id == challengeId)).First() as Challenge;
        }

        private void AddChallenge()
        {
            List<MetricRangeRule> classMetricRules = new List<MetricRangeRule>();
            classMetricRules.Add(new MetricRangeRule { MetricName = "CLOC", FromValue = 3, ToValue = 30 });
            classMetricRules.Add(new MetricRangeRule { MetricName = "NMD", FromValue = 0, ToValue = 2 });

            List<MetricRangeRule> methodMetricRules = new List<MetricRangeRule>();
            methodMetricRules.Add(new MetricRangeRule { MetricName = "MELOC", FromValue = 2, ToValue = 5 });
            methodMetricRules.Add(new MetricRangeRule { MetricName = "NOP", FromValue = 1, ToValue = 4 });

            _learningObjectCache.Add(337, new List<LearningObject>
            {
                new Challenge
                {
                    Id = 3371,
                    LearningObjectSummaryId = 337,
                    Url = "https://github.com/Ana00000/Challenge-inspiration.git",
                    FulfillmentStrategy = new BasicMetricsChecker(classMetricRules, methodMetricRules)
                }
            });
        }

        public List<QuestionAnswer> GetQuestionAnswers(int questionId)
        {
            throw new System.NotImplementedException();
        }
    }
}