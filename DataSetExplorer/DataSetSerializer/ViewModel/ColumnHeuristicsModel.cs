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
            _heuristics.Add(new CodeSmell("Feature Envy"), FEHeuristics());
            _heuristics.Add(new CodeSmell("Data Class"), DCHeuristics());
            _heuristics.Add(new CodeSmell("Refused Bequest"), RBHeuristics());
        }

        internal List<string> GetHeuristics(CodeSmell smell)
        {
            _heuristics.TryGetValue(smell, out var retVal);
            return retVal;
        }

        internal List<string> GetHeuristicsByCodeSmellName(string smellName)
        {
            return _heuristics.First(h => h.Key.Name.Equals(smellName)).Value; 
        }

        internal List<CodeSmell> GetSmells()
        {
            return _heuristics.Keys.ToList();
        }

        private static List<string> LMHeuristics()
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

        private static List<string> LCHeuristics()
        {
            var retVal = new List<string>
            {
                "Class is too long.",
                "Class is too complex.",
                "Class has multiple concerns."
            };
            return retVal;
        }

        private static List<string> FEHeuristics()
        {
            var retVal = new List<string>
            {
                "Function has method chains.",
                "Function uses foreign data.",
                "Function has few foreign providers.",
                "Function does not belong here (semantic)."
            };
            return retVal;
        }

        private static List<string> DCHeuristics()
        {
            var retVal = new List<string>
            {
                "No logic in methods.",
                "Not DTO, DAO,..."
            };
            return retVal;
        }

        private static List<string> RBHeuristics()
        {
            var retVal = new List<string>
            {
                "Few parent members used.", 
                "Many members overriden.", 
                "Unnecessary hierarchy."
            };
            return retVal;
        }
    }
}
