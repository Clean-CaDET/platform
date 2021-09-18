using System.Collections.Generic;
using System.Linq;
using CodeModel.CaDETModel.CodeItems;
using CodeModel.CodeParsers.CSharp.Exceptions;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer
{
    public class ResultMapper
    {
        public CaDETField[] Fields { get; }
        public CaDETMember[] Accessors { get; }
        public CaDETMember[] Methods { get; }

        public ResultMapper(CaDETClass parsedClass)
        {
            var fields = parsedClass.Fields;
            var fieldsDefiningAccessors =
                parsedClass.Members.Where(m => m.IsFieldDefiningAccessor()).ToList();
            var normalMethods = parsedClass.Members.Where(m => m.Type == CaDETMemberType.Method).ToList();

            ValidateCaDETClass(parsedClass.Name, normalMethods, fields, fieldsDefiningAccessors);
            RemoveUnusedMethodsAndFields(normalMethods, fields, fieldsDefiningAccessors);

            Fields = fields.ToArray();
            Accessors = fieldsDefiningAccessors.ToArray();
            Methods = normalMethods.ToArray();
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
    }
}