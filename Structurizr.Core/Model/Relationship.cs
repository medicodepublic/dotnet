using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Structurizr
{
    /// <summary>
    ///     A relationship between two elements.
    /// </summary>
    [DataContract]
    public sealed class Relationship : ModelItem, IEquatable<Relationship>
    {
        private string _description;

        private string _destinationId;

        private string _sourceId;

        private string _url;

        internal Relationship()
        {
        }

        internal Relationship(Element source, Element destination, string description) :
            this(source, destination, description, null)
        {
        }

        internal Relationship(Element source, Element destination, string description, string technology) :
            this(source, destination, description, technology, InteractionStyle.Synchronous)
        {
        }

        internal Relationship(Element source, Element destination, string description, string technology,
            InteractionStyle interactionStyle) :
            this()
        {
            Source = source;
            Destination = destination;
            Description = description;
            Technology = technology;
            InteractionStyle = interactionStyle;

            if (interactionStyle == InteractionStyle.Synchronous)
                AddTags(Structurizr.Tags.Synchronous);
            else
                AddTags(Structurizr.Tags.Asynchronous);
        }

        /// <summary>
        ///     A short description of this relationship.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description
        {
            get => _description ?? "";

            internal set => _description = value;
        }

        /// <summary>
        ///     The ID of the source element.
        /// </summary>
        [DataMember(Name = "sourceId", EmitDefaultValue = false)]
        public string SourceId
        {
            get
            {
                if (Source != null)
                    return Source.Id;
                return _sourceId;
            }
            set => _sourceId = value;
        }

        public Element Source { get; set; }

        /// <summary>
        ///     The ID of the destination element.
        /// </summary>
        [DataMember(Name = "destinationId", EmitDefaultValue = false)]
        public string DestinationId
        {
            get
            {
                if (Destination != null)
                    return Destination.Id;
                return _destinationId;
            }
            set => _destinationId = value;
        }

        public Element Destination { get; set; }

        /// <summary>
        ///     The technology associated with this relationship (e.g. HTTPS, JDBC, etc).
        /// </summary>
        [DataMember(Name = "technology", EmitDefaultValue = false)]
        public string Technology { get; internal set; }

        [DataMember(Name = "linkedRelationshipId", EmitDefaultValue = false)]
        public string LinkedRelationshipId { get; internal set; }

        /// <summary>
        ///     The interaction style (synchronous or asynchronous).
        /// </summary>
        [DataMember(Name = "interactionStyle", EmitDefaultValue = false)]
        public InteractionStyle InteractionStyle { get; set; } = InteractionStyle.Synchronous;

        /// <summary>
        ///     The URL where more information about this relationship can be found.
        /// </summary>
        [DataMember(Name = "url", EmitDefaultValue = false)]
        public string Url
        {
            get => _url;

            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    if (Util.Url.IsUrl(value))
                        _url = value;
                    else
                        throw new ArgumentException(value + " is not a valid URL.");
                }
            }
        }

        public bool Equals(Relationship relationship)
        {
            if (relationship == null) return false;

            if (relationship == this) return true;

            if (!Description.Equals(relationship.Description)) return false;
            if (!Destination.Equals(relationship.Destination)) return false;
            if (!Source.Equals(relationship.Source)) return false;

            return true;
        }

        public override List<string> GetRequiredTags()
        {
            if (LinkedRelationshipId == null)
            {
                string[] tags =
                {
                    Structurizr.Tags.Relationship
                };
                return tags.ToList();
            }

            return new List<string>();
        }

        /// <summary>
        ///     Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Relationship);
        }

        public override int GetHashCode()
        {
            var result = SourceId.GetHashCode();
            result = 31 * result + DestinationId.GetHashCode();
            result = 31 * result + Description.GetHashCode();
            return result;
        }

        public override string ToString()
        {
            return Source + " ---[" + Description + "]---> " + Destination;
        }
    }
}