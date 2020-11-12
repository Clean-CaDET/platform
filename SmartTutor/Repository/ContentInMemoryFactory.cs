using SmartTutor.ContentModel;
using System.Collections.Generic;

namespace SmartTutor.Repository
{
    public class ContentInMemoryFactory
    {
        public Dictionary<SmellType, List<EducationalContent>> createContent()
        {
            Dictionary<SmellType, List<EducationalContent>> educationContents = new Dictionary<SmellType, List<EducationalContent>>();

            List<EducationalContent> longMethodContents = new List<EducationalContent>();
            List<EducationalContent> longParameterListContents = new List<EducationalContent>();
            List<EducationalContent> godClassContents = new List<EducationalContent>();


            longMethodContents.Add(CreateLongMethodContent());
            longParameterListContents.Add(CreateLongParameterListContent());
            godClassContents.Add(CreateGodClassContent());


            educationContents.Add(SmellType.LONG_METHOD, longMethodContents);
            educationContents.Add(SmellType.LONG_PARAM_LISTS, longParameterListContents);
            educationContents.Add(SmellType.GOD_CLASS, godClassContents);

            return educationContents;
        }

        private EducationalContent CreateLongMethodContent()
        {
            EducationalContent educationContent = new EducationalContent();

            educationContent.ContentQuality = 4;
            educationContent.ContentDifficulty = 4;
            educationContent.EducationSnippets = new List<EducationalSnippet>
            {
                CreateLMShortTextSnippet(),
                CreateLMLongTextSnippet(),
                CreateLMImageSnippet(),
                CreateLMVideoSnippet(),
            };

            return educationContent;

        }

        private EducationalContent CreateLongParameterListContent()
        {
            EducationalContent educationContent = new EducationalContent();

            educationContent.ContentQuality = 4;
            educationContent.ContentDifficulty = 5;
            educationContent.EducationSnippets = new List<EducationalSnippet>
            {
                CreateLPSShortTextSnippet(),
                CreateLPSLongTextSnippet(),
                CreateLPSImageSnippet(),
                CreateLPSVideoSnippet(),
                CreateLPSCodeSnippet()
            };

            return educationContent;
        }

        private EducationalContent CreateGodClassContent()
        {
            EducationalContent educationContent = new EducationalContent();

            educationContent.ContentQuality = 4;
            educationContent.ContentDifficulty = 5;
            educationContent.EducationSnippets = new List<EducationalSnippet>
            {
                CreateGodClassShortTextSnippet(),
                CreateGodClassLongTextSnippet(),
                CreateGodClassImageSnippet(),
                CreateGodClassVideoSnippet(),
                CreateGodClassCodeSnippet()
            };

            return educationContent;
        }

        private EducationalSnippet CreateGodClassCodeSnippet()
        {
            EducationalSnippet codeSnippet = new EducationalSnippet();
            codeSnippet.SnippetQuality = 4;
            codeSnippet.Tags = new List<Tag>();
            codeSnippet.Tags.Add(Tag.Interesting);
            codeSnippet.SnippetType = SnippetType.CodeSnippet;
            codeSnippet.Content =
                "Circle makeCircle(double x, double y, double radius);Circle makeCircle(Point center, double radius); ";
            codeSnippet.SnippetDifficulty = 5;
            return codeSnippet;
        }

        private EducationalSnippet CreateGodClassVideoSnippet()
        {
            EducationalSnippet videoSnippet = new EducationalSnippet();
            videoSnippet.SnippetQuality = 4;
            videoSnippet.Tags = new List<Tag>();
            videoSnippet.Tags.Add(Tag.MustKnow);
            videoSnippet.SnippetType = SnippetType.Video;
            videoSnippet.Content = "https://www.youtube.com/watch?v=w_SHQFzOosg";
            videoSnippet.SnippetDifficulty = 4;
            return videoSnippet;
        }

