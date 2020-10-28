using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Structurizr
{
    /// <summary>
    ///     Represents a view on top of a view, which can be used to include or exclude specific elements.
    /// </summary>
    [DataContract]
    public sealed class FilteredView
    {
        private string _baseViewKey;

        [JsonConstructor]
        internal FilteredView()
        {
            Mode = FilterMode.Exclude;
            Tags = new HashSet<string>();
        }

        internal FilteredView(StaticView view, string key, string description, FilterMode mode,
            params string[] tags) : this()
        {
            View = view;
            Key = key;
            Description = description;
            Mode = mode;

            if (tags != null)
                foreach (var tag in tags)
                    Tags.Add(tag);
        }

        [DataMember(Name = "key", EmitDefaultValue = false)]
        public string Key { get; internal set; }

        public View View { get; internal set; }

        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; internal set; }

        [DataMember(Name = "mode", EmitDefaultValue = false)]
        public FilterMode Mode { get; internal set; }

        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public ISet<string> Tags { get; internal set; }

        [DataMember(Name = "baseViewKey", EmitDefaultValue = false)]
        public string BaseViewKey
        {
            get
            {
                if (View != null)
                    return View.Key;
                return _baseViewKey;
            }
            set => _baseViewKey = value;
        }
    }
}