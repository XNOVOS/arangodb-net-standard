using System;
using System.Collections.Generic;
using System.Linq;
using ArangoDBNetStandard.DocumentApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArangoDBNetStandard.Serialization
{
    public class PostDocumentsResponseJsonConverter<T> : JsonConverter<PostDocumentsResponse<T>>
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, PostDocumentsResponse<T> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override PostDocumentsResponse<T> ReadJson(JsonReader reader, Type objectType, PostDocumentsResponse<T> existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            List<PostDocumentResponse<T>> returnValues = new List<PostDocumentResponse<T>>();

            if (reader.TokenType == JsonToken.StartArray)
            {
                JArray jArray = JArray.Load(reader);
                foreach (JToken token in jArray)
                {
                    if (token.Children<JProperty>().Any(x => x.Name == "_id"))
                    {
                        //JsonReader copyReaderForObject = token.CreateReaderWithSettings(reader);
                        //returnValues.Add(serializer.Deserialize<PostDocumentResponse<T>>(copyReaderForObject));
                        returnValues.Add(token.ToObject<PostDocumentResponse<T>>());
                    }
                    else
                    {
                        returnValues.Add(new PostDocumentResponse<T>(token.ToObject<ApiResponse>()));
                    }
                }
            }
            return new PostDocumentsResponse<T>(returnValues);
        }
    }
}