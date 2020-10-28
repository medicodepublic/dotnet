using System;
using System.Runtime.Serialization;

namespace Structurizr
{
    [DataContract]
    public sealed class Font
    {
        private string _url;

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name;

        internal Font()
        {
        }

        public Font(string name)
        {
            Name = name;
        }

        public Font(string name, string url)
        {
            Name = name;
            Url = url;
        }

        /// <summary>
        ///     The URL where more information about this element can be found.
        /// </summary>
        [DataMember(Name = "url", EmitDefaultValue = false)]
        public string Url
        {
            get => _url;

            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    if (Util.Url.IsUrl(value))
                        _url = value;
                    else
                        throw new ArgumentException(value + " is not a valid URL.");
                }
            }
        }
    }
}