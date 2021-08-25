using DataSetExplorer.DataSetBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.Controllers.Annotation.DTOs
{
    public class CodeSmellDTO
    {
        public string Value { get; set; }
        public List<SnippetType> SnippetTypes { get; set; }
    }
}
