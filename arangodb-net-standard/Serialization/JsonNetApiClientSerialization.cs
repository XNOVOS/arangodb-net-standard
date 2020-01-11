using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Text;
using ArangoDBNetStandard.CollectionApi.Models;
using ArangoDBNetStandard.DocumentApi.Models;
using Newtonsoft.Json.Linq;

namespace ArangoDBNetStandard.Serialization
{
    public class GetCollectionsResponseConverter : JsonConverter<GetCollectionsResponse>
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, GetCollectionsResponse value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override GetCollectionsResponse ReadJson(JsonReader reader, Type objectType, GetCollectionsResponse existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            if (jObject.ContainsKey("result"))
            {
                 //jObject["result"];
            }
            return new GetCollectionsResponse(null);
        }
    }
    /// <summary>
    /// Implements a <see cref="IApiClientSerialization"/> that uses Json.NET.
    /// </summary>
    public class JsonNetApiClientSerialization : IApiClientSerialization
    {
        /// <summary>
        /// Deserializes the JSON structure contained by the specified stream
        /// into an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="stream">The stream containing the JSON structure to deserialize.</param>
        /// <returns></returns>
        public virtual T DeserializeFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
            {
                return default(T);
            }

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = CustomizeJsonSerializerForDeserialization(new JsonSerializer());

                if (typeof(T).IsGenericType)
                {
                    Type genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(PostDocumentsResponse<>))
                    {
                        Type classType = typeof(T).GetGenericArguments()[0];
                        JsonConverter responseConverter = Activator.CreateInstance(typeof(PostDocumentsResponseJsonConverter<>).MakeGenericType(classType)) as JsonConverter;
                        js.Converters.Add(responseConverter);
                    }
                    else if (genericTypeDefinition == typeof(DeleteDocumentsResponse<>))
                    {
                        Type classType = typeof(T).GetGenericArguments()[0];
                        JsonConverter responseConverter = Activator.CreateInstance(typeof(DeleteDocumentsResponseJsonConverter<>).MakeGenericType(classType)) as JsonConverter;
                        js.Converters.Add(responseConverter);
                    }
                    else if (genericTypeDefinition == typeof(PatchDocumentsResponse<>))
                    {
                        Type classType = typeof(T).GetGenericArguments()[0];
                        JsonConverter responseConverter = Activator.CreateInstance(typeof(PatchDocumentsResponseJsonConverter<>).MakeGenericType(classType)) as JsonConverter;
                        js.Converters.Add(responseConverter);
                    }
                    else if (genericTypeDefinition == typeof(GetDocumentResponse<>))
                    {
                        Type classType = typeof(T).GetGenericArguments()[0];
                        JsonConverter responseConverter = Activator.CreateInstance(typeof(GetDocumentResponseJsonConverter<>).MakeGenericType(classType)) as JsonConverter;
                        js.Converters.Add(responseConverter);
                    }
                }

                T result = js.Deserialize<T>(jtr);

                return result;
            }
        }

        /// <summary>
        /// Serializes the specified object to a JSON string encoded as UTF-8 bytes,
        /// following the provided rules for camel case property name and null value handling.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="item">The object to serialize.</param>
        /// <param name="useCamelCasePropertyNames">Whether property names should be camel cased (camelCase).</param>
        /// <param name="ignoreNullValues">Whether null values should be ignored.</param>
        /// <returns></returns>
        public virtual byte[] Serialize<T>(
            T item,
            bool useCamelCasePropertyNames,
            bool ignoreNullValues)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = ignoreNullValues ? NullValueHandling.Ignore : NullValueHandling.Include
            };

            if (useCamelCasePropertyNames)
            {
                jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            jsonSettings = CustomizeJsonSerializerSettingsForSerialization(jsonSettings);

            string json = JsonConvert.SerializeObject(item, jsonSettings);

            return Encoding.UTF8.GetBytes(json);
        }

        protected virtual JsonSerializerSettings CustomizeJsonSerializerSettingsForSerialization(JsonSerializerSettings currentSettings)
        {
            return currentSettings;
        }
        protected virtual JsonSerializer CustomizeJsonSerializerForDeserialization(JsonSerializer currentSerializer)
        {
            return currentSerializer;
        }
    }
}
