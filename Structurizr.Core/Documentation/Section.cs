using System.Runtime.Serialization;

namespace Structurizr.Documentation
{
    [DataContract]
    public sealed class Section
    {
        private string _elementId;

        internal Section()
        {
        }

        internal Section(Element element, string title, int order, Format format, string content)
        {
            Element = element;
            Title = title;
            Order = order;
            Format = format;
            Content = content;
        }

        public Element Element { get; internal set; }

        /// <summary>
        ///     The ID of the element.
        /// </summary>
        [DataMember(Name = "elementId", EmitDefaultValue = false)]
        public string ElementId
        {
            get
            {
                if (Element != null)
                    return Element.Id;
                return _elementId;
            }

            set => _elementId = value;
        }

        [DataMember(Name = "title", EmitDefaultValue = true)]
        public string Title { get; internal set; }

        /// <summary>
        ///     (this is for backwards compatibility with older client libraries)
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = true)]
        internal string SectionType
        {
            set => Title = value;
        }

        [DataMember(Name = "order", EmitDefaultValue = true)]
        public int Order { get; internal set; }

        [DataMember(Name = "format", EmitDefaultValue = true)]
        public Format Format { get; internal set; }

        [DataMember(Name = "content", EmitDefaultValue = false)]
        public string Content { get; internal set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Section);
        }

        public bool Equals(Section section)
        {
            if (section == this) return true;

            if (section == null) return false;

            if (ElementId != null)
                return ElementId.Equals(section.ElementId) && Title == section.Title;
            return Title == section.Title;
        }

        public override int GetHashCode()
        {
            var result = ElementId != null ? ElementId.GetHashCode() : 0;
            result = 31 * result + Title.GetHashCode();
            return result;
        }
    }
}