using System.Collections.Generic;
using System.Linq;
using CodeModel.CaDETModel.CodeItems;
using CodeModel.CodeParsers.CSharp.Exceptions;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer
{
    public class FilteredClass
    {
        public CaDETField[] Fields { get; private set; }
        public CaDETMember[] Accessors { get; private set; }
        public CaDETMember[] Methods { get; private set; }

        public FilteredClass(CaDETClass parsedClass)
        {
            Fields = parsedClass.Fields.ToArray();
            Accessors =
                parsedClass.Members.Where(m => m.IsFieldDefiningAccessor()).ToArray();
            Methods = parsedClass.Members.Where(m => m.Type == CaDETMemberType.Method).ToArray();

            ValidateCaDETClass(parsedClass.Name);
            RemoveUnusedMethodsAndFields();
        }

        private void ValidateCaDETClass(string className)
        {
            if (Fields.Length == 0 && Accessors.Length == 0)
                throw new ClassWithoutElementsException($"Class `{className}` has no data members.");
            if (Methods.Length == 0)
                throw new ClassWithoutElementsException($"Class `{className}` has no normal methods.");
        }

        private void RemoveUnusedMethodsAndFields()
        {
            Fields = Fields.Where(field => Methods.Any(method => method.AccessedFields.Contains(field))).ToArray();
            Accessors = Accessors.Where(accessor =>
                Methods.Any(method => method.AccessedAccessors.Contains(accessor))).ToArray();
            Methods = Methods.Where(member => member.AccessedFields.Count != 0 || member.AccessedAccessors.Count != 0).ToArray();
        }
    }
}