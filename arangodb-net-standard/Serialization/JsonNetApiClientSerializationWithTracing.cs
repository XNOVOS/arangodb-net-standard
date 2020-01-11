using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ArangoDBNetStandard.Serialization
{
    public class JsonNetApiClientSerializationWithTracing : JsonNetApiClientSerialization
    {
        private readonly ITraceWriter _traceWriter;

        public JsonNetApiClientSerializationWithTracing(ITraceWriter traceWriter = null)
        {
            _traceWriter = traceWriter ?? new DiagnosticsTraceWriter { LevelFilter = TraceLevel.Verbose };
        }

        protected override JsonSerializerSettings CustomizeJsonSerializerSettingsForSerialization(JsonSerializerSettings currentSettings)
        {
            currentSettings.TraceWriter = _traceWriter;
            return base.CustomizeJsonSerializerSettingsForSerialization(currentSettings);
        }

        protected override JsonSerializer CustomizeJsonSerializerForDeserialization(JsonSerializer currentSerializer)
        {
            currentSerializer.TraceWriter = _traceWriter;
            return base.CustomizeJsonSerializerForDeserialization(currentSerializer);
        }
    }
}