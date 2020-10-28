using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Structurizr
{
    /// <summary>
    ///     A software system.
    /// </summary>
    [DataContract]
    public sealed class SoftwareSystem : StaticStructureElement, IEquatable<SoftwareSystem>
    {
        private HashSet<Container> _containers;

        internal SoftwareSystem()
        {
            _containers = new HashSet<Container>();
        }

        /// <summary>
        ///     The location of this software system.
        /// </summary>
        [DataMember(Name = "location", EmitDefaultValue = true)]
        public Location Location { get; set; }

        /// <summary>
        ///     The set of containers within this software system.
        /// </summary>
        [DataMember(Name = "containers", EmitDefaultValue = false)]
        public ISet<Container> Containers
        {
            get => new HashSet<Container>(_containers);

            internal set => _containers = new HashSet<Container>(value);
        }

        public override string CanonicalName => CanonicalNameSeparator + FormatForCanonicalName(Name);

        public override Element Parent
        {
            get => null;

            set { }
        }

        public bool Equals(SoftwareSystem softwareSystem)
        {
            return Equals(softwareSystem as Element);
        }

        /// <summary>
        ///     Adds a container with the specified name, description and technology
        ///     (unless one exists with the same name already).
        /// </summary>
        /// <param name="name">the name of the container (e.g. "Web Application")</param>
        /// <param name="description">a short description/list of responsibilities</param>
        /// <param name="technology">the technoogy choice (e.g. "Spring MVC", "Java EE", etc)</param>
        public Container AddContainer(string name, string description, string technology)
        {
            return Model.AddContainer(this, name, description, technology);
        }

        internal void Add(Container container)
        {
            _containers.Add(container);
        }

        /// <summary>
        ///     Gets the container with the specified name (or null if it doesn't exist).
        /// </summary>
        public Container GetContainerWithName(string name)
        {
            foreach (var container in _containers)
                if (container.Name == name)
                    return container;

            return null;
        }

        /// <summary>
        ///     Gets the container with the specified ID (or null if it doesn't exist).
        /// </summary>
        public Container GetContainerWithId(string id)
        {
            foreach (var container in _containers)
                if (container.Id == id)
                    return container;

            return null;
        }

        public override List<string> GetRequiredTags()
        {
            return new List<string>
            {
                Structurizr.Tags.Element,
                Structurizr.Tags.SoftwareSystem
            };
        }
    }
}