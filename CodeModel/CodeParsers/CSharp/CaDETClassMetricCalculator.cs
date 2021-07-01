using CodeModel.CaDETModel.CodeItems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.CodeParsers.CSharp
{
    internal class CaDETClassMetricCalculator
    {
        //TODO: See how this class will change with new metrics and try to decouple it from CSharp (e.g., by moving to CaDETClassMetric constructor)
        //TODO: Currently we see feature envy for CaDETClass.
        internal Dictionary<CaDETMetric, double> CalculateClassMetrics(CaDETClass parsedClass)
        {
            return new Dictionary<CaDETMetric, double>
            {
                [CaDETMetric.CLOC] = GetLinesOfCode(parsedClass.SourceCode),
                [CaDETMetric.NMD] = GetNumberOfMethodsDeclared(parsedClass),
                [CaDETMetric.NAD] = GetNumberOfAttributesDefined(parsedClass),
                [CaDETMetric.NMD_NAD] = GetNumberOfMethodsDeclared(parsedClass) + GetNumberOfAttributesDefined(parsedClass),
                [CaDETMetric.WMC] = GetWeightedMethodPerClass(parsedClass),
                [CaDETMetric.LCOM] = GetLackOfCohesionOfMethods(parsedClass),
                [CaDETMetric.LCOM3] = GetLackOfCohesionOfMethods3(parsedClass),
                [CaDETMetric.LCOM4] = GetLackOfCohesionOfMethods4(parsedClass),
                [CaDETMetric.TCC] = GetTightClassCohesion(parsedClass),
                [CaDETMetric.ATFD] = GetAccessToForeignData(parsedClass),
                [CaDETMetric.CNOR] = CountReturnStatements(parsedClass),
                [CaDETMetric.CNOL] = CountLoops(parsedClass),
                [CaDETMetric.CNOC] = CountComparisonOperators(parsedClass),
                [CaDETMetric.CNOA] = CountNumberOfAssignments(parsedClass),
                [CaDETMetric.NOPM] = CountNumberOfPrivateMethods(parsedClass),
                [CaDETMetric.NOPF] = CountNumberOfProtectedFields(parsedClass),
                [CaDETMetric.CMNB] = CountMaxNestedBlocks(parsedClass),
                [CaDETMetric.RFC] = CountUniqueMethodInvocations(parsedClass),
                [CaDETMetric.CBO] = CountDependencies(parsedClass),
                [CaDETMetric.DIT] = CountInheritanceLevel(parsedClass),
                [CaDETMetric.DCC] = CountClassCoupling(parsedClass),
                [CaDETMetric.ATFD_10] = GetAccessToForeignDataDirectly(parsedClass),
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

        /// <summary>
        /// ATFD: Access To Foreign Data Directly
        /// DOI: 10.1145/1852786.1852797
        /// </summary>
        private double GetAccessToForeignDataDirectly(CaDETClass parsedClass)
        {
            ISet<CaDETField> accessedExternalFields = new HashSet<CaDETField>();

            foreach (var member in parsedClass.Members)
            {
                accessedExternalFields.UnionWith(member.AccessedFields.Where(f => !f.Parent.Equals(member.Parent)));
            }

            return accessedExternalFields.Count;
        }

        private double GetLackOfCohesionOfMethods(CaDETClass parsedClass)
        {
            //TODO: Will need to reexamine the way we look at accessors and fields
            double maxCohesion = (GetNumberOfAttributesDefined(parsedClass)) * GetNumberOfMethodsDeclared(parsedClass);
            if (maxCohesion == 0) return -1;

            double methodFieldAccess = 0;
            foreach (var method in parsedClass.Members.Where(method => method.Type.Equals(CaDETMemberType.Method)))
            {
                methodFieldAccess += CountOwnFieldAndAccessorAccessed(parsedClass, method);
            }
            return Math.Round(1 - methodFieldAccess/maxCohesion, 3);
        }

        /// <summary>
        /// LCOM - Lack of Cohesion of Methods
        /// Article: Chidamber and Kemerer, 1991, "Towards a Metric Suite for Object-Oriented Design", Proc. OOPSLA'91, Sigplan Notices, 26(11), 197-211
        /// </summary> 
        private double GetLackOfCohesionOfMethods3(CaDETClass parsedClass)
        {
            var numberOfAttributes = GetNumberOfAttributesDefined(parsedClass);
            var numberOfMethods = GetNumberOfMethodsDeclared(parsedClass);
          
            if (numberOfMethods == 0 || numberOfAttributes == 0) return 0;
            if (numberOfMethods == 1) return 0;

            double methodFieldAccess = 0;
            foreach (var method in parsedClass.Members.Where(method => method.Type.Equals(CaDETMemberType.Method)))
            {
                methodFieldAccess += CountOwnFieldAndAccessorAccessed(parsedClass, method);
            }
            return Math.Round((numberOfMethods - (methodFieldAccess/numberOfAttributes)) / (numberOfMethods - 1), 3);
        }

        /// <summary>
        /// LCOM - Lack of Cohesion of Methods
        /// DOI: 10.1145/2723742.2723753
        /// </summary>
        private double GetLackOfCohesionOfMethods4(CaDETClass parsedClass)
        {
            var methods = parsedClass.Members.Where(method => method.Type.Equals(CaDETMemberType.Method)).ToList();

            var numberOfMethodsThatAccessOwnFieldsOrMethods = CountNumberOfMethodsThatAccessToOwnFieldsOrMethods(parsedClass, methods);
            var numberOfMethodsThatShareAccessToFieldOrAccessor = CountNumberOfMethodsThatShareAccessToAFieldOrAccessor(methods);

            return numberOfMethodsThatAccessOwnFieldsOrMethods - numberOfMethodsThatShareAccessToFieldOrAccessor;
        }

        private int CountNumberOfMethodsThatAccessToOwnFieldsOrMethods(CaDETClass parsedClass, List<CaDETMember> methods)
        {
            int counter = 0;
            foreach (var method in methods)
            {
                int numberOfOwnFieldAndMethodsAccessed = CountOwnFieldAndAccessorAccessed(parsedClass, method);
                numberOfOwnFieldAndMethodsAccessed += method.InvokedMethods.Count(method => Enumerable.Contains(parsedClass.Members, method));
                if (numberOfOwnFieldAndMethodsAccessed != 0)
                {
                    counter++;
                }
            }
            return counter;
        }

        private static int CountNumberOfMethodsThatShareAccessToAFieldOrAccessor(List<CaDETMember> classMethods)
        {
            HashSet<CaDETMember> methodsThatShareAccess = new HashSet<CaDETMember>();

            for (var i = 0; i < classMethods.Count - 1; i++)
            {
                for (var j = i + 1; j < classMethods.Count; j++)
                {
                    var firstMethod = classMethods[i];
                    var secondMethod = classMethods[j];

                    if (firstMethod.GetAccessedOwnFields().Intersect(secondMethod.GetAccessedOwnFields()).Any()
                        || firstMethod.GetAccessedOwnAccessors().Intersect(secondMethod.GetAccessedOwnAccessors()).Any())
                    {
                        methodsThatShareAccess.Add(firstMethod);
                        methodsThatShareAccess.Add(secondMethod);
                    }
                }
            }
            return methodsThatShareAccess.Count;
        }

        /// <summary>
        /// TCC - Tight Class Cohesion
        /// DOI: 10.1145/223427.211856
        /// </summary>
        private double GetTightClassCohesion(CaDETClass parsedClass)
        {
            int N = GetNumberOfMethodsDeclared(parsedClass);
           
            double NP = (N * (N - 1)) / 2;
            if (NP == 0) return -1;

            var classMethods = parsedClass.Members.FindAll(m => m.Type.Equals(CaDETMemberType.Method));

            return Math.Round(CountMethodPairsThatShareAccessToAFieldOrAccessor(classMethods) / NP, 2);
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
        private double GetWeightedMethodPerClass(CaDETClass parsedClass)
        {
            return parsedClass.Members.Sum(method => method.Metrics[CaDETMetric.CYCLO]);
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

        // Implementation based on https://github.com/mauricioaniche/ck
        private double CountReturnStatements(CaDETClass parsedClass)
        {
            return parsedClass.Members.Sum(method => method.Metrics[CaDETMetric.MNOR]);
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private double CountLoops(CaDETClass parsedClass)
        {
            return parsedClass.Members.Sum(method => method.Metrics[CaDETMetric.MNOL]);
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private double CountComparisonOperators(CaDETClass parsedClass)
        {
            return parsedClass.Members.Sum(method => method.Metrics[CaDETMetric.MNOC]);
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private double CountNumberOfAssignments(CaDETClass parsedClass)
        {
            return parsedClass.Members.Sum(method => method.Metrics[CaDETMetric.MNOA]);
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private int CountNumberOfPrivateMethods(CaDETClass parsedClass)
        {
            return parsedClass.Members.Count(method => method.Type.Equals(CaDETMemberType.Method) &&
                                                       method.Modifiers.Any(m => m.Value == CaDETModifierValue.Private));
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private int CountNumberOfProtectedFields(CaDETClass parsedClass)
        {
            return parsedClass.Fields.Count(field => field.Modifiers.Any(f => f.Value == CaDETModifierValue.Protected));
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private double CountMaxNestedBlocks(CaDETClass parsedClass)
        {
            if (!parsedClass.Members.Any())
            {
                return 0;
            }
            return parsedClass.Members.Max(method => method.Metrics[CaDETMetric.MMNB]);
        }

        private int CountUniqueMethodInvocations(CaDETClass parsedClass)
        {
            var invokedMethods = new HashSet<CaDETMember>();
            foreach (var member in parsedClass.Members)
            {
                invokedMethods.UnionWith(member.InvokedMethods.ToList());
            }
            return invokedMethods.Count();
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private int CountDependencies(CaDETClass parsedClass)
        {
            List<CaDETClass> allDependencies = new List<CaDETClass>();
            allDependencies.AddRange(parsedClass.GetFieldLinkedTypes().Distinct());
            allDependencies.AddRange(parsedClass.GetMethodLinkedReturnTypes().Distinct());
            allDependencies.AddRange(parsedClass.GetMethodLinkedVariableTypes().Distinct());
            var uniqueDependencies = allDependencies.GroupBy(d => d.FullName).Select(d => d.First());
            return uniqueDependencies.Count();
        }

        //Implementation based on http://www.theijes.com/papers/v5-i6/C05014019.pdf
        private int CountClassCoupling(CaDETClass parsedClass)
        {
            List<CaDETClass> allDependencies = new List<CaDETClass>();
            allDependencies.AddRange(parsedClass.GetFieldLinkedTypes().Where(c => c.FullName != parsedClass.FullName).Distinct());
            allDependencies.AddRange(parsedClass.GetMethodLinkedReturnTypes().Where(c => c.FullName != parsedClass.FullName).Distinct());
            allDependencies.AddRange(parsedClass.GetMethodLinkedVariableTypes().Where(c => c.FullName != parsedClass.FullName).Distinct());
            allDependencies.AddRange(parsedClass.GetMethodLinkedParameterTypes().Where(c => c.FullName != parsedClass.FullName).Distinct());
            allDependencies.AddRange(parsedClass.GetMethodInvocationsTypes().Where(c => c.FullName != parsedClass.FullName).Distinct());
            var uniqueDependencies = allDependencies.GroupBy(d => d.FullName).Select(d => d.First());
            return uniqueDependencies.Count();
        }

        //Implementation based on https://objectscriptquality.com/docs/metrics/depth-inheritance-tree
        private int CountInheritanceLevel(CaDETClass parsedClass)
        {
            CaDETClass parent = parsedClass.Parent;
            if (parent == null)
            {
                return 0;
            }

            return 1 + CountInheritanceLevel(parent);
        }
    }
}