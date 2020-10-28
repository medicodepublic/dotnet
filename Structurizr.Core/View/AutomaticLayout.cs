using System;
using System.Runtime.Serialization;

namespace Structurizr
{
    [DataContract]
    public class AutomaticLayout
    {
        private int _edgeSeparation;

        private int _nodeSeparation;

        private int _rankSeparation;

        /// <summary>
        ///     The rank direction.
        /// </summary>
        [DataMember(Name = "rankDirection", EmitDefaultValue = false)]
        public RankDirection RankDirection;

        /// <summary>
        ///     Whether the automatic layout algorithm should create vertices.
        /// </summary>
        [DataMember(Name = "vertices", EmitDefaultValue = true)]
        public bool Vertices;

        internal AutomaticLayout()
        {
        }

        internal AutomaticLayout(RankDirection rankDirection, int rankSeparation, int nodeSeparation,
            int edgeSeparation, bool vertices)
        {
            RankDirection = rankDirection;
            RankSeparation = rankSeparation;
            NodeSeparation = nodeSeparation;
            EdgeSeparation = edgeSeparation;
            Vertices = vertices;
        }

        /// <summary>
        ///     The rank separation (in pixels).
        /// </summary>
        [DataMember(Name = "rankSeparation", EmitDefaultValue = false)]
        public int RankSeparation
        {
            get => _rankSeparation;

            set
            {
                if (value < 0) throw new ArgumentException("The rank separation must be a positive integer.");

                _rankSeparation = value;
            }
        }

        [DataMember(Name = "nodeSeparation", EmitDefaultValue = false)]
        public int NodeSeparation
        {
            get => _nodeSeparation;

            set
            {
                if (value < 0) throw new ArgumentException("The node separation must be a positive integer.");

                _nodeSeparation = value;
            }
        }

        [DataMember(Name = "edgeSeparation", EmitDefaultValue = false)]
        public int EdgeSeparation
        {
            get => _edgeSeparation;

            set
            {
                if (value < 0) throw new ArgumentException("The edge separation must be a positive integer.");

                _edgeSeparation = value;
            }
        }
    }
}