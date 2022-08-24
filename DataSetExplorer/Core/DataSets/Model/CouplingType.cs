namespace DataSetExplorer.Core.DataSets.Model
{
    public enum CouplingType
    {
        Field,
        MethodInvocation,
        Parameter,
        ReturnType,
        Variable,
        Parent,
        AccessedAccessor,
        AccessedField,
        BelongsTo,
        Subclass
    }
}
