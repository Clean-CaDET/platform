using System.Collections.Generic;
using System.Linq;
using CodeModel.CaDETModel.CodeItems;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer
{
    public class CohesionAnalyzer
    {
        public List<CohesiveParts> IdentifyCohesiveParts(CaDETClass parsedClass)
        {
            ClassInteractions classInteractions = new ClassInteractions(parsedClass);

            IEnumerable<CohesiveParts> possibleParts = GetAllPossibleParts(classInteractions);

            return possibleParts.Where(cohesiveParts =>
                    cohesiveParts.Parts.TrueForAll(set => CalculateLCOM(set) < 0.4))
                .ToList();
        }

        private IEnumerable<CohesiveParts> GetAllPossibleParts(ClassInteractions classInteractions)
        {
            var accessesThatCannotBeCut = classInteractions.GetAccessesThatCannotBeCut();
            var accessesToBeCut = GetAccessesThatCanBeCut(classInteractions.Accesses, accessesThatCannotBeCut);
            var possibleParts = new List<CohesiveParts>();
            foreach (var accesses in accessesToBeCut)
            {
                if (possibleParts.Any(s => accesses.IsProperSupersetOf(s.AccessesToCut))) continue;
                var parts = FindValidCohesiveParts(accesses, new HashSet<Access>(classInteractions.Accesses));
                if (parts is not { Count: 2 }) continue;
                var cohesiveParts = new CohesiveParts(accesses, parts);
                possibleParts.RemoveAll(s => s.AccessesToCut.IsProperSupersetOf(cohesiveParts.AccessesToCut));
                possibleParts.Add(cohesiveParts);
            }

            return possibleParts;
        }

        private IEnumerable<HashSet<Access>> GetAccessesThatCanBeCut(HashSet<Access> allAccesses,
            IEnumerable<HashSet<Access>> accessesThatCannotBeCut)
        {
            var data = allAccesses.ToArray();
            if (data.Length == 0) return new List<HashSet<Access>>();

            var allAccessesCombinations = Enumerable
                .Range(0, 1 << (data.Length / 2 + 1))
                .Select(index => data
                    .Where((v, i) => (index & (1 << i)) != 0)
                    .ToHashSet());

            return allAccessesCombinations.Where(accesses =>
                !accessesThatCannotBeCut.Any(accesses.IsSupersetOf)
            );
        }

        private List<HashSet<Access>> FindValidCohesiveParts(HashSet<Access> accessesToCut, HashSet<Access> accesses)
        {
            List<HashSet<Access>> result = new List<HashSet<Access>>();
            accesses.ExceptWith(accessesToCut);

            var lastGroup = new HashSet<Access>();
            var foundAccesses = FindConnectedAccesses(accesses);
            while (foundAccesses != null)
            {
                result.Add(foundAccesses);
                if (result.Count == 2) break;
                lastGroup = accesses;
                accesses.ExceptWith(foundAccesses);
                foundAccesses = FindConnectedAccesses(accesses);
            }

            result.Add(lastGroup);

            return result;
        }

        private HashSet<Access> FindConnectedAccesses(IReadOnlyCollection<Access> accessesToCheck)
        {
            if (accessesToCheck.Count == 0) return null;
            var foundAccesses = SearchForConnectedAccesses(accessesToCheck);
            return foundAccesses.Count != accessesToCheck.Count ? foundAccesses : null;
        }

        private HashSet<Access> SearchForConnectedAccesses(IReadOnlyCollection<Access> accesses)
        {
            List<int> visitedMethods = new List<int>();
            List<int> visitedFields = new List<int>();
            HashSet<Access> collectedAccesses = new HashSet<Access>();
            HashSet<Access> accessesToVisit = new HashSet<Access> { accesses.First() };

            while (accessesToVisit.Count != 0)
            {
                HashSet<Access> accessesForNextIteration = new HashSet<Access>();
                foreach (var access in accessesToVisit)
                {
                    if (!visitedMethods.Contains(access.Method))
                    {
                        List<Access> accessesForMethod = accesses.Where(acs => acs.Method == access.Method).ToList();
                        accessesForMethod.ForEach(e => accessesForNextIteration.Add(e));
                        visitedMethods.Add(access.Method);
                    }

                    if (visitedFields.Contains(access.Field)) continue;
                    List<Access> accessesForField = accesses.Where(acs => acs.Field == access.Field).ToList();
                    accessesForField.ForEach(e => accessesForNextIteration.Add(e));
                    visitedFields.Add(access.Field);
                }

                foreach (var visitedAccesses in accessesToVisit)
                {
                    collectedAccesses.Add(visitedAccesses);
                }

                accessesToVisit = accessesForNextIteration;
            }

            return collectedAccesses;
        }

        private static double CalculateLCOM(HashSet<Access> accesses)
        {
            int methods = accesses.GroupBy(e => e.Method).Count();
            int fields = accesses.GroupBy(e => e.Field).Count();
            return 1 - (double)accesses.Count / (methods * fields);
        }
    }
}