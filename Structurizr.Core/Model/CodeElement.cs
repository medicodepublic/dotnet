using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Structurizr
{
    /// <summary>
    ///     Represents a code element, such as a class or interface,
    ///     that is part of the implementation of a component.
    /// </summary>
    [DataContract]
    public sealed class CodeElement : IEquatable<CodeElement>
    {
        /// <summary>
        ///     The name of the code element ... typically the simple class/interface name.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public readonly string Name;

        /// <summary>
        ///     The name of the code element ... typically the simple class/interface name.
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public readonly string Type;

        private string _url;

        [JsonConstructor]
        internal CodeElement()
        {
        }

        /// <summary>
        ///     Creates a CodeElement based upon the fully qualified name provided.
        /// </summary>
        /// <param name="fullyQualifiedTypeName">A fully qualified type name</param>
        public CodeElement(string fullyQualifiedTypeName)
        {
            if (fullyQualifiedTypeName == null || fullyQualifiedTypeName.Trim().Length == 0)
                throw new ArgumentException("A fully qualified name must be provided.");

            var typeName = fullyQualifiedTypeName.Substring(0, fullyQualifiedTypeName.IndexOf(","));
            var dot = typeName.LastIndexOf('.');
            if (dot > -1)
            {
                Name = typeName.Substring(dot + 1);
                Type = fullyQualifiedTypeName;
            }
            else
            {
                Name = typeName;
                Type = fullyQualifiedTypeName;
            }

            Language = "C#";
        }

        /// <summary>
        ///     The role of the code element ... Primary or Supporting.
        /// </summary>
        [DataMember(Name = "role", EmitDefaultValue = true)]
        public CodeElementRole Role { get; internal set; }

        /// <summary>
        ///     The fully qualified type of the code element.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        ///     A URL; e.g. a reference to the code element in source code control.
        /// </summary>
        [DataMember(Name = "url", EmitDefaultValue = false)]
        public string Url
        {
            get => _url;

            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    Uri uri;
                    var result = Uri.TryCreate(value, UriKind.Absolute, out uri);
                    if (result)
                        _url = value;
                    else
                        throw new ArgumentException(value + " is not a valid URL.");
                }
            }
        }

        /// <summary>
        ///     The programming language used to create the code element.
        /// </summary>
        [DataMember(Name = "language", EmitDefaultValue = false)]
        public string Language { get; set; }

        /// <summary>
        ///     The category of code element; e.g. class, interface, etc.
        /// </summary>
        [DataMember(Name = "category", EmitDefaultValue = false)]
        public string Category { get; set; }

        /// <summary>
        ///     The visibility of the code element; e.g. public, package, private.
        /// </summary>
        [DataMember(Name = "visibility", EmitDefaultValue = false)]
        public string Visibility { get; set; }

        /// <summary>
        ///     The size of the code element; e.g. the number of lines.
        /// </summary>
        [DataMember(Name = "size", EmitDefaultValue = true)]
        public long Size { get; set; }

        public bool Equals(CodeElement other)
        {
            return other != null && other.Type.Equals(Type);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CodeElement);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
    }
}