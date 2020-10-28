using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Structurizr.IO.Json;

namespace Structurizr.Encryption
{
    public class EncryptedJsonReader
    {
        public EncryptedWorkspace Read(StringReader reader)
        {
            var workspace = JsonConvert.DeserializeObject<EncryptedWorkspace>(
                reader.ReadToEnd(),
                new StringEnumConverter(),
                new PaperSizeJsonConverter(),
                new EncryptionStrategyJsonConverter());

            return workspace;
        }
    }
}