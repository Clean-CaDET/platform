using CodeModel.CaDETModel.CodeItems;
using CodeModel.CodeParsers.CSharp.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.CodeParsers.CSharp
{
    internal class CSharpCaDETMemberBuilder
    {

        private const string _separator = ".";

        private readonly MemberDeclarationSyntax _cSharpMember;
        private readonly SemanticModel _semanticModel;
        private readonly CaDETMember _member;
        private readonly CaDETMemberMetricCalculator _metricCalculator;

        internal CSharpCaDETMemberBuilder(MemberDeclarationSyntax cSharpMember, SemanticModel semanticModel)
        {
            _member = CreateMemberBasedOnType(cSharpMember);
            _cSharpMember = cSharpMember;
            _semanticModel = semanticModel;
            _metricCalculator = new CaDETMemberMetricCalculator();
        }

        internal CaDETMember CreateBasicMember(CaDETClass parent)
        {
            _member.Modifiers = _cSharpMember.Modifiers.Select(modifier => new CaDETModifier(modifier.ValueText)).ToList();
            _member.SourceCode = _cSharpMember.ToString();
            _member.Parent = parent;
            _member.Params = GetMethodParams();
            _member.ReturnType = GetMethodReturnType();
            _member.Variables = GetMethodVariables();
            return _member;
        }

        private List<CaDETVariable> GetMethodVariables()
        {
            List<CaDETVariable> methodVariables = new List<CaDETVariable>();
            if (_cSharpMember is PropertyDeclarationSyntax) return methodVariables;
            var variableDeclarations = _cSharpMember.DescendantNodes().OfType<VariableDeclarationSyntax>();

            foreach (var variableDeclaration in variableDeclarations)
            {
                foreach (var variable in variableDeclaration.Variables)
                {
                    methodVariables.Add(new CaDETVariable(variable.Identifier.ToString(), 
                        new CaDETLinkedType() { FullType = ((ILocalSymbol)_semanticModel.GetDeclaredSymbol(variable)).Type.ToString() }));
                }
            }
            return methodVariables;
        }

        internal void DetermineAccessedCodeItems(List<CaDETClass> allProjectClasses)
        {
            _member.InvokedMethods = CalculateInvokedMethods(allProjectClasses);
            _member.AccessedAccessors = CalculateAccessedAccessors(allProjectClasses);
            _member.AccessedFields = CalculateAccessedFields(allProjectClasses);
        }

        internal void CalculateMetrics()
        {
            _member.Metrics = _metricCalculator.CalculateMemberMetrics(_cSharpMember, _member);
        }

        private static CaDETMember CreateMemberBasedOnType(MemberDeclarationSyntax member)
        {
            return member switch
            {
                PropertyDeclarationSyntax property => new CaDETMember { Type = CaDETMemberType.Property, Name = GetNameWithInterface(property.ExplicitInterfaceSpecifier, property.Identifier) },
                ConstructorDeclarationSyntax constructor => CreateNonStaticConstructor(constructor),
                MethodDeclarationSyntax method => CreateMethod(method),
                _ => throw new InappropriateMemberTypeException("Unsupported member type " + member.ToFullString() + "for CaDETMember.")
            };
        }

        private static CaDETMember CreateNonStaticConstructor(ConstructorDeclarationSyntax constructor)
        {
            if (constructor.Modifiers.Count(m => m.ValueText == "static") > 0)
                throw new InappropriateMemberTypeException("Error at: " + constructor.ToFullString() +
                                                           ". Static constructors are not supported.");
            return new CaDETMember { Type = CaDETMemberType.Constructor, Name = constructor.Identifier.Text };
        }

        private static CaDETMember CreateMethod(MethodDeclarationSyntax method)
        {
            var methodName = GetNameWithInterface(method.ExplicitInterfaceSpecifier, method.Identifier);
            if (method.TypeParameterList == null) return new CaDETMember {Type = CaDETMemberType.Method, Name = methodName};

            methodName += GetTypeParameterNameExtension(method.TypeParameterList);

            return new CaDETMember { Type = CaDETMemberType.Method, Name = methodName };
        }

        private static string GetTypeParameterNameExtension(TypeParameterListSyntax paramList)
        {
            var methodName = "<";
            for (var i = 0; i < paramList.Parameters.Count; i++)
            {
                methodName += paramList.Parameters[i].Identifier.Text;
                if (i != paramList.Parameters.Count - 1) methodName += ", ";
            }
            methodName += ">";
            return methodName;
        }

        private static string GetNameWithInterface(ExplicitInterfaceSpecifierSyntax explicitInterface, SyntaxToken identifier)
        {
            return explicitInterface == null ? identifier.Text : explicitInterface + identifier.Text;
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
                memberParams.Add(new CaDETParameter { Name = symbol.Name, Type = new CaDETLinkedType() {FullType = symbol.ToDisplayString()} });
            }

            return memberParams;
        }

        private CaDETLinkedType GetMethodReturnType()
        {
            if (_cSharpMember is ConstructorDeclarationSyntax) return null;
            var type = _semanticModel.GetTypeInfo(_cSharpMember.DescendantNodes().OfType<TypeSyntax>().First()).Type;
            if (type == null) return new CaDETLinkedType();
            return new CaDETLinkedType() { FullType = type.ToString() };
        }

        private List<CaDETMember> CalculateInvokedMethods(List<CaDETClass> allProjectClasses)
        {
            List<CaDETMember> methods = new List<CaDETMember>();
            var invokedMethods = _cSharpMember.DescendantNodes().OfType<InvocationExpressionSyntax>();
            foreach (var invoked in invokedMethods)
            {
                var symbol = FindSymbol(invoked);
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

        private ISymbol FindSymbol(InvocationExpressionSyntax invoked)
        {
            var symbolInfo = _semanticModel.GetSymbolInfo(invoked.Expression);
            if (symbolInfo.Symbol != null) return symbolInfo.Symbol;

            return symbolInfo.CandidateSymbols.Length > 0 ? symbolInfo.CandidateSymbols.First() : null;
        }

        private List<CaDETField> CalculateAccessedFields(List<CaDETClass> allProjectClasses)
        {
            List<CaDETField> fields = new List<CaDETField>();
            var accessedFields = _semanticModel.GetOperation(_cSharpMember).Descendants().OfType<IFieldReferenceOperation>();
            foreach (var field in accessedFields)
            {
                var fullFieldName = field.Member.ToDisplayString();
                var containingClass = FindContainingClass(allProjectClasses, fullFieldName);
                if(IsEnumeration(containingClass)) continue;
                var accessedField = containingClass.FindField(fullFieldName.Split(_separator).Last());
                if(accessedField == null) continue;
                fields.Add(accessedField);
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

        private List<CaDETMember> CalculateAccessedAccessors(List<CaDETClass> allProjectClasses)
        {
            List<CaDETMember> accessors = new List<CaDETMember>();
            var accessedAccessors = _semanticModel.GetOperation(_cSharpMember).Descendants().OfType<IPropertyReferenceOperation>(); //.DescendantTokens().OfType<IPropertyReferenceOperation>();
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