using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Structurizr.IO.Json
{
    public class JsonReader
    {
        public Workspace Read(StringReader reader)
        {
            var settings = new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter(),
                    new IsoDateTimeConverter(),
                    new PaperSizeJsonConverter()
                },
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };

            var workspace = JsonConvert.DeserializeObject<Workspace>(reader.ReadToEnd(), settings);
            workspace.Hydrate();

            return workspace;
        }
    }
}