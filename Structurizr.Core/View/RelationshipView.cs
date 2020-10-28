﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Structurizr
{
    /// <summary>
    ///     An instance of a model relationship in a View.
    /// </summary>
    [DataContract]
    public sealed class RelationshipView : ModelItemView, IEquatable<RelationshipView>
    {
        private int? _position;

        private string id;

        internal RelationshipView()
        {
        }

        internal RelationshipView(Relationship relationship)
        {
            Relationship = relationship;
        }

        public Relationship Relationship { get; set; }

        /// <summary>
        ///     The ID of the relationship.
        /// </summary>
        /// <value>The ID of the relationship.</value>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id
        {
            get
            {
                if (Relationship != null)
                    return Relationship.Id;
                return id;
            }
            set => id = value;
        }

        /// <summary>
        ///     The order of this relationship (used in dynamic views only; e.g. 1.0, 1.1, 2.0, etc).
        /// </summary>
        [DataMember(Name = "order", EmitDefaultValue = false)]
        public string Order { get; set; }

        /// <summary>
        ///     The description of this relationship (used in dynamic views only).
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        ///     The set of vertices used to render the relationship.
        /// </summary>
        [DataMember(Name = "vertices", EmitDefaultValue = false)]
        public List<Vertex> Vertices { get; set; }

        /// <summary>
        ///     The routing of the line.
        /// </summary>
        [DataMember(Name = "routing", EmitDefaultValue = false)]
        public Routing? Routing { get; set; }

        /// <summary>
        ///     The position of the annotation along the line; 0 (start) to 100 (end).
        /// </summary>
        [DataMember(Name = "position", EmitDefaultValue = false)]
        public int? Position
        {
            get => _position;
            set
            {
                if (value != null)
                {
                    if (value < 0)
                        _position = 0;
                    else if (value > 100)
                        _position = 100;
                    else
                        _position = value;
                }
            }
        }

        public bool Equals(RelationshipView relationshipView)
        {
            if (relationshipView == null) return false;
            if (relationshipView == this) return true;

            if (Description != null
                ? Description != relationshipView.Description
                : relationshipView.Description != null) return false;
            if (Id != relationshipView.Id) return false;
            return !(Order != null ? Order != relationshipView.Order : relationshipView.Order != null);
        }

        public override string ToString()
        {
            return Relationship.ToString();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RelationshipView);
        }

        public override int GetHashCode()
        {
            var result = Id.GetHashCode();
            result = 31 * result + (Description != null ? Description.GetHashCode() : 0);
            result = 31 * result + (Order != null ? Order.GetHashCode() : 0);
            return result;
        }

        internal void CopyLayoutInformationFrom(RelationshipView source)
        {
            if (source != null)
            {
                Vertices = source.Vertices;
                Routing = source.Routing;
                Position = source.Position;
            }
        }
    }
}