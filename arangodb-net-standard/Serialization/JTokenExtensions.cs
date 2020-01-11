using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArangoDBNetStandard.Serialization
{
    public static class JTokenExtensions
    {
        public static JsonReader CreateReaderWithSettings(this JToken jToken, JsonReader reader)
        {
            JsonReader jTokenReader = jToken.CreateReader();
            jTokenReader.Culture = reader.Culture;
            jTokenReader.DateFormatString = reader.DateFormatString;
            jTokenReader.DateParseHandling = reader.DateParseHandling;
            jTokenReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            jTokenReader.FloatParseHandling = reader.FloatParseHandling;
            jTokenReader.MaxDepth = reader.MaxDepth;
            jTokenReader.SupportMultipleContent = reader.SupportMultipleContent;
            return jTokenReader;
        }
    }
}