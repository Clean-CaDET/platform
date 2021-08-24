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
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            reader.Read();

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string propertyName = reader.GetString();
            if (propertyName != "TypeDiscriminator")
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            string typeDiscriminator = reader.GetString();

            LearningObjectDTO dto = typeDiscriminator switch
            {
                "text" => new TextDTO(),
                "image" => new ImageDTO(),
                "video" => new VideoDTO(),
                _ => throw new JsonException()
            };

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return dto;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case ("url"):
                            string url = reader.GetString();
                            switch (dto)
                            {
                                case ImageDTO imageDto:
                                    imageDto.Url = url;
                                    break;
                                case VideoDTO videoDto:
                                    videoDto.Url = url;
                                    break;
                            }

                            break;
                        case ("caption"):
                            var caption = reader.GetString();
                            ((ImageDTO) dto).Caption = caption;
                            break;
                        case ("content"):
                            var content = reader.GetString();
                            ((TextDTO) dto).Content = content;
                            break;
                        case ("learningObjectSummaryId"):
                            var learningObjectSummaryId = reader.GetInt32();
                            dto.LearningObjectSummaryId = learningObjectSummaryId;
                            break;
                        case ("id"):
                            var id = reader.GetInt32();
                            dto.Id = id;
                            break;
                    }
                }
            }

            throw new JsonException();
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