using System.Runtime.Serialization;

namespace Structurizr.Encryption
{
    [DataContract]
    public abstract class EncryptionStrategy
    {
        public EncryptionStrategy()
        {
        }

        public EncryptionStrategy(string passphrase)
        {
            Passphrase = passphrase;
        }

        [DataMember(Name = "type", EmitDefaultValue = false)]
        public abstract string Type { get; }

        public string Passphrase { get; set; }

        [DataMember(Name = "location", EmitDefaultValue = false)]
        public string Location => "Client";

        public abstract string Encrypt(string plaintext);
        public abstract string Decrypt(string ciphertext);
    }
}