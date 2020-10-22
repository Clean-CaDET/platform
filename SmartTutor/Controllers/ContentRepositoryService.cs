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
            EducationSnippet snippetThree = new EducationSnippet();
            EducationSnippet snippetFour = new EducationSnippet();
            EducationSnippet snippetFive = new EducationSnippet();

            snippetOne.SnippetQuaility = 4;
            snippetOne.Tags = new List<Tag>(); snippetOne.Tags.Add(Tag.MustKnow);
            snippetOne.EducationType = EducationType.ShortText;
            snippetOne.Content = "This method have problem with long parameter list";
            

            snippetTwo.SnippetQuaility = 5;
            snippetTwo.Tags = new List<Tag>(); snippetTwo.Tags.Add(Tag.Interesting);
            snippetTwo.EducationType = EducationType.LongText;
            snippetTwo.Content = "The ideal number of arguments for a function is zero (niladic). Next comes one (monadic), followed closely by two (dyadic). Three arguments (triadic) should be avoided where possible. More than three (polyadic) requires very special justification—and then shouldn’t be used anyway.The argument is at a different level of abstraction than the function name and forces you to know a detail that isn’t particularly important.at that point.";

            snippetThree.SnippetQuaility = 4;
            snippetThree.Tags = new List<Tag>(); snippetThree.Tags.Add(Tag.Interesting);
            snippetThree.EducationType = EducationType.LongText;
            snippetThree.Content = "Argument Objects. When a function seems to need more than two or three arguments, it is likely that some of those arguments ought to be wrapped into a class of their own.Consider, for example, the difference between the two following declarations:";


            snippetFour.SnippetQuaility = 4;
            snippetFour.Tags = new List<Tag>(); snippetFour.Tags.Add(Tag.Interesting);
            snippetFour.EducationType = EducationType.CodeSnippet;
            snippetFour.Content = "Circle makeCircle(double x, double y, double radius);Circle makeCircle(Point center, double radius); ";

            snippetFive.SnippetQuaility = 4;
            snippetFive.Tags = new List<Tag>(); snippetFive.Tags.Add(Tag.MustKnow);
            snippetFive.EducationType = EducationType.Image;
            snippetFive.Content = "https://refactoring.guru/images/refactoring/content/smells/long-parameter-list-01.png";


            educationContent.EducationSnippets.Add(snippetOne);
            educationContent.EducationSnippets.Add(snippetTwo);
            educationContent.EducationSnippets.Add(snippetThree);
            educationContent.EducationSnippets.Add(snippetFour);
            educationContent.EducationSnippets.Add(snippetFive);


            return educationContent;
        }

        public EducationContent GetLongMethod()
        {
            EducationContent educationContent = new EducationContent();

            educationContent.EducationQuaility = 4;
            educationContent.EducationSnippets = new List<EducationSnippet>();

            EducationSnippet snippetOne = new EducationSnippet();
            EducationSnippet snippetTwo = new EducationSnippet();
            EducationSnippet snippetThree = new EducationSnippet();
            EducationSnippet snippetFour = new EducationSnippet();
            EducationSnippet snippetFive = new EducationSnippet();

            snippetOne.SnippetQuaility = 4;
            snippetOne.Tags = new List<Tag>(); snippetOne.Tags.Add(Tag.MustKnow);
            snippetOne.EducationType = EducationType.ShortText;
            snippetOne.Content = "This method have problem with bad smell: Long method";

            snippetTwo.SnippetQuaility = 5;
            snippetTwo.Tags = new List<Tag>(); snippetTwo.Tags.Add(Tag.Interesting);
            snippetTwo.EducationType = EducationType.LongText;
            snippetTwo.Content = "Lines should not be 150 characters long. Functions should not be 100 lines long. Functions should hardly ever be 20 lines long. Every function in this program was just two, or three, or four lines long.Each was transparently obvious.Each told a story. And each led you to the next in a compelling order.That’s how short your functions should be!";

            snippetThree.SnippetQuaility = 3;
            snippetThree.Tags = new List<Tag>(); snippetThree.Tags.Add(Tag.Interesting);
            snippetThree.EducationType = EducationType.ShortText;
            snippetThree.Content = "Blocks and Indenting. This implies that the blocks within if statements, else statements, while statements, and so on should be one line long.Probably that line should be a function call.";

            snippetFour.SnippetQuaility = 4;
            snippetFour.Tags = new List<Tag>(); snippetFour.Tags.Add(Tag.MustKnow);
            snippetFour.EducationType = EducationType.Image;
            snippetFour.Content = "https://refactoring.guru/images/refactoring/content/smells/long-method-01-3x.png";
            
            snippetFive.SnippetQuaility = 4;
            snippetFive.Tags = new List<Tag>(); snippetFive.Tags.Add(Tag.MustKnow);
            snippetFive.EducationType = EducationType.Video;
            snippetFive.Content = "https://www.youtube.com/watch?v=w_SHQFzOosg";


            educationContent.EducationSnippets.Add(snippetOne);
            educationContent.EducationSnippets.Add(snippetTwo);
            educationContent.EducationSnippets.Add(snippetThree);
            educationContent.EducationSnippets.Add(snippetFour);
            educationContent.EducationSnippets.Add(snippetFive);


            return educationContent;
        }
    }
}
