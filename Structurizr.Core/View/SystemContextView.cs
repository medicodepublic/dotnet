using System.Runtime.Serialization;

namespace Structurizr
{
    /// <summary>
    ///     A system context view.
    /// </summary>
    [DataContract]
    public sealed class SystemContextView : StaticView
    {
        internal SystemContextView()
        {
        }

        internal SystemContextView(SoftwareSystem softwareSystem, string key, string description) : base(softwareSystem,
            key, description)
        {
            AddElement(softwareSystem, true);
        }

        public override string Name => SoftwareSystem.Name + " - System Context";

        /// <summary>
        ///     Determines whether the enterprise boundary (to differentiate "internal" elements from "external" elements") should
        ///     be visible on the resulting diagram.
        /// </summary>
        [DataMember(Name = "enterpriseBoundaryVisible", EmitDefaultValue = false)]
        public bool? EnterpriseBoundaryVisible { get; set; }

        /// <summary>
        ///     Adds all software systems and all people to this view.
        /// </summary>
        public override void AddAllElements()
        {
            AddAllSoftwareSystems();
            AddAllPeople();
        }

        /// <summary>
        ///     Adds people and software systems that are directly related to the given element.
        /// </summary>
        public override void AddNearestNeighbours(Element element)
        {
            AddNearestNeighbours(element, typeof(SoftwareSystem));
            AddNearestNeighbours(element, typeof(Person));
        }
    }
}