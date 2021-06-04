namespace DataSetExplorer.DataSetSerializer.ViewModel
{
    public class NewSpreadSheetColumnModel
    {
        public ColumnHeuristicsModel SmellsAndHeuristics { get; }
        public bool IncludeMetrics { get; }

        public NewSpreadSheetColumnModel(ColumnHeuristicsModel smellsAndHeuristics, bool includeMetrics)
        {
            SmellsAndHeuristics = smellsAndHeuristics;
            IncludeMetrics = includeMetrics;
        }

        public NewSpreadSheetColumnModel(): this(new ColumnHeuristicsModel(), false)
        {
        }
    }
}
