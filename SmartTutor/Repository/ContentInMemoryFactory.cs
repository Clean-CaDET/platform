using SmartTutor.ContentModel;
using System;
using System.Collections.Generic;

namespace SmartTutor.Repository
{
    public class ContentInMemoryFactory
    {
        public Dictionary<SmellType, List<EducationContent>> createContent()
        {
            Dictionary<SmellType, List<EducationContent>> educationContents = new Dictionary<SmellType, List<EducationContent>>();

            List<EducationContent> longMethodContents = new List<EducationContent>();
            List<EducationContent> longParameterListContents = new List<EducationContent>();

            longMethodContents.Add(CreateLongMethodContent());
            longParameterListContents.Add(CreateLongParameterListContent());


            educationContents.Add(SmellType.LONG_METHOD, longMethodContents);
            educationContents.Add(SmellType.LONG_PARAM_LISTS, longParameterListContents);

            return educationContents;
        }

        private EducationContent CreateLongMethodContent()
        {
            EducationContent educationContent = new EducationContent();

            educationContent.ContentQuality = 4;
            educationContent.ContentDifficulty = 4;
            educationContent.EducationSnippets = new List<EducationSnippet>
            {
                CreateLMShortTextSnippet(),
                CreateLMLongTextSnippet(),
                CreateLMImageSnippet(),
                CreateLMVideoSnippet(),
            };

            return educationContent;

        }

        private EducationContent CreateLongParameterListContent()
        {
            EducationContent educationContent = new EducationContent();

            educationContent.ContentQuality = 4;
            educationContent.ContentDifficulty = 5;
            educationContent.EducationSnippets = new List<EducationSnippet>
            {
                CreateLPSShortTextSnippet(),
                CreateLPSLongTextSnippet(),
                CreateLPSImageSnippet(),
                CreateLPSVideoSnippet(),
                CreateLPSCodeSnippet()
            };

            return educationContent;
        }

        /// <summary>
        /// LM - LongMethod
        /// </summary>
        /// <returns></returns>
        private EducationSnippet CreateLMShortTextSnippet()
        {
            EducationSnippet shortTextSnippet = new EducationSnippet();
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
        private EducationSnippet CreateLMLongTextSnippet()
        {
            EducationSnippet longTextSnippet = new EducationSnippet();
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
        private EducationSnippet CreateLMImageSnippet()
        {
            EducationSnippet imageSnippet = new EducationSnippet();
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
        private EducationSnippet CreateLMVideoSnippet()
        {
            EducationSnippet videoSnippet = new EducationSnippet();
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
        private EducationSnippet CreateLPSShortTextSnippet()
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

        /// <summary>
        /// LPS - LongParameterList
        /// </summary>
        /// <returns></returns>
        private EducationSnippet CreateLPSLongTextSnippet()
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

        /// <summary>
        /// LPS - LongParameterList
        /// </summary>
        /// <returns></returns>
        private EducationSnippet CreateLPSImageSnippet()
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

        /// <summary>
        /// LPS - LongParameterList
        /// </summary>
        /// <returns></returns>
        private EducationSnippet CreateLPSVideoSnippet()
        {
            EducationSnippet videoSnippet = new EducationSnippet();
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
        private EducationSnippet CreateLPSCodeSnippet()
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
    }
}
