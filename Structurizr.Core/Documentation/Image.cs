using System.Runtime.Serialization;

namespace Structurizr.Documentation
{
    [DataContract]
    public sealed class Image
    {
        internal Image()
        {
        }

        internal Image(string name, string content, string type)
        {
            Name = name;
            Content = content;
            Type = type;
        }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; internal set; }

        [DataMember(Name = "content", EmitDefaultValue = false)]
        public string Content { get; private set; }

        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; private set; }
    }
}