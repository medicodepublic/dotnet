using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Structurizr
{
    [DataContract]
    public abstract class ModelItem
    {
        private ISet<Perspective> _perspectives = new HashSet<Perspective>();

        private Dictionary<string, string> _properties = new Dictionary<string, string>();

        private readonly List<string> _tags = new List<string>();

        internal ModelItem()
        {
        }

        /// <summary>
        ///     The ID of this item in the model.
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public string Tags
        {
            get
            {
                var listOfTags = new List<string>(GetRequiredTags());
                listOfTags.AddRange(_tags);

                if (listOfTags.Count == 0) return "";

                var buf = new StringBuilder();
                foreach (var tag in listOfTags)
                {
                    buf.Append(tag);
                    buf.Append(",");
                }

                var tagsAsString = buf.ToString();
                return tagsAsString.Substring(0, tagsAsString.Length - 1);
            }

            set
            {
                _tags.Clear();

                if (value == null) return;

                _tags.AddRange(value.Split(','));
            }
        }

        /// <summary>
        ///     The collection of name-value property pairs associated with this element, as a Dictionary.
        /// </summary>
        [DataMember(Name = "properties", EmitDefaultValue = false)]
        public Dictionary<string, string> Properties
        {
            get => _properties;

            internal set
            {
                if (value != null)
                    _properties = value;
                else
                    _properties.Clear();
            }
        }

        /// <summary>
        ///     The set of perspectives associated with this element.
        /// </summary>
        [DataMember(Name = "perspectives", EmitDefaultValue = false)]
        public ISet<Perspective> Perspectives
        {
            get => new HashSet<Perspective>(_perspectives);

            internal set => _perspectives = new HashSet<Perspective>(value);
        }

        public IEnumerable<string> GetAllTags()
        {
            if (string.IsNullOrWhiteSpace(Tags))
                return Enumerable.Empty<string>();
            return Tags.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
        }

        public void AddTags(params string[] tags)
        {
            if (tags == null) return;

            foreach (var tag in tags)
                if (tag != null)
                {
                    var t = tag.Trim();
                    if (!_tags.Contains(t)) _tags.Add(t);
                }
        }

        public virtual void RemoveTag(string tag)
        {
            if (tag != null) _tags.Remove(tag.Trim());
        }

        public abstract List<string> GetRequiredTags();

        /// <summary>
        ///     Adds a name-value pair property to this element.
        /// </summary>
        /// <param name="name">the name of the property</param>
        /// <param name="value">the value of the property</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddProperty(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("A property name must be specified.");

            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("A property value must be specified.");

            Properties[name] = value;
        }

        /// <summary>
        ///     Adds a perspective to this model item.
        /// </summary>
        /// <param name="name">the name of the perspective (e.g. "Security", must be unique)</param>
        /// <param name="description"></param>
        /// <returns>a Perspective object</returns>
        public Perspective AddPerspective(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("A name must be specified.");

            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("A description must be specified.");

            if (Perspectives.Contains(new Perspective(name, "")))
                throw new ArgumentException("A perspective named \"" + name + "\" already exists.");

            var perspective = new Perspective(name, description);
            _perspectives.Add(perspective);

            return perspective;
        }
    }
}