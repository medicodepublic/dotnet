using System;
using System.Security.Cryptography;
using System.Text;

namespace Structurizr.Api
{
    internal class HashBasedMessageAuthenticationCode
    {
        private readonly string apiSecret;

        internal HashBasedMessageAuthenticationCode(string apiSecret)
        {
            this.apiSecret = apiSecret;
        }

        public string Generate(string content)
        {
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret));
            var bytes = Encoding.UTF8.GetBytes(content);
            var hash = hmac.ComputeHash(bytes);

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}