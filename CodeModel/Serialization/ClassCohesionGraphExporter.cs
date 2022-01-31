using CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CodeModel.Serialization
{
    public class ClassCohesionGraphExporter
    {
        public void ExportJSON(CaDETClass caDetClass, string filePath)
        {
            var graph = GetClassCohesionGraph(caDetClass);
            SaveToJSON(graph, filePath);
        }

        internal Dictionary<string, Dictionary<string, bool>> GetClassCohesionGraph(CaDETClass caDetClass)
        {
            var methods = caDetClass.Members.Where(m => m.Type == CaDETMemberType.Method).ToList();
            var allMethodsFieldsAndAccessors = GetAllMethodsFieldsAndAccessors(methods);
            return BuildCohesionGraphConnectionMatrix(methods, allMethodsFieldsAndAccessors);
        }

        private void SaveToJSON(Dictionary<string, Dictionary<string, bool>> graph, string filePath)
        {
            var json = JsonSerializer.Serialize(graph, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        private ISet<string> GetAllMethodsFieldsAndAccessors(List<CaDETMember> methods)
        {
            var retVal = new HashSet<string>();
            foreach (var method in methods)
            {
                retVal.UnionWith(GetAllMethodsFieldsAndAccessors(method));
            }
            return retVal;
        }

        private static ISet<string> GetAllMethodsFieldsAndAccessors(CaDETMember method)
        {
            var retVal = new HashSet<string>();
            retVal.UnionWith(method.GetAccessedOwnFields().Select(f => f.Name));
            retVal.UnionWith(method.GetAccessedOwnAccessors().Select(a => a.Name));
            retVal.UnionWith(method.InvokedMethods.Distinct()
                .Where(i => i.Parent.Equals(method.Parent))
                .Select(m => m.GetMethodNameWithParamTypes()));
            return retVal;
        }

        private static Dictionary<string, Dictionary<string, bool>> BuildCohesionGraphConnectionMatrix(List<CaDETMember> methods, ISet<string> allMethodsFieldsAndAccessors)
        {
            // { "rowName": { "columnName" : connectionExists }
            var retVal = new Dictionary<string, Dictionary<string, bool>>();
            foreach (var method in methods)
            {
                retVal[method.GetMethodNameWithParamTypes()] = new Dictionary<string, bool>();
                var allAccessedItems = GetAllMethodsFieldsAndAccessors(method);
                foreach (var item in allMethodsFieldsAndAccessors)
                {
                    retVal[method.GetMethodNameWithParamTypes()][item] = allAccessedItems.Contains(item);
                }
            }

            return retVal;
        }
    }
}
