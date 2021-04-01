using SmartTutor.Controllers.DTOs.Lecture;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartTutor.Controllers.Mappers
{
    //Based on https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-core-3-1#support-polymorphic-deserialization
    //TODO: Research a better solution and refactor this.
    internal class LearningObjectJsonConverter : JsonConverter<LearningObjectDTO>
    {
        public override bool CanConvert(Type typeToConvert) =>
            typeof(LearningObjectDTO).IsAssignableFrom(typeToConvert);

        public override LearningObjectDTO Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException("Learning objects are only sent, not read");
        }

        public override void Write(
            Utf8JsonWriter writer, LearningObjectDTO learningObject, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (learningObject is TextDTO text)
            {
                writer.WriteString("typeDiscriminator", "text");
                writer.WriteString("content", text.Content);
            }
            else if (learningObject is ImageDTO image)
            {
                writer.WriteString("typeDiscriminator", "image");
                writer.WriteString("url", image.Url);
                writer.WriteString("caption", image.Caption);
            }
            else if (learningObject is VideoDTO video)
            {
                writer.WriteString("typeDiscriminator", "video");
                writer.WriteString("url", video.Url);
            }
            else if (learningObject is ChallengeDTO challenge)
            {
                writer.WriteString("typeDiscriminator", "challenge");
                writer.WriteString("url", challenge.Url);
                writer.WriteString("description", challenge.Description);
            }
            else if (learningObject is QuestionDTO question)
            {
                writer.WriteString("typeDiscriminator", "challenge");
                writer.WriteString("text", question.Text);
                writer.WriteStartArray();
                writer.WriteString("possibleAnswers", string.Join(",", question.PossibleAnswers));
                writer.WriteEndArray();
            }

            writer.WriteNumber("id", learningObject.Id);
            writer.WriteNumber("learningObjectSummaryId", learningObject.LearningObjectSummaryId);

            writer.WriteEndObject();
        }
    }
}