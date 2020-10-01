using Microsoft.CodeAnalysis.CSharp.Syntax;
using RepositoryCompiler.CodeModel.CaDETModel;
using System;
using System.Linq;

namespace RepositoryCompiler.CodeModel.CodeParsers.CSharp
{
    public class CSharpMetricCalculator
    {
        //TODO: See how this class will change with new metrics and try to decouple it from CSharp (e.g., by moving to CaDETClassMetric constructor)
        public CaDETClassMetrics CalculateClassMetrics(CaDETClass parsedClass)
        {
            return new CaDETClassMetrics
            {
                LOC = GetLinesOfCode(parsedClass.SourceCode),
                NMD = GetNumberOfMethodsDeclared(parsedClass),
                NAD = GetNumberOfAttributesDefined(parsedClass),
                WMC = GetWeightedMethodCount(parsedClass),
                LCOM = GetLackOfCohesionOfMethods(parsedClass)
            };
        }

        private double? GetLackOfCohesionOfMethods(CaDETClass parsedClass)
        {
            //TODO: Will need to reexamine the way we look at accessors and fields
            double maxCohesion = (GetNumberOfAttributesDefined(parsedClass) + GetNumberOfSimpleAccessors(parsedClass)) * GetNumberOfMethodsDeclared(parsedClass);
            if (maxCohesion == 0) return null;

            double methodFieldAccess = 0;
            foreach (var method in parsedClass.Methods.Where(method => method.Type.Equals(CaDETMemberType.Method)))
            {
                methodFieldAccess += CountOwnFieldAndAccessorAccessed(parsedClass, method);
            }
            return 1 - methodFieldAccess/maxCohesion;
        }

        private int GetNumberOfSimpleAccessors(CaDETClass parsedClass)
        {
            return parsedClass.Methods.Count(method => method.IsSimpleAccessor());
        }

        private int CountOwnFieldAndAccessorAccessed(CaDETClass parsedClass, CaDETMember method)
        {
            int counter = 0;
            foreach (var fieldOrAccessor in method.AccessedFieldsAndAccessors)
            {
                if (Enumerable.Contains(parsedClass.Fields, fieldOrAccessor) || Enumerable.Contains(parsedClass.Methods, fieldOrAccessor))
                {
                    counter++;
                }
            }

            return counter;
        }

        private int GetWeightedMethodCount(CaDETClass parsedClass)
        {
            return parsedClass.Methods.Sum(method => method.Metrics.CYCLO);
        }

        private int GetNumberOfAttributesDefined(CaDETClass parsedClass)
        {
            return parsedClass.Fields.Count;
        }

        private int GetNumberOfMethodsDeclared(CaDETClass parsedClass)
        {
            return parsedClass.Methods.Count(method => method.Type.Equals(CaDETMemberType.Method));
        }

        public CaDETMemberMetrics CalculateMemberMetrics(MemberDeclarationSyntax member)
        {
            return new CaDETMemberMetrics
            {
                CYCLO = CalculateCyclomaticComplexity(member),
                LOC = GetLinesOfCode(member.ToString())
            };
        }

        private int GetLinesOfCode(string code)
        {
            return code.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
        }

        private int CalculateCyclomaticComplexity(MemberDeclarationSyntax method)
        {
            //Concretely, in C# the CC of a method is 1 + {the number of following expressions found in the body of the method}:
            //if | while | for | foreach | case | default | continue | goto | && | || | catch | ternary operator ?: | ??
            //https://www.ndepend.com/docs/code-metrics#CC
            int count = method.DescendantNodes().OfType<IfStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<WhileStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<ForStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<ForEachStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<CaseSwitchLabelSyntax>().Count();
            count += method.DescendantNodes().OfType<DefaultSwitchLabelSyntax>().Count();
            count += method.DescendantNodes().OfType<ContinueStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<GotoStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<ConditionalExpressionSyntax>().Count();
            count += method.DescendantNodes().OfType<CatchClauseSyntax>().Count();

            count += CountOccurrences(method.ToString(), "&&");
            count += CountOccurrences(method.ToString(), "||");
            count += CountOccurrences(method.ToString(), "??");
            
            return count + 1;
        }

        private int CountOccurrences(string text, string pattern)
        {
            var count = 0;
            var i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }
    }
}