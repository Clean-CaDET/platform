using System.Collections.Generic;
using SmartTutor.ContentModel;

namespace SmartTutor.Repository.CreateSnippet
{
    public class CreateLongParameterList
    {
        public EducationSnippet ImageSnippet()
        {
            EducationSnippet snippetFive = new EducationSnippet();
            snippetFive.SnippetQuality = 4;
            snippetFive.Tags = new List<Tag>();
            snippetFive.Tags.Add(Tag.MustKnow);
            snippetFive.SnippetType = SnippetType.Image;
            snippetFive.Content = "https://refactoring.guru/images/refactoring/content/smells/long-parameter-list-01.png";
            snippetFive.SnippetDifficulty = 5;
            return snippetFive;
        }

        public EducationSnippet CodeSnippet()
        {
            EducationSnippet snippetFour = new EducationSnippet();
            snippetFour.SnippetQuality = 4;
            snippetFour.Tags = new List<Tag>();
            snippetFour.Tags.Add(Tag.Interesting);
            snippetFour.SnippetType = SnippetType.CodeSnippet;
            snippetFour.Content =
                "Circle makeCircle(double x, double y, double radius);Circle makeCircle(Point center, double radius); ";
            snippetFour.SnippetDifficulty = 5;
            return snippetFour;
        }

        public EducationSnippet LongTexTwoSnippet()
        {
            EducationSnippet snippet = new EducationSnippet();
            snippet.SnippetQuality = 4;
            snippet.Tags = new List<Tag>();
            snippet.Tags.Add(Tag.Interesting);
            snippet.SnippetType = SnippetType.LongText;
            snippet.Content =
                "Argument Objects. When a function seems to need more than two or three arguments, it is likely that some of those arguments ought to be wrapped into a class of their own.Consider, for example, the difference between the two following declarations:";
            snippet.SnippetDifficulty = 4;

            return snippet;
        }

        public EducationSnippet LongTextSnippet()
        {
            EducationSnippet snippet = new EducationSnippet();
            snippet.SnippetQuality = 5;
            snippet.Tags = new List<Tag>();
            snippet.Tags.Add(Tag.Interesting);
            snippet.SnippetType = SnippetType.LongText;
            snippet.Content =
                "The ideal number of arguments for a function is zero (niladic). Next comes one (monadic), followed closely by two (dyadic). Three arguments (triadic) should be avoided where possible. More than three (polyadic) requires very special justification—and then shouldn’t be used anyway.The argument is at a different level of abstraction than the function name and forces you to know a detail that isn’t particularly important.at that point.";
            snippet.SnippetDifficulty = 3;
            return snippet;
        }

        public EducationSnippet ShortTextSnippet()
        {
            EducationSnippet snippet = new EducationSnippet
            {
                SnippetQuality = 4,
                Tags = new List<Tag>()
            };
            snippet.Tags.Add(Tag.MustKnow);
            snippet.SnippetType = SnippetType.ShortText;
            snippet.SnippetDifficulty = 3;
            snippet.Content = "This method have problem with long parameter list";
            return snippet;
        }
    }
}