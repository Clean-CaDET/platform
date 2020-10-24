using System.Collections.Generic;
using SmartTutor.ContentModel;

namespace SmartTutor.Repository.CreateSnippet
{
    public class CreateLongMethod
    {
        public EducationSnippet VideoSnippet()
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

        public EducationSnippet ImageSnippet()
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

        public EducationSnippet ShortTextTwoSnippet()
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

        public EducationSnippet LongTextSnippet()
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

        public EducationSnippet ShortTextSnippet()
        {
            EducationSnippet shortTextSnippet = new EducationSnippet();
            shortTextSnippet.SnippetQuality = 4;
            shortTextSnippet.Tags = new List<Tag>();
            shortTextSnippet.Tags.Add(Tag.MustKnow);
            shortTextSnippet.SnippetType = SnippetType.ShortText;
            shortTextSnippet.Content = "This method have problem with bad smell: Long method";
            shortTextSnippet.SnippetDifficulty = 2;
            return shortTextSnippet;
        }
    }
}