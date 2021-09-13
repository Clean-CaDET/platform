using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer.Model;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer
{
    public class CohesivePartsOutput
    {
        private string AccessesToRemove { get; }
        private List<string> TextsOfParts { get; }

        public CohesivePartsOutput(string accessesToRemove, List<string> textsOfTextsOfParts)
        {
            AccessesToRemove = accessesToRemove;
            TextsOfParts = textsOfTextsOfParts;
        }

        public override bool Equals(object obj)
        {
            if (obj is not CohesivePartsOutput cohesivePartsOutput) return false;

            if (cohesivePartsOutput.AccessesToRemove != AccessesToRemove) return false;

            return cohesivePartsOutput.TextsOfParts.All(textOfPart =>
                cohesivePartsOutput.TextsOfParts.Contains(textOfPart));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AccessesToRemove, TextsOfParts);
        }

        public static CohesivePartsOutput[] GenerateOutput(CohesiveParts[] cohesiveParts, ResultMapper resultMapper)
        {
            if (cohesiveParts.Length == 0)
                return Array.Empty<CohesivePartsOutput>();

            var result = new CohesivePartsOutput[cohesiveParts.Length];
            for (var i = 0; i < cohesiveParts.Length; i++)
            {
                var part = cohesiveParts[i];
                var accessesToRemove = GetAccessesToRemoveText(part, resultMapper);
                var textsOfParts = part.Parts.Select(p => GetClassPartText(p, resultMapper)).ToList();

                result[i] = new CohesivePartsOutput(accessesToRemove, textsOfParts);
            }

            return result;
        }

        private static string GetAccessesToRemoveText(CohesiveParts part, ResultMapper resultMapper)
        {
            if (part.AccessesToRemove.Count == 0)
                return "Class is already disconnected. No accesses should be removed.\n";

            var builder = new StringBuilder();
            builder.Append("To perform refactoring remove following method-field accesses:\n");
            foreach (var access in part.AccessesToRemove)
            {
                var method = resultMapper.MethodsMapping[access.Method].Name;
                var dataMember = resultMapper.FieldsMapping.ContainsKey(access.DataMember)
                    ? resultMapper.FieldsMapping[access.DataMember].Name
                    : resultMapper.AccessorsMapping[access.DataMember].Name;

                builder.Append(method);
                builder.Append(" -> ");
                builder.Append(dataMember);
                builder.Append('\n');
            }

            return builder.ToString();
        }

        private static string GetClassPartText(ClassPart classPart, ResultMapper resultMapper)
        {
            var dataMembers = classPart.Accesses.GroupBy(access => access.DataMember).Select(group => group.Key)
                .ToList();
            var normalMethods = classPart.Accesses.GroupBy(access => access.Method).Select(group => group.Key).ToList();

            var builder = new StringBuilder();
            builder.Append("Cohesive part:\nData members: ");
            var fields = dataMembers.Where(dataMember => resultMapper.FieldsMapping.ContainsKey(dataMember))
                .Select(i => resultMapper.FieldsMapping[i].Name);
            var accessors = dataMembers.Where(dataMember => resultMapper.AccessorsMapping.ContainsKey(dataMember))
                .Select(i => resultMapper.AccessorsMapping[i].Name);
            builder.AppendJoin(", ", fields);
            builder.AppendJoin(", ", accessors);

            builder.Append("\nNormal methods: ");
            var methods = normalMethods.Select(i => resultMapper.MethodsMapping[i].Name);
            builder.AppendJoin(", ", methods);

            return builder.ToString();
        }
    }
}