using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Structurizr
{
    /// <summary>
    ///     A dynamic view, used to describe behaviour between static elements at runtime.
    /// </summary>
    [DataContract]
    public sealed class DynamicView : View
    {
        private readonly SequenceNumber _sequenceNumber = new SequenceNumber();

        private string _elementId;

        internal DynamicView()
        {
        }

        internal DynamicView(Model model, string key, string description) : base(null, key, description)
        {
            Model = model;
            Element = null;
        }

        internal DynamicView(SoftwareSystem softwareSystem, string key, string description) : base(softwareSystem, key,
            description)
        {
            Model = softwareSystem.Model;
            Element = softwareSystem;
        }

        internal DynamicView(Container container, string key, string description) : base(container.SoftwareSystem, key,
            description)
        {
            Model = container.Model;
            Element = container;
        }

        public override Model Model { get; set; }

        public override ISet<RelationshipView> Relationships
        {
            get
            {
                var list = new List<RelationshipView>(base.Relationships);
                var ordersAreNumeric = true;

                foreach (var relationshipView in list)
                    ordersAreNumeric = ordersAreNumeric && isNumeric(relationshipView.Order);

                if (ordersAreNumeric)
                    list.Sort(CompareAsNumber);
                else
                    list.Sort(CompareAsString);

                return new HashSet<RelationshipView>(list);
            }
        }

        public override string Name
        {
            get
            {
                if (Element != null)
                    return Element.Name + " - Dynamic";
                return "Dynamic";
            }
        }

        public Element Element { get; set; }

        /// <summary>
        ///     The ID of the container this view is associated with.
        /// </summary>
        [DataMember(Name = "elementId", EmitDefaultValue = false)]
        public string ElementId
        {
            get => Element != null ? Element.Id : _elementId;
            set => _elementId = value;
        }

        private bool isNumeric(string str)
        {
            try
            {
                double.Parse(str);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private int CompareAsNumber(RelationshipView x, RelationshipView y)
        {
            return double.Parse(x.Order).CompareTo(double.Parse(y.Order));
        }

        private int CompareAsString(RelationshipView x, RelationshipView y)
        {
            return x.Order.CompareTo(y.Order);
        }

        public RelationshipView Add(Element source, Element destination)
        {
            return Add(source, "", destination);
        }

        public RelationshipView Add(Element source, string description, Element destination)
        {
            if (source != null && destination != null)
            {
                CheckElement(source);
                CheckElement(destination);

                // check that the relationship is in the model before adding it
                var relationship = source.GetEfferentRelationshipWith(destination);
                if (relationship != null)
                {
                    AddElement(source, false);
                    AddElement(destination, false);
                    var relationshipView = AddRelationship(relationship, description, _sequenceNumber.GetNext());
                    return relationshipView;
                }

                throw new ArgumentException("Relationship does not exist in model");
            }

            throw new ArgumentException("Source and destination must not be null");
        }

        private void CheckElement(Element e)
        {
            // people can always be added
            if (e is Person) return;

            // if the scope of this dynamic is a software system, we only want:
            // - containers inside that software system
            // - other software systems
            if (Element is SoftwareSystem)
            {
                if (e.Equals(Element))
                    throw new ArgumentException(
                        e.Name + " is already the scope of this view and cannot be added to it.");
                if (e is Container && !e.Parent.Equals(Element))
                    throw new ArgumentException("Only containers that reside inside " + Element.Name +
                                                " can be added to this view.");
                if (e is Component)
                    throw new ArgumentException(
                        "Components can't be added to a dynamic view when the scope is a software system.");
            }

            // if the scope of this dynamic view is a container, we only want other containers inside the same software system
            // and other components inside the container
            if (Element is Container)
            {
                if (e.Equals(Element) || e.Equals(Element.Parent))
                    throw new ArgumentException(
                        e.Name + " is already the scope of this view and cannot be added to it.");
                if (e is Container && !e.Parent.Equals(Element.Parent))
                    throw new ArgumentException("Only containers that reside inside " + Element.Parent.Name +
                                                " can be added to this view.");

                if (e is Component && !e.Parent.Equals(Element))
                    throw new ArgumentException("Only components that reside inside " + Element.Name +
                                                " can be added to this view.");
            }
        }

        public override RelationshipView Add(Relationship relationship)
        {
            // when adding a relationship to a DynamicView we suppose the user really wants to also see both elements
            AddElement(relationship.Source, false);
            AddElement(relationship.Destination, false);

            return base.Add(relationship);
        }

        internal override RelationshipView FindRelationshipView(RelationshipView sourceRelationshipView)
        {
            foreach (var relationshipView in Relationships)
                if (relationshipView.Relationship.Equals(sourceRelationshipView.Relationship))
                    if (relationshipView.Description != null &&
                        relationshipView.Description.Equals(sourceRelationshipView.Description) &&
                        relationshipView.Order.Equals(sourceRelationshipView.Order))
                        return relationshipView;

            return null;
        }

        public void StartParallelSequence()
        {
            _sequenceNumber.StartParallelSequence();
        }

        public void EndParallelSequence()
        {
            EndParallelSequence(false);
        }

        public void EndParallelSequence(bool endAllParallelSequencesAndContinueNumbering)
        {
            _sequenceNumber.EndParallelSequence(endAllParallelSequencesAndContinueNumbering);
        }
    }
}