using System.IO;
using Structurizr.IO.Json;
using Xunit;

namespace Structurizr.Api.Tests.IO
{
    public class JsonReaderTests
    {
        [Fact]
        public void Test_DeserializationOfProperties()
        {
            var workspace = new Workspace("Name", "Description");
            var softwareSystem = workspace.Model.AddSoftwareSystem("Name", "Description");
            softwareSystem.AddProperty("Name", "Value");

            var stringWriter = new StringWriter();
            new JsonWriter(false).Write(workspace, stringWriter);

            var stringReader = new StringReader(stringWriter.ToString());
            workspace = new JsonReader().Read(stringReader);
            Assert.Equal("Value", workspace.Model.GetSoftwareSystemWithName("Name").Properties["Name"]);
        }
    }
}