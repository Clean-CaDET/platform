using System.ComponentModel;

namespace RepositoryCompiler.CodeModel.CaDETModel.CodeItems
{
    public class CaDETModifier
    {
        public CaDETModifierValue Value { get; set; }

        public CaDETModifier(string modifier)
        {
            Value = modifier switch
            {
                "public" => CaDETModifierValue.Public,
                "private" => CaDETModifierValue.Private,
                "internal" => CaDETModifierValue.Internal,
                "protected" => CaDETModifierValue.Protected,
                "virtual" => CaDETModifierValue.Virtual,
                "abstract" => CaDETModifierValue.Abstract,
                "static" => CaDETModifierValue.Static,
                "unsafe" => CaDETModifierValue.Unsafe,
                "override" => CaDETModifierValue.Override,
                "extern" => CaDETModifierValue.Extern,
                "new" => CaDETModifierValue.New,
                "readonly" => CaDETModifierValue.ReadOnly,
                "sealed" => CaDETModifierValue.Sealed,
                "const" => CaDETModifierValue.Const,
                "partial" => CaDETModifierValue.Partial,
                _ => throw new InvalidEnumArgumentException(modifier)
            };
        }
    }
    public enum CaDETModifierValue
    {
        Public,
        Private,
        Internal,
        Protected,
        Virtual,
        Abstract,
        Static,
        Unsafe,
        Override,
        Extern,
        New,
        ReadOnly,
        Sealed,
        Const,
        Partial
    }
}