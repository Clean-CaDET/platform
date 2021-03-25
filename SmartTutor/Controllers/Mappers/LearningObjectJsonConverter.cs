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
                writer.WriteString("TypeDiscriminator", "text");
                writer.WriteString("Text", text.Content);
            }
            else if (learningObject is ImageDTO image)
            {
                writer.WriteString("TypeDiscriminator", "image");
                writer.WriteString("Url", image.Url);
                writer.WriteString("Caption", image.Caption);
            }
            else if (learningObject is VideoDTO video)
            {
                writer.WriteString("TypeDiscriminator", "video");
                writer.WriteString("Url", video.Url);
            }

            writer.WriteNumber("Id", learningObject.Id);
            writer.WriteNumber("LearningObjectSummaryId", learningObject.LearningObjectSummaryId);

            writer.WriteEndObject();
        }
    }
}