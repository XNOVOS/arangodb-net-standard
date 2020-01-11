using System;
using System.Collections.Generic;
using System.Linq;
using ArangoDBNetStandard.DocumentApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArangoDBNetStandard.Serialization
{
    public class DeleteDocumentsResponseJsonConverter<T> : JsonConverter<DeleteDocumentsResponse<T>>
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, DeleteDocumentsResponse<T> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override DeleteDocumentsResponse<T> ReadJson(JsonReader reader, Type objectType, DeleteDocumentsResponse<T> existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            List<DeleteDocumentResponse<T>> returnValues = new List<DeleteDocumentResponse<T>>();

            if (reader.TokenType == JsonToken.StartArray)
            {
                JArray jArray = JArray.Load(reader);
                foreach (JToken token in jArray)
                {
                    if (token.Children<JProperty>().Any(x => x.Name == "_id"))
                    {
                        //JsonReader copyReaderForObject = token.CreateReaderWithSettings(reader);
                        //returnValues.Add(serializer.Deserialize<PostDocumentResponse<T>>(copyReaderForObject));
                        returnValues.Add(token.ToObject<DeleteDocumentResponse<T>>());
                    }
                    else
                    {
                        returnValues.Add(new DeleteDocumentResponse<T>(token.ToObject<ApiResponse>()));
                    }
                }
            }
            return new DeleteDocumentsResponse<T>(returnValues);
        }
    }
}