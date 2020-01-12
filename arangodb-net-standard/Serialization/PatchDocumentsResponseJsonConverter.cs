using System;
using System.Collections.Generic;
using System.Linq;
using ArangoDBNetStandard.DocumentApi.Models;
using ArangoDBNetStandard.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArangoDBNetStandard.Serialization
{
    public class PatchDocumentsResponseJsonConverter<T> : JsonConverter<PatchDocumentsResponse<T>>
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, PatchDocumentsResponse<T> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override PatchDocumentsResponse<T> ReadJson(JsonReader reader, Type objectType, PatchDocumentsResponse<T> existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            List<PatchDocumentResponse<T>> returnValues = new List<PatchDocumentResponse<T>>();

            if (reader.TokenType == JsonToken.StartArray)
            {
                JArray jArray = JArray.Load(reader);
                foreach (JToken token in jArray)
                {
                    if (token.Children<JProperty>().Any(x => x.Name == "_id"))
                    {
                        //JsonReader copyReaderForObject = token.CreateReaderWithSettings(reader);
                        //returnValues.Add(serializer.Deserialize<PostDocumentResponse<T>>(copyReaderForObject));
                        returnValues.Add(token.ToObject<PatchDocumentResponse<T>>());
                    }
                    else
                    {
                        returnValues.Add(new PatchDocumentResponse<T>(token.ToObject<ApiResponse>()));
                    }
                }
            }
            return new PatchDocumentsResponse<T>(returnValues);
        }
    }
}