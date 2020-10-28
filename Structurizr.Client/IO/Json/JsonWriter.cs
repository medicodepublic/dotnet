using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Structurizr.IO.Json
{
    public class JsonWriter
    {
        public JsonWriter(bool indentOutput)
        {
            IndentOutput = indentOutput;
        }

        public bool IndentOutput { get; set; }

        public void Write(Workspace workspace, TextWriter writer)
        {
            var json = JsonConvert.SerializeObject(workspace,
                IndentOutput ? Formatting.Indented : Formatting.None,
                new StringEnumConverter(),
                new IsoDateTimeConverter(),
                new PaperSizeJsonConverter());

            writer.Write(json);
        }
    }
}