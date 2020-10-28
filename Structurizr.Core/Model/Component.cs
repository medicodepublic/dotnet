using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Structurizr
{
    /// <summary>
    ///     A component (a grouping of related functionality behind an interface that runs inside a container).
    /// </summary>
    [DataContract]
    public sealed class Component : StaticStructureElement, IEquatable<Component>
    {
        private HashSet<CodeElement> _codeElements;

        internal Component()
        {
            _codeElements = new HashSet<CodeElement>();
        }

        public override Element Parent { get; set; }

        public Container Container => Parent as Container;

        /// <summary>
        ///     The technology associated with this component (e.g. Spring Bean).
        /// </summary>
        [DataMember(Name = "technology", EmitDefaultValue = false)]
        public string Technology { get; set; }

        /// <summary>
        ///     The size of this component (e.g. lines of code).
        /// </summary>
        [DataMember(Name = "size", EmitDefaultValue = true)]
        public long Size { get; set; }

        /// <summary>
        ///     The implementation type (e.g. a fully qualified interface/class name).
        /// </summary>
        [DataMember(Name = "code", EmitDefaultValue = false)]
        public ISet<CodeElement> CodeElements
        {
            get => new HashSet<CodeElement>(_codeElements);

            internal set => _codeElements = new HashSet<CodeElement>(value);
        }

        public override string CanonicalName =>
            Parent.CanonicalName + CanonicalNameSeparator + FormatForCanonicalName(Name);

        /// <summary>
        ///     Gets the type of this component (e.g. a fully qualified interface/class name).
        /// </summary>
        public string Type
        {
            get
            {
                var codeElement = _codeElements.FirstOrDefault(ce => ce.Role == CodeElementRole.Primary);
                return codeElement?.Type;
            }

            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    _codeElements.RemoveWhere(ce => ce.Role == CodeElementRole.Primary);
                    var codeElement = new CodeElement(value);
                    codeElement.Role = CodeElementRole.Primary;
                    _codeElements.Add(codeElement);
                }
            }
        }

        public bool Equals(Component component)
        {
            return Equals(component as Element);
        }

        public override List<string> GetRequiredTags()
        {
            return new List<string>
            {
                Structurizr.Tags.Element,
                Structurizr.Tags.Component
            };
        }

        public CodeElement AddSupportingType(string type)
        {
            var codeElement = new CodeElement(type);
            codeElement.Role = CodeElementRole.Supporting;
            _codeElements.Add(codeElement);

            return codeElement;
        }
    }
}