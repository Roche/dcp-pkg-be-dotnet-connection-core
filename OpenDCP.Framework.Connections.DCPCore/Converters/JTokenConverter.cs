using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;

namespace OpenDCP.Framework.Connections.DCPCore.Converters
{
    public class JTokenConverter : System.Text.Json.Serialization.JsonConverter<JToken>
    {
        public override JToken? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var settings = new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                return JsonConvert.DeserializeObject<JToken>(jsonDoc.RootElement.GetRawText(), settings);
            }
        }

        public override void Write(Utf8JsonWriter writer, JToken value, JsonSerializerOptions options)
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
