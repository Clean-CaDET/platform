using DataSetExplorer.DataSetBuilder.Model;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.DataSetSerializer.ViewModel
{
    public class ColumnHeuristicsModel
    {
        private readonly Dictionary<CodeSmell, List<string>> _heuristics;

        internal ColumnHeuristicsModel()
        {
            _heuristics = new Dictionary<CodeSmell, List<string>>();
            //TODO: Load from DB or config.
            _heuristics.Add(new CodeSmell("Large Class"), LCHeuristics());
            _heuristics.Add(new CodeSmell("Long Method"), LMHeuristics());
        }

        internal List<string> GetHeuristics(CodeSmell smell)
        {
            _heuristics.TryGetValue(smell, out var retVal);
            return retVal;
        }

        internal List<CodeSmell> GetSmells()
        {
            return _heuristics.Keys.ToList();
        }

        private List<string> LMHeuristics()
        {
            var retVal = new List<string>
            {
                "Function is too long.",
                "Function is too complex.",
                "Function does multiple things.",
                //"Different abstraction levels."
            };
            return retVal;
        }

        private List<string> LCHeuristics()
        {
            var retVal = new List<string>
            {
                "Class is too long.",
                "Class is too complex.",
                "Class has multiple concerns."
            };
            return retVal;
        }
    }
}
