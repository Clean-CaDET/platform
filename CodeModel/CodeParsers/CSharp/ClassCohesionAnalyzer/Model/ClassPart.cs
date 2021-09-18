using System.Collections.Generic;
using System.Linq;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer.Model
{
    public class ClassPart
    {
        public HashSet<Access> Accesses { get; }

        public ClassPart(FilteredClass filteredClass)
        {
            Accesses = GetAllAccesses(filteredClass);
        }

        public ClassPart(IEnumerable<Access> accesses)
        {
            Accesses = new HashSet<Access>(accesses);
        }

        private HashSet<Access> GetAllAccesses(FilteredClass filteredClass)
        {
            var fields = filteredClass.Fields;
            var fieldsDefiningAccessors = filteredClass.Accessors;
            var normalMethods = filteredClass.Methods;

            var accesses = new HashSet<Access>();
            for (var i = 0; i < normalMethods.Length; i++)
            {
                for (var j = 0; j < fields.Length; j++)
                    if (normalMethods[i].AccessedFields.Contains(fields[j]))
                        accesses.Add(new Access(i, j));

                for (var j = 0; j < fieldsDefiningAccessors.Length; j++)
                    if (normalMethods[i].AccessedAccessors.Contains(fieldsDefiningAccessors[j]))
                        accesses.Add(new Access(i, j + fields.Length));
            }

            return accesses;
        }

        private IEnumerable<HashSet<Access>> GetAccessesThatCannotBeRemoved()
        {
            var allFieldIndexes = Accesses.Select(access => access.DataMember).ToHashSet();
            var allMethodIndexes = Accesses.Select(access => access.Method).ToHashSet();

            var accessesPerField =
                allFieldIndexes.Select(fieldIndex => Accesses.Where(e => e.Method == fieldIndex).ToHashSet());
            var accessesPerMethod =
                allMethodIndexes.Select(methodIndex => Accesses.Where(e => e.DataMember == methodIndex).ToHashSet());

            var result = accessesPerField
                .Where(fieldAccesses => fieldAccesses.Count != 0).ToList();
            result.AddRange(accessesPerMethod
                .Where(methodAccesses => methodAccesses.Count != 0));

            return result;
        }

        public IEnumerable<HashSet<Access>> GetAccessesThatCanBeRemoved()
        {
            var data = Accesses.ToArray();
            if (data.Length == 0) return new List<HashSet<Access>>();

            // create all combinations of edges without repetition
            // as hashsets having 0 to n/2 + 1 elements
            var allAccessesCombinations = Enumerable
                .Range(0, 1 << ((data.Length / 2) + 1))
                .Select(index => data
                    .Where((v, i) => (index & (1 << i)) != 0)
                    .ToHashSet()
                );

            var accessesThatCannotBeRemoved = GetAccessesThatCannotBeRemoved();
            return allAccessesCombinations.Where(accesses =>
                !accessesThatCannotBeRemoved.Any(accesses.IsSupersetOf)
            );
        }
    }
}