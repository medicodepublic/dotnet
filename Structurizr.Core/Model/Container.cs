using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Structurizr
{
    /// <summary>
    ///     A container (something that can execute code or host data).
    /// </summary>
    [DataContract]
    public sealed class Container : StaticStructureElement, IEquatable<Container>
    {
        private HashSet<Component> _components;

        internal Container()
        {
            _components = new HashSet<Component>();
        }

        public override Element Parent { get; set; }

        public SoftwareSystem SoftwareSystem => Parent as SoftwareSystem;

        /// <summary>
        ///     The technology associated with this container (e.g. Windows Service).
        /// </summary>
        [DataMember(Name = "technology", EmitDefaultValue = false)]
        public string Technology { get; set; }

        /// <summary>
        ///     The set of components within this container.
        /// </summary>
        [DataMember(Name = "components", EmitDefaultValue = false)]
        public ISet<Component> Components
        {
            get => new HashSet<Component>(_components);

            set => _components = new HashSet<Component>(value);
        }

        public override string CanonicalName =>
            Parent.CanonicalName + CanonicalNameSeparator + FormatForCanonicalName(Name);

        public bool Equals(Container container)
        {
            return Equals(container as Element);
        }

        public Component AddComponent(string name, string description)
        {
            return AddComponent(name, description, null);
        }

        public Component AddComponent(string name, string description, string technology)
        {
            return AddComponent(name, (string) null, description, technology);
        }

        public Component AddComponent(string name, Type type, string description, string technology)
        {
            return AddComponent(name, type.AssemblyQualifiedName, description, technology);
        }

        public Component AddComponent(string name, string type, string description, string technology)
        {
            return Model.AddComponent(this, name, type, description, technology);
        }

        internal void Add(Component component)
        {
            if (GetComponentWithName(component.Name) == null) _components.Add(component);
        }

        public Component GetComponentWithName(string name)
        {
            if (name == null) return null;

            foreach (var component in Components)
                if (component.Name == name)
                    return component;

            return null;
        }

        public Component GetComponentOfType(string type)
        {
            if (type == null) return null;

            return _components.Where(c => c.Type == type).FirstOrDefault();
        }


        public override List<string> GetRequiredTags()
        {
            return new List<string>
            {
                Structurizr.Tags.Element,
                Structurizr.Tags.Container
            };
        }
    }
}