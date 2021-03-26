using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SmartTutor.Controllers.DTOs.Lecture;

namespace SmartTutor.Controllers.Mappers
{
    //Based on https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-core-3-1#support-polymorphic-deserialization
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

            writer.WriteNumber("id", learningObject.Id);
            writer.WriteNumber("learningObjectSummaryId", learningObject.LearningObjectSummaryId);

            writer.WriteEndObject();
        }
    }
}