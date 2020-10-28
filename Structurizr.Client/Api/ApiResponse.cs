using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Structurizr.Api
{
    [DataContract]
    internal sealed class ApiResponse
    {
        [DataMember(Name = "message", EmitDefaultValue = false)]
        internal string Message;

        [DataMember(Name = "revision", EmitDefaultValue = false)]
        internal long? Revision;

        [DataMember(Name = "success", EmitDefaultValue = false)]
        internal bool Success;

        internal static ApiResponse Parse(string json)
        {
            var settings = new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                Converters = new List<JsonConverter>
                {
                    new IsoDateTimeConverter()
                }
            };

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(json, settings);
            return apiResponse;
        }
    }
}