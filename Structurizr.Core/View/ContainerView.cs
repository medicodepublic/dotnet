using System.Runtime.Serialization;

namespace Structurizr
{
    /// <summary>
    ///     A container view.
    /// </summary>
    [DataContract]
    public sealed class ContainerView : StaticView
    {
        internal ContainerView()
        {
        }

        internal ContainerView(SoftwareSystem softwareSystem, string key, string description) : base(softwareSystem,
            key, description)
        {
        }

        public override string Name => SoftwareSystem.Name + " - Containers";

        /// <summary>
        ///     Determines whether software system boundaries should be visible for "external" containers (those outside the
        ///     software system in scope).
        /// </summary>
        [DataMember(Name = "externalSoftwareSystemBoundariesVisible", EmitDefaultValue = false)]
        public bool? ExternalSoftwareSystemBoundariesVisible { get; set; }


        /// <summary>
        ///     Adds all software systems, people and containers to this view.
        /// </summary>
        public override void AddAllElements()
        {
            AddAllSoftwareSystems();
            AddAllPeople();
            AddAllContainers();
        }

        public override void Add(SoftwareSystem softwareSystem)
        {
            if (softwareSystem != null && !softwareSystem.Equals(SoftwareSystem)) AddElement(softwareSystem, true);
        }

        public void AddAllContainers()
        {
            foreach (var container in SoftwareSystem.Containers) Add(container);
        }

        public void Add(Container container)
        {
            AddElement(container, true);
        }

        public void Remove(Container container)
        {
            RemoveElement(container);
        }

        /// <summary>
        ///     Adds people, software systems and containers that are directly related to the given element.
        /// </summary>
        public override void AddNearestNeighbours(Element element)
        {
            AddNearestNeighbours(element, typeof(Person));
            AddNearestNeighbours(element, typeof(SoftwareSystem));
            AddNearestNeighbours(element, typeof(Container));
        }
    }
}