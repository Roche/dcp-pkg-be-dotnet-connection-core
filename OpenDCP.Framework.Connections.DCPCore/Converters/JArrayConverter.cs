using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;

namespace OpenDCP.Framework.Connections.DCPCore.Converters
{
    public class JArrayConverter : System.Text.Json.Serialization.JsonConverter<JArray>
    {
        public override JArray Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var settings = new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                return JsonConvert.DeserializeObject<JArray>(string.IsNullOrEmpty(jsonDoc.RootElement.GetRawText()) ? "[]" : jsonDoc.RootElement.GetRawText(), settings)!;
            }
        }

        public override void Write(Utf8JsonWriter writer, JArray value, JsonSerializerOptions options)
        {
            var settings = new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            writer.WriteRawValue(JsonConvert.SerializeObject(value, settings));
        }
    }
}
