using SmartTutor.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTutor.Controllers
{
    public class ContentRepositoryService
    {
        public ContentRepositoryService() { }

        public EducationContent GetLongParameterListContent()
        {
            EducationContent educationContent = new EducationContent();

            educationContent.EducationQuaility = 4;
            educationContent.EducationSnippets = new List<EducationSnippet>();

            EducationSnippet snippetOne = new EducationSnippet();
            EducationSnippet snippetTwo = new EducationSnippet();

            snippetOne.SnippetQuaility = 4;
            snippetOne.Tags = new List<Tag>(); snippetOne.Tags.Add(Tag.MustKnow);
            snippetOne.EducationType = EducationType.ShortText;
            snippetOne.Content = "This method have problem with long parameter list";

            snippetTwo.SnippetQuaility = 5;
            snippetTwo.Tags = new List<Tag>(); snippetTwo.Tags.Add(Tag.Interesting);
            snippetTwo.EducationType = EducationType.LongText;
            snippetTwo.Content = "The ideal number of arguments for a function is zero (niladic). Next comes one (monadic), followed closely by two (dyadic). Three arguments (triadic) should be avoided where possible. More than three (polyadic) requires very special justification—and then shouldn’t be used anyway.The argument is at a different level of abstraction than the function name and forces you to know a detail that isn’t particularly important.at that point.";

            educationContent.EducationSnippets.Add(snippetOne);
            educationContent.EducationSnippets.Add(snippetTwo);


            return educationContent;
        }
    }
}
