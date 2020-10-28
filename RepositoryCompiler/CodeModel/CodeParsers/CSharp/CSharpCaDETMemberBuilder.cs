using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryCompiler.CodeModel.CodeParsers.CSharp
{
    internal class CSharpCaDETMemberBuilder
    {

        private const string _separator = ".";

        private readonly MemberDeclarationSyntax _cSharpMember;
        private readonly SemanticModel _semanticModel;
        private readonly CaDETMember _member;

        internal CSharpCaDETMemberBuilder(MemberDeclarationSyntax cSharpMember, SemanticModel semanticModel)
        {
            _member = CreateMemberBasedOnType(cSharpMember);
            if (_member == null) throw new InappropriateMemberTypeException();
            _cSharpMember = cSharpMember;
            _semanticModel = semanticModel;
        }

        internal CaDETMember CreateBasicMember(CaDETClass parent)
        {
            _member.Modifiers = _cSharpMember.Modifiers.Select(modifier => new CaDETModifier(modifier.ValueText)).ToList();
            _member.SourceCode = _cSharpMember.ToString();
            _member.Parent = parent;
            _member.Params = GetMethodParams();
            return _member;
        }

        internal void DetermineAccessedCodeItems(List<CaDETClass> allProjectClasses)
        {
            _member.InvokedMethods = CalculateInvokedMethods(allProjectClasses);
            _member.AccessedAccessors = CalculateAccessedAccessors(allProjectClasses);
            _member.AccessedFields = CalculateAccessedFields(allProjectClasses);
        }

        internal void CalculateMetrics(CSharpMetricCalculator calculator)
        {
            _member.Metrics = calculator.CalculateMemberMetrics(_cSharpMember, _member);
        }

        private CaDETMember CreateMemberBasedOnType(MemberDeclarationSyntax member)
        {
            return member switch
            {
                PropertyDeclarationSyntax property => new CaDETMember { Type = CaDETMemberType.Property, Name = property.Identifier.Text },
                ConstructorDeclarationSyntax constructor => new CaDETMember { Type = CaDETMemberType.Constructor, Name = constructor.Identifier.Text },
                MethodDeclarationSyntax method => new CaDETMember { Type = CaDETMemberType.Method, Name = method.Identifier.Text },
                _ => null
            };
        }
        private List<CaDETParameter> GetMethodParams()
        {
            List<CaDETParameter> memberParams = new List<CaDETParameter>();
            var paramLists = _cSharpMember.DescendantNodes().OfType<ParameterListSyntax>().ToList();
            if (!paramLists.Any()) return memberParams;

            // First() below gives the function param list. Other elements of the list include params for inline lambda expressions
            var parameters = paramLists.First().Parameters;
            foreach (var parameter in parameters)
            {
                var symbol = _semanticModel.GetDeclaredSymbol(parameter);
                memberParams.Add(new CaDETParameter { Name = symbol.Name, Type = symbol.ToDisplayString() });
            }

            return memberParams;
        }

        private ISet<CaDETMember> CalculateInvokedMethods(List<CaDETClass> allProjectClasses)
        {
            ISet<CaDETMember> methods = new HashSet<CaDETMember>();
            var invokedMethods = _cSharpMember.DescendantNodes().OfType<InvocationExpressionSyntax>();
            foreach (var invoked in invokedMethods)
            {
                var symbol = _semanticModel.GetSymbolInfo(invoked.Expression).Symbol;
                if (symbol == null) continue; //True when invoked method is a system or library call and not part of our code.
                foreach (var projectClass in allProjectClasses)
                {
                    var invokedCaDETMember = projectClass.FindMemberBySignature(symbol.ToDisplayString());
                    if (invokedCaDETMember == null) continue;
                    methods.Add(invokedCaDETMember);
                    break;
                }
            }

            return methods;
        }
        
        private ISet<CaDETField> CalculateAccessedFields(List<CaDETClass> allProjectClasses)
        {
            ISet<CaDETField> fields = new HashSet<CaDETField>();
            var accessedFields = _semanticModel.GetOperation(_cSharpMember).Descendants().OfType<IFieldReferenceOperation>();
            foreach (var field in accessedFields)
            {
                var fullFieldName = field.Member.ToDisplayString();
                var containingClass = FindContainingClass(allProjectClasses, fullFieldName);
                if(IsEnumeration(containingClass)) continue;
                fields.Add(containingClass.FindField(fullFieldName.Split(_separator).Last()));
            }
            return fields;
        }

        private CaDETClass FindContainingClass(List<CaDETClass> classes, string stubElementName)
        {
            string[] nameParts = stubElementName.Split(_separator);
            string className = string.Join(_separator, nameParts, 0, nameParts.Length - 1);
            return classes.Find(c => c.FullName.Equals(className));
        }

        private static bool IsEnumeration(CaDETClass containingClass)
        {
            return containingClass == null;
        }

        private ISet<CaDETMember> CalculateAccessedAccessors(List<CaDETClass> allProjectClasses)
        {
            ISet<CaDETMember> accessors = new HashSet<CaDETMember>();
            var accessedAccessors = _semanticModel.GetOperation(_cSharpMember).Descendants().OfType<IPropertyReferenceOperation>();
            foreach (var accessor in accessedAccessors)
            {
                foreach (var projectClass in allProjectClasses)
                {
                    var accessedCaDETMember = projectClass.FindMemberBySignature(accessor.Member.ToDisplayString() + "()");
                    if (accessedCaDETMember == null) continue;
                    accessors.Add(accessedCaDETMember);
                    break;
                }
            }
            return accessors;
        }
    }
}