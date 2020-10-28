using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Structurizr.IO.Json;

namespace Structurizr.Encryption
{
    public class EncryptedJsonWriter
    {
        public EncryptedJsonWriter(bool indentOutput)
        {
            IndentOutput = indentOutput;
        }

        public bool IndentOutput { get; set; }

        public void Write(EncryptedWorkspace workspace, StringWriter writer)
        {
            var json = JsonConvert.SerializeObject(workspace,
                IndentOutput ? Formatting.Indented : Formatting.None,
                new StringEnumConverter(),
                new PaperSizeJsonConverter());

            writer.WriteLine(json);
        }
    }
}