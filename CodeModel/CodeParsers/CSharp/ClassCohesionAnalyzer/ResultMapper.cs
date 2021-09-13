using System.Collections.Generic;
using System.Linq;
using CodeModel.CaDETModel.CodeItems;
using CodeModel.CodeParsers.CSharp.Exceptions;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer
{
    public class ResultMapper
    {
        public Dictionary<int, CaDETField> FieldsMapping { get; private set; }
        public Dictionary<int, CaDETMember> AccessorsMapping { get; private set; }
        public Dictionary<int, CaDETMember> MethodsMapping { get; private set; }

        public ResultMapper(CaDETClass parsedClass)
        {
            var fields = parsedClass.Fields;
            var fieldsDefiningAccessors =
                parsedClass.Members.Where(m => m.IsFieldDefiningAccessor()).ToList();
            var normalMethods = parsedClass.Members.Where(m => m.Type == CaDETMemberType.Method).ToList();

            ValidateCaDETClass(parsedClass.Name, normalMethods, fields, fieldsDefiningAccessors);
            RemoveUnusedMethodsAndFields(normalMethods, fields, fieldsDefiningAccessors);

            InitializeMappings(fields, fieldsDefiningAccessors, normalMethods);
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

        private void InitializeMappings(IReadOnlyList<CaDETField> fields,
            IReadOnlyList<CaDETMember> fieldsDefiningAccessors, IReadOnlyList<CaDETMember> normalMethods)
        {
            FieldsMapping = new Dictionary<int, CaDETField>();
            for (var i = 0; i < fields.Count; i++)
            {
                FieldsMapping[i] = fields[i];
            }

            AccessorsMapping = new Dictionary<int, CaDETMember>();
            for (var i = 0; i < fieldsDefiningAccessors.Count; i++)
            {
                AccessorsMapping[i + fields.Count] = fieldsDefiningAccessors[i];
            }

            MethodsMapping = new Dictionary<int, CaDETMember>();
            for (var i = 0; i < normalMethods.Count; i++)
            {
                MethodsMapping[i] = normalMethods[i];
            }
        }
    }
}