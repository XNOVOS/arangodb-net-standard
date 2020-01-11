using System;
using ArangoDBNetStandard.DocumentApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArangoDBNetStandard.Serialization
{
    public class GetDocumentResponseJsonConverter<T> : JsonConverter<GetDocumentResponse<T>>
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, GetDocumentResponse<T> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override GetDocumentResponse<T> ReadJson(JsonReader reader, Type objectType, GetDocumentResponse<T> existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            return new GetDocumentResponse<T>(jObject.ToObject<T>());
        }
    }
}