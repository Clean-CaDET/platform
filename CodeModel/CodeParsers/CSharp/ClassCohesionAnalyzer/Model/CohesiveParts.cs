using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer.Model
{
    public class CohesiveParts
    {
        public List<ClassPart> Parts { get; }
        public HashSet<Access> AccessesToRemove { get; }

        public CohesiveParts(HashSet<Access> accessesToRemove, IEnumerable<HashSet<Access>> parts)
        {
            AccessesToRemove = accessesToRemove;
            Parts = parts.Select(part => new ClassPart(part)).ToList();
        }
        
        public string RefactoringResults(FilteredClass filteredClass)
        {
            var builder = new StringBuilder();
            builder.Append(GetAccessesToRemoveText(filteredClass));
            var textsOfParts = Parts.Select(p => GetClassPartText(p, filteredClass)).ToList();
            builder.AppendJoin('\n', textsOfParts);

            return builder.ToString();
        }

        private string GetAccessesToRemoveText(FilteredClass filteredClass)
        {
            if (AccessesToRemove.Count == 0)
                return "Class is already disconnected. No accesses should be removed.\n";

            var builder = new StringBuilder();
            builder.Append("To perform the refactoring remove the following method-field accesses:\n");
            foreach (var access in AccessesToRemove)
            {
                var method = filteredClass.Methods[access.Method].Name;
                var dataMember = access.DataMember < filteredClass.Fields.Length
                    ? filteredClass.Fields[access.DataMember].Name
                    : filteredClass.Accessors[access.DataMember].Name;

                builder.Append(method);
                builder.Append(" -> ");
                builder.Append(dataMember);
                builder.Append('\n');
            }

            return builder.ToString();
        }

        private string GetClassPartText(ClassPart classPart, FilteredClass filteredClass)
        {
            var dataMembers = classPart.Accesses.GroupBy(access => access.DataMember).Select(group => group.Key)
                .ToList();
            var normalMethods = classPart.Accesses.GroupBy(access => access.Method).Select(group => group.Key).ToList();

            var builder = new StringBuilder();
            builder.Append("Cohesive part:\nData members: ");
            var fields = dataMembers.Where(dataMember => dataMember < filteredClass.Fields.Length)
                .Select(i => filteredClass.Fields[i].Name);
            var accessors = dataMembers.Where(dataMember => dataMember >= filteredClass.Fields.Length)
                .Select(i => filteredClass.Accessors[i].Name);
            builder.AppendJoin(", ", fields);
            builder.AppendJoin(", ", accessors);

            builder.Append("\nNormal methods: ");
            var methods = normalMethods.Select(i => filteredClass.Methods[i].Name);
            builder.AppendJoin(", ", methods);

            return builder.ToString();
        }
    }
}