﻿using System;
using System.Runtime.Serialization;

namespace Structurizr
{
    /// <summary>
    ///     Represents an architectural perspective, that can be applied to elements and relationships.
    ///     See https://www.viewpoints-and-perspectives.info/home/perspectives/ for more details of this concept
    /// </summary>
    [DataContract]
    public sealed class Perspective : IEquatable<Perspective>
    {
        internal Perspective()
        {
        }

        internal Perspective(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        ///     The name of this perspective.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; internal set; }

        /// <summary>
        ///     The content of this perspective.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; internal set; }

        public bool Equals(Perspective other)
        {
            return other != null &&
                   Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Perspective);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}