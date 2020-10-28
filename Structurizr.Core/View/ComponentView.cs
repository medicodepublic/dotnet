using System.Runtime.Serialization;

namespace Structurizr
{
    /// <summary>
    ///     A system context view.
    /// </summary>
    [DataContract]
    public sealed class ComponentView : StaticView
    {
        private string containerId;

        internal ComponentView()
        {
        }

        internal ComponentView(Container container, string key, string description) : base(container.SoftwareSystem,
            key, description)
        {
            Container = container;
        }

        public override string Name => SoftwareSystem.Name + " - " + Container.Name + " - Components";

        public Container Container { get; set; }

        /// <summary>
        ///     The ID of the container this view is associated with.
        /// </summary>
        [DataMember(Name = "containerId", EmitDefaultValue = false)]
        public string ContainerId
        {
            get
            {
                if (Container != null)
                    return Container.Id;
                return containerId;
            }
            set => containerId = value;
        }

        /// <summary>
        ///     Determines whether container boundaries should be visible for "external" components (those outside the container in
        ///     scope).
        /// </summary>
        [DataMember(Name = "externalContainerBoundariesVisible", EmitDefaultValue = false)]
        public bool? ExternalContainerBoundariesVisible { get; set; }

        public override void AddAllElements()
        {
            AddAllSoftwareSystems();
            AddAllPeople();
            AddAllContainers();
            AddAllComponents();
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
            if (container != null && !container.Equals(Container)) AddElement(container, true);
        }

        public void Remove(Container container)
        {
            RemoveElement(container);
        }

        public void AddAllComponents()
        {
            foreach (var component in Container.Components) Add(component);
        }

        public void Add(Component component)
        {
            if (component != null) AddElement(component, true);
        }

        public void Remove(Component component)
        {
            RemoveElement(component);
        }

        /// <summary>
        ///     Adds people, software systems, containers and components that are directly related to the given element.
        /// </summary>
        public override void AddNearestNeighbours(Element element)
        {
            AddNearestNeighbours(element, typeof(Person));
            AddNearestNeighbours(element, typeof(SoftwareSystem));
            AddNearestNeighbours(element, typeof(Container));
            AddNearestNeighbours(element, typeof(Component));
        }
    }
}