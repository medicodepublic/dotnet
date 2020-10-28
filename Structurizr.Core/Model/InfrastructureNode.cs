using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Structurizr
{
    /// <summary>
    ///     Represents an infrastructure node, which is something like:
    ///     - Load balancer
    ///     - Firewall
    ///     - DNS service
    ///     - etc
    /// </summary>
    [DataContract]
    public sealed class InfrastructureNode : DeploymentElement
    {
        private DeploymentNode _parent;

        internal InfrastructureNode()
        {
        }

        /// <summary>
        ///     The parent DeploymentNode, or null if there is no parent.
        /// </summary>
        public override Element Parent
        {
            get => _parent;
            set => _parent = value as DeploymentNode;
        }

        [DataMember(Name = "technology", EmitDefaultValue = false)]
        public string Technology { get; set; }

        public override string CanonicalName =>
            _parent.CanonicalName + CanonicalNameSeparator + FormatForCanonicalName(Name);

        public override List<string> GetRequiredTags()
        {
            return new List<string>
            {
                Structurizr.Tags.Element,
                Structurizr.Tags.InfrastructureNode
            };
        }

        /// <summary>
        ///     Adds a relationship between this and another deployment element (deployment node, infrastructure node, or container
        ///     instance).
        /// </summary>
        /// <param name="destination">the destination DeploymentElement</param>
        /// <param name="description">a short description of the relationship</param>
        /// <param name="technology">the technology</param>
        /// <returns>a Relationship object</returns>
        public Relationship Uses(DeploymentElement destination, string description, string technology)
        {
            return Model.AddRelationship(this, destination, description, technology);
        }

        /// <summary>
        ///     Adds a relationship between this and another deployment element (deployment node, infrastructure node, or container
        ///     instance).
        /// </summary>
        /// <param name="destination">the destination DeploymentElement</param>
        /// <param name="description">a short description of the relationship</param>
        /// <param name="technology">the technology</param>
        /// <param name="interactionStyle">the interaction style (Synchronous vs Asynchronous)</param>
        /// <returns>a Relationship object</returns>
        public Relationship Uses(DeploymentElement destination, string description, string technology,
            InteractionStyle interactionStyle)
        {
            return Model.AddRelationship(this, destination, description, technology, interactionStyle);
        }
    }
}