using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public abstract class ChallengeFulfillmentStrategy
    {
        [Key] public int Id { get; set; }

        public abstract HintDirectory EvaluateSubmission(List<CaDETClass> solutionAttempt);
        public abstract List<ChallengeHint> GetAllHints();

        protected List<CaDETMember> GetMethodsFromClasses(List<CaDETClass> classes)
        {
            return classes.SelectMany(c => c.Members.Where(m => m.Type.Equals(CaDETMemberType.Method))).ToList();
        }

        protected List<CaDETField> GetFieldsFromClasses(List<CaDETClass> classes)
        {
            return classes.SelectMany(c => c.Fields).ToList();
        }

        protected List<CaDETMember> GetMembersFromClasses(List<CaDETClass> classes)
        {
            return classes.SelectMany(c => c.Members).ToList();
        }

        protected List<string> GetVariableNamesFromClasses(List<CaDETClass> classes)
        {
            return classes.SelectMany(c => c.Members).SelectMany(m => m.VariableNames).ToList();
        }

        protected List<CaDETParameter> GetParametersFromClasses(List<CaDETClass> classes)
        {
            return classes.SelectMany(c => c.Members).SelectMany(m => m.Params).ToList();
        }
    }
}