        private EducationalSnippet CreateGodClassImageSnippet()
        {
            EducationalSnippet imageSnippet = new EducationalSnippet();
            imageSnippet.SnippetQuality = 2;
            imageSnippet.Tags = new List<Tag>();
            imageSnippet.Tags.Add(Tag.Interesting);
            imageSnippet.SnippetType = SnippetType.Image;
            imageSnippet.Content = "https://refactoring.guru/images/refactoring/content/smells/large-class-01.png";
            imageSnippet.SnippetDifficulty = 1;
            return imageSnippet;
        }

        private EducationalSnippet CreateGodClassLongTextSnippet()
        {
            EducationalSnippet longTextSnippet = new EducationalSnippet();
            longTextSnippet.SnippetQuality = 5;
            longTextSnippet.Tags = new List<Tag>();
            longTextSnippet.Tags.Add(Tag.MustKnow);
            longTextSnippet.SnippetType = SnippetType.LongText;
            longTextSnippet.Content =
                "The God class smell occurs when a huge class which is surrounded by many data classes acts as a controller (i.e. takes most of the decisions and monopolises the functionality offered by the software). The class defines many data members and methods and exhibits low cohesion.";
            longTextSnippet.SnippetDifficulty = 4;
            return longTextSnippet;

        }

        private EducationalSnippet CreateGodClassShortTextSnippet()
        {
            EducationalSnippet shortTextSnippet = new EducationalSnippet();
            shortTextSnippet.SnippetQuality = 3;
            shortTextSnippet.Tags = new List<Tag>();
            shortTextSnippet.Tags.Add(Tag.Interesting);
            shortTextSnippet.SnippetType = SnippetType.ShortText;
            shortTextSnippet.Content =
                "When a class is trying to do too much, it often shows up as too many fields. When a class has too many fields, duplicated code cannot be far behind.";
            shortTextSnippet.SnippetDifficulty = 2;
            return shortTextSnippet;
        }
            /// <summary>
            /// LM - LongMethod
            /// </summary>
            /// <returns></returns>
        private EducationalSnippet CreateLMShortTextSnippet()
        {
            EducationalSnippet shortTextSnippet = new EducationalSnippet();
            shortTextSnippet.SnippetQuality = 3;
            shortTextSnippet.Tags = new List<Tag>();
            shortTextSnippet.Tags.Add(Tag.Interesting);
            shortTextSnippet.SnippetType = SnippetType.ShortText;
            shortTextSnippet.Content =
                "Blocks and Indenting. This implies that the blocks within if statements, else statements, while statements, and so on should be one line long.Probably that line should be a function call.";
            shortTextSnippet.SnippetDifficulty = 2;
            return shortTextSnippet;
        }

        /// <summary>
        /// LM - LongMethod
        /// </summary>
        /// <returns></returns>
        private EducationalSnippet CreateLMLongTextSnippet()
        {
            EducationalSnippet longTextSnippet = new EducationalSnippet();
            longTextSnippet.SnippetQuality = 5;
            longTextSnippet.Tags = new List<Tag>();
            longTextSnippet.Tags.Add(Tag.Interesting);
            longTextSnippet.SnippetType = SnippetType.LongText;
            longTextSnippet.Content =
                "Lines should not be 150 characters long. Functions should not be 100 lines long. Functions should hardly ever be 20 lines long. Every function in this program was just two, or three, or four lines long.Each was transparently obvious.Each told a story. And each led you to the next in a compelling order.That’s how short your functions should be!";
            longTextSnippet.SnippetDifficulty = 3;
            return longTextSnippet;
        }

        /// <summary>
        /// LM - LongMethod
        /// </summary>
        /// <returns></returns>
        private EducationalSnippet CreateLMImageSnippet()
        {
            EducationalSnippet imageSnippet = new EducationalSnippet();
            imageSnippet.SnippetQuality = 4;
            imageSnippet.Tags = new List<Tag>();
            imageSnippet.Tags.Add(Tag.MustKnow);
            imageSnippet.SnippetType = SnippetType.Image;
            imageSnippet.Content = "https://refactoring.guru/images/refactoring/content/smells/long-method-01-3x.png";
            imageSnippet.SnippetDifficulty = 4;
            return imageSnippet;
        }

