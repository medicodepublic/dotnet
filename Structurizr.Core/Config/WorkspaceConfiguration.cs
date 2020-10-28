using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Structurizr.Config
{
    [DataContract]
    public sealed class WorkspaceConfiguration
    {
        private HashSet<User> _users;

        [JsonConstructor]
        internal WorkspaceConfiguration()
        {
            _users = new HashSet<User>();
        }

        [DataMember(Name = "users", EmitDefaultValue = false)]
        public ISet<User> Users
        {
            get => new HashSet<User>(_users);

            internal set => _users = new HashSet<User>(value);
        }

        public void AddUser(string username, Role role)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("A username must be specified.");

            _users.Add(new User(username, role));
        }
    }
}