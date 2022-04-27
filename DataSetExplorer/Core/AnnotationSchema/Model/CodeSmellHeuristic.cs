namespace DataSetExplorer.Core.AnnotationSchema.Model
{
    public class CodeSmellHeuristic
    {
        public int CodeSmellDefinitionId { get; set; }
        public CodeSmellDefinition CodeSmellDefinition { get; set; }
        public int HeuristicId { get; set; }
        public HeuristicDefinition Heuristic { get; set; }

        public CodeSmellHeuristic(CodeSmellDefinition codeSmellDefinition, HeuristicDefinition heuristic)
        {
            CodeSmellDefinition = codeSmellDefinition;
            CodeSmellDefinitionId = codeSmellDefinition.Id;
            Heuristic = heuristic;
            HeuristicId = heuristic.Id;
        }

        public CodeSmellHeuristic()
        {
        }
    }
}