using System;
using System.Runtime.Serialization;
using Structurizr.Util;

namespace Structurizr
{
    [DataContract]
    public sealed class Branding
    {
        private string _logo;

        [DataMember(Name = "font", EmitDefaultValue = false)]
        public Font Font;

        [DataMember(Name = "logo", EmitDefaultValue = false)]
        public string Logo
        {
            get => _logo;
            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    if (Url.IsUrl(value) || value.StartsWith("data:image/"))
                        _logo = value.Trim();
                    else
                        throw new ArgumentException(value + " is not a valid URL.");
                }
            }
        }
    }
}