using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Structurizr
{
    /// <summary>
    ///     Represents a deployment instance of a Container, which can be added to a DeploymentNode.
    /// </summary>
    [DataContract]
    public sealed class ContainerInstance : DeploymentElement
    {
        private const int DefaultHealthCheckIntervalInSeconds = 60;
        private const long DefaultHealthCheckTimeoutInMilliseconds = 0;

        private string _containerId;

        private HashSet<HttpHealthCheck> _healthChecks = new HashSet<HttpHealthCheck>();

        internal ContainerInstance()
        {
        }

        internal ContainerInstance(Container container, int instanceId, string environment)
        {
            Container = container;
            AddTags(Structurizr.Tags.ContainerInstance);
            InstanceId = instanceId;
            Environment = environment;
        }

        public Container Container { get; internal set; }

        [DataMember(Name = "containerId", EmitDefaultValue = false)]
        public string ContainerId
        {
            get
            {
                if (Container != null)
                    return Container.Id;
                return _containerId;
            }
            set => _containerId = value;
        }

        [DataMember(Name = "instanceId", EmitDefaultValue = false)]
        public int InstanceId { get; internal set; }

        public override string Name
        {
            get => null;
            internal set
            {
                // no-op
            }
        }

        /// <summary>
        ///     The set of health checks associated with this container instance.
        /// </summary>
        [DataMember(Name = "healthChecks", EmitDefaultValue = false)]
        public ISet<HttpHealthCheck> HealthChecks
        {
            get => new HashSet<HttpHealthCheck>(_healthChecks);

            internal set => _healthChecks = new HashSet<HttpHealthCheck>(value);
        }

        public override string CanonicalName => Container.CanonicalName + "[" + InstanceId + "]";

        public override Element Parent
        {
            get => Container.Parent;
            set { }
        }

        public override List<string> GetRequiredTags()
        {
            return new List<string>();
        }

        public override void RemoveTag(string tag)
        {
            // do nothing ... tags cannot be removed from container instances (they should reflect the container they are based upon)
        }

        /// <summary>
        ///     Adds a new health check, with the default interval (60 seconds) and timeout (0 milliseconds).
        /// </summary>
        /// <param name="name">The name of the health check.</param>
        /// <param name="url">The URL of the health check.</param>
        /// <returns>A HttpHealthCheck instance.</returns>
        public HttpHealthCheck AddHealthCheck(string name, string url)
        {
            return AddHealthCheck(name, url, DefaultHealthCheckIntervalInSeconds,
                DefaultHealthCheckTimeoutInMilliseconds);
        }

        /// <summary>
        ///     Adds a new health check.
        /// </summary>
        /// <param name="name">The name of the health check.</param>
        /// <param name="url">The URL of the health check.</param>
        /// <param name="interval">The polling interval, in seconds.</param>
        /// <param name="timeout">The timeout, in milliseconds.</param>
        /// <returns>A HttpHealthCheck instance.</returns>
        public HttpHealthCheck AddHealthCheck(string name, string url, int interval, long timeout)
        {
            if (name == null || name.Trim().Length == 0)
                throw new ArgumentException("The name must not be null or empty.");

            if (url == null || url.Trim().Length == 0)
                throw new ArgumentException("The URL must not be null or empty.");

            if (!Util.Url.IsUrl(url)) throw new ArgumentException(url + " is not a valid URL.");

            if (interval < 0) throw new ArgumentException("The polling interval must be zero or a positive integer.");

            if (timeout < 0) throw new ArgumentException("The timeout must be zero or a positive integer.");

            var healthCheck = new HttpHealthCheck(name, url, interval, timeout);
            _healthChecks.Add(healthCheck);

            return healthCheck;
        }
    }
}