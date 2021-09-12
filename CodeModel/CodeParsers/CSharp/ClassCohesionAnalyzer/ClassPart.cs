using System.Collections.Generic;
using System.Linq;
using CodeModel.CaDETModel.CodeItems;
using CodeModel.CodeParsers.CSharp.Exceptions;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer
{
    public class ClassPart
    {
        public HashSet<Access> Accesses { get; }

        public ClassPart(CaDETClass parsedClass)
        {
            var fields = parsedClass.Fields;
            var fieldsDefiningAccessors =
                parsedClass.Members.Where(m => m.IsFieldDefiningAccessor()).ToList();
            var normalMethods = parsedClass.Members.Where(m => m.Type == CaDETMemberType.Method).ToList();

            ValidateCaDETClass(parsedClass.Name, normalMethods, fields, fieldsDefiningAccessors);
            RemoveUnusedMethodsAndFields(normalMethods, fields, fieldsDefiningAccessors);

            Accesses = GetAllAccesses(normalMethods, fields, fieldsDefiningAccessors);
        }

        public ClassPart(IEnumerable<Access> parsedClass)
        {
            Accesses = new HashSet<Access>(parsedClass);
        }

        private void ValidateCaDETClass(string className, List<CaDETMember> normalMethods, List<CaDETField> fields,
            List<CaDETMember> fieldsDefiningAccessors)
        {
            if (fields.Count == 0 && fieldsDefiningAccessors.Count == 0)
                throw new ClassWithoutElementsException($"Class `{className}` has no data members.");
            if (normalMethods.Count == 0)
                throw new ClassWithoutElementsException($"Class `{className}` has no normal methods.");
        }

        private static void RemoveUnusedMethodsAndFields(List<CaDETMember> normalMethods, List<CaDETField> fields,
            List<CaDETMember> fieldsDefiningAccessors)
        {
            normalMethods.RemoveAll(member => member.AccessedFields.Count == 0 && member.AccessedAccessors.Count == 0);
            fields.RemoveAll(field =>
                !normalMethods.Any(method => method.AccessedFields.Contains(field))
            );
            fieldsDefiningAccessors.RemoveAll(accessor =>
                !normalMethods.Any(method => method.AccessedAccessors.Contains(accessor)));
        }

        private HashSet<Access> GetAllAccesses(List<CaDETMember> normalMethods, List<CaDETField> fields,
            List<CaDETMember> fieldsDefiningAccessors)
        {
            var accesses = new HashSet<Access>();
            for (var i = 0; i < normalMethods.Count; i++)
            {
                for (var j = 0; j < fields.Count; j++)
                    if (normalMethods[i].AccessedFields.Contains(fields[j]))
                        accesses.Add(new Access(i, j));

                for (var j = 0; j < fieldsDefiningAccessors.Count; j++)
                    if (normalMethods[i].AccessedAccessors.Contains(fieldsDefiningAccessors[j]))
                        accesses.Add(new Access(i, j + fields.Count));
            }

            return accesses;
        }

        private IEnumerable<HashSet<Access>> GetAccessesThatCannotBeRemoved()
        {
            var allFieldIndexes = Accesses.Select(access => access.Field).ToHashSet();
            var allMethodIndexes = Accesses.Select(access => access.Method).ToHashSet();

            var accessesPerField =
                allFieldIndexes.Select(fieldIndex => Accesses.Where(e => e.Method == fieldIndex).ToHashSet());
            var accessesPerMethod =
                allMethodIndexes.Select(methodIndex => Accesses.Where(e => e.Field == methodIndex).ToHashSet());

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
                .Range(0, 1 << (data.Length / 2 + 1))
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