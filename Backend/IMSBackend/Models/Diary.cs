using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IMSBackend.Models
{
    public class Diary
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string ClassId { get; set; }

        [JsonConverter(typeof(FlexibleStringConverter))]
        public string Week { get; set; }
        public int? CompletionLevel { get; set; }
        public int? Feeling { get; set; }
        public string TaskDescription { get; set; }
        public string NewKnowledge { get; set; }
        public string Issues { get; set; }
        public string Solutions { get; set; }
        public string NextPlan { get; set; }
        public string SupportNeeded { get; set; }
        public string Evidence { get; set; }
        public string Status { get; set; } = "Draft";
        public bool IsReadByTeacher { get; set; }
        public bool IsReadByStudent { get; set; } = true;
        public DateTime NgayTao { get; set; } = DateTime.UtcNow;
        public DateTime NgayCapNhat { get; set; } = DateTime.UtcNow;
        
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }

    public class Feedback
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string DiaryId { get; set; }
        public string TeacherName { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        [JsonIgnore]
        public Diary Diary { get; set; }
    }

    public class FlexibleStringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.Number => reader.TryGetInt64(out var l) ? l.ToString() : reader.GetDouble().ToString(),
                JsonTokenType.True => "true",
                JsonTokenType.False => "false",
                JsonTokenType.Null => null,
                _ => throw new JsonException($"Unexpected token {reader.TokenType} for string field.")
            };
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
