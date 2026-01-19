using SmartTutor.Controllers.Content.DTOs;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartTutor.Controllers.Content.Mappers
{
    //Based on https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-core-3-1#support-polymorphic-deserialization
    //TODO: Research a better solution and refactor this.
    public class LearningObjectJsonConverter : JsonConverter<LearningObjectDTO>
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
                WriteQuestion(writer, question);
            }
            else if (learningObject is ArrangeTaskDTO task)
            {
                WriteArrangeTask(writer, task);
            }

            writer.WriteNumber("id", learningObject.Id);
            writer.WriteNumber("learningObjectSummaryId", learningObject.LearningObjectSummaryId);

            writer.WriteEndObject();
        }

        private static void WriteQuestion(Utf8JsonWriter writer, QuestionDTO question)
        {
            writer.WriteString("typeDiscriminator", "question");
            writer.WriteString("text", question.Text);
            writer.WritePropertyName("possibleAnswers");
            writer.WriteStartArray();
            foreach (var answer in question.PossibleAnswers)
            {
                writer.WriteStartObject();
                writer.WriteNumber("id", answer.Id);
                writer.WriteString("text", answer.Text);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }

        private void WriteArrangeTask(Utf8JsonWriter writer, ArrangeTaskDTO task)
        {
            writer.WriteString("typeDiscriminator", "arrangeTask");
            writer.WriteString("text", task.Text);
            writer.WritePropertyName("containers");

            writer.WriteStartArray();
            foreach (var container in task.Containers)
            {
                writer.WriteStartObject();
                writer.WriteNumber("id", container.Id);
                writer.WriteString("title", container.Title);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WritePropertyName("unarrangedElements");
            writer.WriteStartArray();
            foreach (var element in task.UnarrangedElements)
            {
                writer.WriteStartObject();
                writer.WriteNumber("id", element.Id);
                writer.WriteString("text", element.Text);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
    }
}