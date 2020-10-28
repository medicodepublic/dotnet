using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Structurizr
{
    /// <summary>
    ///     A person who uses a software system.
    /// </summary>
    [DataContract]
    public sealed class Person : StaticStructureElement, IEquatable<Person>
    {
        internal Person()
        {
        }

        /// <summary>
        ///     The location of this person.
        /// </summary>
        [DataMember(Name = "location", EmitDefaultValue = true)]
        public Location Location { get; set; }

        public override string CanonicalName => CanonicalNameSeparator + FormatForCanonicalName(Name);

        public override Element Parent
        {
            get => null;

            set { }
        }

        public bool Equals(Person person)
        {
            return Equals(person as Element);
        }

        public override List<string> GetRequiredTags()
        {
            return new List<string>
            {
                Structurizr.Tags.Element,
                Structurizr.Tags.Person
            };
        }

        public new Relationship Delivers(Person destination, string description)
        {
            throw new InvalidOperationException();
        }

        public new Relationship Delivers(Person destination, string description, string technology)
        {
            throw new InvalidOperationException();
        }

        public new Relationship Delivers(Person destination, string description, string technology,
            InteractionStyle interactionStyle)
        {
            throw new InvalidOperationException();
        }

        public Relationship InteractsWith(Person destination, string description)
        {
            return Model.AddRelationship(this, destination, description);
        }

        public Relationship InteractsWith(Person destination, string description, string technology)
        {
            return Model.AddRelationship(this, destination, description, technology);
        }

        public Relationship InteractsWith(Person destination, string description, string technology,
            InteractionStyle interactionStyle)
        {
            return Model.AddRelationship(this, destination, description, technology, interactionStyle);
        }
    }
}