using SmartTutor.ContentModel;
using System;
using System.Collections.Generic;

namespace SmartTutor.Repository
{
    public class ContentInMemoryFactory
    {
        public Dictionary<SmellType, List<EducationContent>> CreateContent()
        {
            Dictionary<SmellType, List<EducationContent>> educationContents = new Dictionary<SmellType, List<EducationContent>>();

            List<EducationContent> longMethodContents = new List<EducationContent>();
            List<EducationContent> longParameterListContents = new List<EducationContent>();
            List<EducationContent> godClassContents = new List<EducationContent>();


            longMethodContents.Add(CreateLongMethodContent());
            longParameterListContents.Add(CreateLongParameterListContent());
            godClassContents.Add(CreateGodClassContent());


            educationContents.Add(SmellType.LONG_METHOD, longMethodContents);
            educationContents.Add(SmellType.LONG_PARAM_LISTS, longParameterListContents);
            educationContents.Add(SmellType.GOD_CLASS, godClassContents);

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

        private EducationContent CreateGodClassContent()
        {
            EducationContent educationContent = new EducationContent();

            educationContent.ContentQuality = 4;
            educationContent.ContentDifficulty = 5;
            educationContent.EducationSnippets = new List<EducationSnippet>
            {
                CreateGodClassShortTextSnippet(),
                CreateGodClassLongTextSnippet(),
                CreateGodClassImageSnippet(),
                CreateGodClassVideoSnippet(),
                CreateGodClassCodeSnippet()
            };

            return educationContent;
        }

        private EducationSnippet CreateGodClassCodeSnippet()
        {
            EducationSnippet codeSnippet = new EducationSnippet();
            codeSnippet.SnippetQuality = 4;
            codeSnippet.Tags = new List<Tag>();
            codeSnippet.Tags.Add(Tag.Interesting);
            codeSnippet.SnippetType = SnippetType.CodeSnippet;
            codeSnippet.Content =
                "Circle makeCircle(double x, double y, double radius);Circle makeCircle(Point center, double radius); ";
            codeSnippet.SnippetDifficulty = 5;
            return codeSnippet;
        }

        private EducationSnippet CreateGodClassVideoSnippet()
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

        private EducationSnippet CreateGodClassImageSnippet()
        {
            EducationSnippet imageSnippet = new EducationSnippet();
            imageSnippet.SnippetQuality = 2;
            imageSnippet.Tags = new List<Tag>();
            imageSnippet.Tags.Add(Tag.Interesting);
            imageSnippet.SnippetType = SnippetType.Image;
            imageSnippet.Content = "https://cdn.activestate.com/wp-content/uploads/2018/10/technical-debt-ceos-perspective.jpg";
            imageSnippet.SnippetDifficulty = 1;
            return imageSnippet;
        }

        private EducationSnippet CreateGodClassLongTextSnippet()
        {
            EducationSnippet longTextSnippet = new EducationSnippet();
            longTextSnippet.SnippetQuality = 5;
            longTextSnippet.Tags = new List<Tag>();
            longTextSnippet.Tags.Add(Tag.MustKnow);
            longTextSnippet.SnippetType = SnippetType.LongText;
            longTextSnippet.Content =
                "The God class smell occurs when a huge class which is surrounded by many data classes acts as a controller (i.e. takes most of the decisions and monopolises the functionality offered by the software). The class defines many data members and methods and exhibits low cohesion.";
            longTextSnippet.SnippetDifficulty = 4;
            return longTextSnippet;

        }

        private EducationSnippet CreateGodClassShortTextSnippet()
        {
            EducationSnippet shortTextSnippet = new EducationSnippet();
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
            imageSnippet.Content = "https://www.zaraffasoft.com/wp-content/uploads/2016/09/rsz_104572068.jpg";
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
            snippetFive.Content = "https://stackify.com/wp-content/uploads/2017/05/DotNet-Developers-Headers-min.jpg";
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
