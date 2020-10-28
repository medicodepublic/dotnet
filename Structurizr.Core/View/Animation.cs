using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Structurizr
{
    [DataContract]
    public sealed class Animation
    {
        private HashSet<string> _elements;

        private HashSet<string> _relationships;

        internal Animation()
        {
            _elements = new HashSet<string>();
            _relationships = new HashSet<string>();
        }

        internal Animation(int order, ISet<Element> elements, ISet<Relationship> relationships) : this()
        {
            Order = order;

            foreach (var element in elements) _elements.Add(element.Id);

            foreach (var relationship in relationships) _relationships.Add(relationship.Id);
        }

        [DataMember(Name = "order", EmitDefaultValue = false)]
        public int Order { get; internal set; }

        [DataMember(Name = "elements", EmitDefaultValue = false)]
        public ISet<string> Elements
        {
            get => new HashSet<string>(_elements);

            internal set => _elements = new HashSet<string>(value);
        }

        [DataMember(Name = "relationships", EmitDefaultValue = false)]
        public ISet<string> Relationships
        {
            get => new HashSet<string>(_relationships);

            internal set => _relationships = new HashSet<string>(value);
        }
    }
}