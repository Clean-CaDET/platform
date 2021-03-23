using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompiler.CodeModel.CaDETModel.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryCompiler.CodeModel.CodeParsers.CSharp
{
    internal class CaDETClassMetricCalculator
    {
        //TODO: See how this class will change with new metrics and try to decouple it from CSharp (e.g., by moving to CaDETClassMetric constructor)
        //TODO: Currently we see feature envy for CaDETClass.
        internal CaDETClassMetrics CalculateClassMetrics(CaDETClass parsedClass)
        {
            return new CaDETClassMetrics
            {
                LOC = GetLinesOfCode(parsedClass.SourceCode),
                NMD = GetNumberOfMethodsDeclared(parsedClass),
                NAD = GetNumberOfAttributesDefined(parsedClass),
                WMC = GetWeightedMethodPerClass(parsedClass),
                LCOM = GetLackOfCohesionOfMethods(parsedClass),
                TCC = GetTightClassCohesion(parsedClass),
                ATFD = GetAccessToForeignData(parsedClass),
                NOR = CountReturnStatements(parsedClass),
                NOL = CountLoops(parsedClass),
                NOC = CountComparisonOperators(parsedClass),
                NOA = CountNumberOfAssignments(parsedClass),
                NOPM = CountNumberOfPrivateMethods(parsedClass),
                NOPF = CountNumberOfProtectedFields(parsedClass),
            };
        }
        public int GetLinesOfCode(string code)
        {
            return code.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
        }

        /// <summary>
        /// ATFD: Access To Foreign Data
        /// DOI: 10.1109/ESEM.2009.5314231
        /// </summary>
        private int GetAccessToForeignData(CaDETClass parsedClass)
        {
            ISet<CaDETField> accessedExternalFields = new HashSet<CaDETField>();
            ISet<CaDETMember> accessedExternalAccessors = new HashSet<CaDETMember>();

            foreach (var member in parsedClass.Members)
            {
                accessedExternalFields.UnionWith(member.AccessedFields.Where(f => !f.Parent.Equals(member.Parent)));
                accessedExternalAccessors.UnionWith(member.AccessedAccessors.Where(a => !a.Parent.Equals(member.Parent)));
            }

            return accessedExternalAccessors.Count + accessedExternalFields.Count;
        }

        private double? GetLackOfCohesionOfMethods(CaDETClass parsedClass)
        {
            //TODO: Will need to reexamine the way we look at accessors and fields
            double maxCohesion = (GetNumberOfAttributesDefined(parsedClass)) * GetNumberOfMethodsDeclared(parsedClass);
            if (maxCohesion == 0) return null;

            double methodFieldAccess = 0;
            foreach (var method in parsedClass.Members.Where(method => method.Type.Equals(CaDETMemberType.Method)))
            {
                methodFieldAccess += CountOwnFieldAndAccessorAccessed(parsedClass, method);
            }
            return Math.Round(1 - methodFieldAccess/maxCohesion, 3);
        }

        /// <summary>
        /// TCC - Tight Class Cohesion
        /// DOI: 10.1145/223427.211856
        /// </summary>
        private double? GetTightClassCohesion(CaDETClass parsedClass)
        {
            int N = GetNumberOfMethodsDeclared(parsedClass);
           
            double NP = (N * (N - 1)) / 2;
            if (NP == 0) return null;

            return Math.Round(CountMethodPairsThatShareAccessToAFieldOrAccessor(parsedClass.Members) / NP, 2);
        }

        private static int CountMethodPairsThatShareAccessToAFieldOrAccessor(List<CaDETMember> classMethods)
        {
            int methodPairsThatShareAccessToAFieldOrAccessor = 0;

            for (var i = 0; i < classMethods.Count - 1; i++)
            {
                for (var j = i+1; j < classMethods.Count; j++)
                {
                    var firstMethod = classMethods[i];
                    var secondMethod = classMethods[j];

                    if (firstMethod.GetAccessedOwnFields().Intersect(secondMethod.GetAccessedOwnFields()).Any() 
                        || firstMethod.GetAccessedOwnAccessors().Intersect(secondMethod.GetAccessedOwnAccessors()).Any())
                    {
                        methodPairsThatShareAccessToAFieldOrAccessor++;
                    }
                }
            }
            return methodPairsThatShareAccessToAFieldOrAccessor;
        }

        private int CountOwnFieldAndAccessorAccessed(CaDETClass parsedClass, CaDETMember method)
        {
            int counter = method.AccessedFields.Count(field => Enumerable.Contains(parsedClass.Fields, field));
            counter += method.AccessedAccessors.Count(accessor => Enumerable.Contains(parsedClass.Members, accessor));

            return counter;
        }

        /// <summary>
        /// WMC - Weighted Method Per Class
        /// DOI: 10.1109/32.295895
        /// </summary>
        private int GetWeightedMethodPerClass(CaDETClass parsedClass)
        {
            return parsedClass.Members.Sum(method => method.Metrics.CYCLO);
        }
        
        private int GetNumberOfAttributesDefined(CaDETClass parsedClass)
        {
            //TODO: Probably should expand to include simple accessors that do not have a related field.
            //TODO: It is C# specific, but this is the CSSharpMetricCalculator
            return parsedClass.Fields.Count + parsedClass.Members.Count(m => m.IsFieldDefiningAccessor());
        }

        private int GetNumberOfMethodsDeclared(CaDETClass parsedClass)
        {
            return parsedClass.Members.Count(method => method.Type.Equals(CaDETMemberType.Method));
        }

        private int CountReturnStatements(CaDETClass parsedClass)
        {
            return parsedClass.Members.Sum(method => method.Metrics.NOR);
        }

        private int CountLoops(CaDETClass parsedClass)
        {
            return parsedClass.Members.Sum(method => method.Metrics.NOL);
        }

        private int CountComparisonOperators(CaDETClass parsedClass)
        {
            return parsedClass.Members.Sum(method => method.Metrics.NOC);
        }

        private int CountNumberOfAssignments(CaDETClass parsedClass)
        {
            return parsedClass.Members.Sum(method => method.Metrics.NOA);
        }

        private int CountNumberOfPrivateMethods(CaDETClass parsedClass)
        {
            return parsedClass.Members.Count(method => method.Type.Equals(CaDETMemberType.Method) &&
                                                       method.Modifiers.Any(m => m.Value == CaDETModifierValue.Private));
        }

        private int CountNumberOfProtectedFields(CaDETClass parsedClass)
        {
            return parsedClass.Fields.Count(field => field.Modifiers.Any(f => f.Value == CaDETModifierValue.Protected));
        }
    }
}