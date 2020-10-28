using System.IO;
using System.Runtime.Serialization;
using Structurizr.IO.Json;

namespace Structurizr.Encryption
{
    [DataContract]
    public class EncryptedWorkspace : AbstractWorkspace
    {
        private Workspace _workspace;

        public EncryptedWorkspace()
        {
        }

        public EncryptedWorkspace(Workspace workspace, EncryptionStrategy encryptionStrategy)
        {
            Workspace = workspace;
            EncryptionStrategy = encryptionStrategy;

            Configuration = workspace.Configuration;
            workspace.ClearConfiguration();

            var stringWriter = new StringWriter();
            var jsonWriter = new JsonWriter(false);
            jsonWriter.Write(workspace, stringWriter);

            Id = workspace.Id;
            Name = workspace.Name;
            Description = workspace.Description;
            Version = workspace.Version;
            Revision = workspace.Revision;
            LastModifiedAgent = workspace.LastModifiedAgent;
            LastModifiedUser = workspace.LastModifiedUser;
            Thumbnail = workspace.Thumbnail;

            Plaintext = stringWriter.ToString();
            Ciphertext = encryptionStrategy.Encrypt(Plaintext);
        }

        public Workspace Workspace
        {
            get
            {
                if (_workspace != null) return _workspace;

                if (Ciphertext != null)
                {
                    Plaintext = EncryptionStrategy.Decrypt(Ciphertext);
                    var stringReader = new StringReader(Plaintext);
                    return new JsonReader().Read(stringReader);
                }

                return null;
            }

            set => _workspace = value;
        }

        [DataMember(Name = "encryptionStrategy", EmitDefaultValue = false)]
        public EncryptionStrategy EncryptionStrategy { get; internal set; }

        internal string Plaintext { get; set; }

        [DataMember(Name = "ciphertext", EmitDefaultValue = false)]
        public string Ciphertext { get; internal set; }
    }
}