        /// <summary>
        /// LM - LongMethod
        /// </summary>
        /// <returns></returns>
        private EducationalSnippet CreateLMVideoSnippet()
        {
            EducationalSnippet videoSnippet = new EducationalSnippet();
            videoSnippet.SnippetQuality = 4;
            videoSnippet.Tags = new List<Tag>();
            videoSnippet.Tags.Add(Tag.MustKnow);
            videoSnippet.SnippetType = SnippetType.Video;
            videoSnippet.Content = "https://www.youtube.com/watch?v=w_SHQFzOosg";
            videoSnippet.SnippetDifficulty = 4;
            return videoSnippet;
        }

        /// <summary>
        /// LPS - LongParameterList
        /// </summary>
        /// <returns></returns>
        private EducationalSnippet CreateLPSShortTextSnippet()
        {
            EducationalSnippet snippet = new EducationalSnippet
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

        /// <summary>
        /// LPS - LongParameterList
        /// </summary>
        /// <returns></returns>
        private EducationalSnippet CreateLPSLongTextSnippet()
        {
            EducationalSnippet snippet = new EducationalSnippet();
            snippet.SnippetQuality = 5;
            snippet.Tags = new List<Tag>();
            snippet.Tags.Add(Tag.Interesting);
            snippet.SnippetType = SnippetType.LongText;
            snippet.Content =
                "The ideal number of arguments for a function is zero (niladic). Next comes one (monadic), followed closely by two (dyadic). Three arguments (triadic) should be avoided where possible. More than three (polyadic) requires very special justification—and then shouldn’t be used anyway.The argument is at a different level of abstraction than the function name and forces you to know a detail that isn’t particularly important.at that point.";
            snippet.SnippetDifficulty = 3;
            return snippet;
        }

        /// <summary>
        /// LPS - LongParameterList
        /// </summary>
        /// <returns></returns>
        private EducationalSnippet CreateLPSImageSnippet()
        {
            EducationalSnippet snippetFive = new EducationalSnippet();
            snippetFive.SnippetQuality = 4;
            snippetFive.Tags = new List<Tag>();
            snippetFive.Tags.Add(Tag.MustKnow);
            snippetFive.SnippetType = SnippetType.Image;
            snippetFive.Content = "https://refactoring.guru/images/refactoring/content/smells/long-parameter-list-01.png";
            snippetFive.SnippetDifficulty = 5;
            return snippetFive;
        }

        /// <summary>
        /// LPS - LongParameterList
        /// </summary>
        /// <returns></returns>
        private EducationalSnippet CreateLPSVideoSnippet()
        {
            EducationalSnippet videoSnippet = new EducationalSnippet();
            videoSnippet.SnippetQuality = 4;
            videoSnippet.Tags = new List<Tag>();
            videoSnippet.Tags.Add(Tag.MustKnow);
            videoSnippet.SnippetType = SnippetType.Video;
            videoSnippet.Content = "https://www.youtube.com/watch?v=w_SHQFzOosg";
            videoSnippet.SnippetDifficulty = 4;
            return videoSnippet;
        }

        /// <summary>
        /// LPS - LongParameterList
        /// </summary>
        /// <returns></returns>
        private EducationalSnippet CreateLPSCodeSnippet()
        {
            EducationalSnippet snippetFour = new EducationalSnippet();
            snippetFour.SnippetQuality = 4;
            snippetFour.Tags = new List<Tag>();
            snippetFour.Tags.Add(Tag.Interesting);
            snippetFour.SnippetType = SnippetType.CodeSnippet;
            snippetFour.Content =
                "Circle makeCircle(double x, double y, double radius);Circle makeCircle(Point center, double radius); ";
            snippetFour.SnippetDifficulty = 5;
            return snippetFour;
        }
    }
}
