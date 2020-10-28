using System;
using System.Runtime.Serialization;

namespace Structurizr
{
    /// <summary>
    ///     An instance of a model element (Person, Software System, Container or Component) in a View.
    /// </summary>
    [DataContract]
    public sealed class ElementView : ModelItemView, IEquatable<ElementView>
    {
        private string id;

        internal ElementView()
        {
        }

        internal ElementView(Element element)
        {
            Element = element;
        }

        public Element Element { get; set; }

        /// <summary>
        ///     The ID of the element.
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id
        {
            get
            {
                if (Element != null)
                    return Element.Id;
                return id;
            }

            set => id = value;
        }

        /// <summary>
        ///     The horizontal position of the element when rendered.
        /// </summary>
        [DataMember(Name = "x", EmitDefaultValue = false)]
        public int? X { get; set; }

        /// <summary>
        ///     The vertical position of the element when rendered.
        /// </summary>
        [DataMember(Name = "y", EmitDefaultValue = false)]
        public int? Y { get; set; }

        public bool Equals(ElementView elementView)
        {
            if (elementView == null) return false;
            if (elementView == this) return true;

            return Id == elementView.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ElementView);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            if (Element != null)
                return Element.ToString();
            return Id;
        }

        internal void CopyLayoutInformationFrom(ElementView source)
        {
            if (source != null)
            {
                X = source.X;
                Y = source.Y;
            }
        }
    }
}