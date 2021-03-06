﻿using System.Runtime.Serialization;

namespace Structurizr.Core.View
{
    /// <summary>
    ///     Provides a way for the terminology on diagrams, etc to be modified (e.g. language translations).
    /// </summary>
    [DataContract]
    public sealed class Terminology
    {
        [DataMember(Name = "code", EmitDefaultValue = false)]
        public string Code;

        [DataMember(Name = "component", EmitDefaultValue = false)]
        public string Component;

        [DataMember(Name = "container", EmitDefaultValue = false)]
        public string Container;

        [DataMember(Name = "deploymentNode", EmitDefaultValue = false)]
        public string DeploymentNode;

        [DataMember(Name = "enterprise", EmitDefaultValue = false)]
        public string Enterprise;

        [DataMember(Name = "infrastructureNode", EmitDefaultValue = false)]
        public string InfrastructureNode;

        [DataMember(Name = "person", EmitDefaultValue = false)]
        public string Person;

        [DataMember(Name = "relationship", EmitDefaultValue = false)]
        public string Relationship;

        [DataMember(Name = "softwareSystem", EmitDefaultValue = false)]
        public string SoftwareSystem;
    }
}