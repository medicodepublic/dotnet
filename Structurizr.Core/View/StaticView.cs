﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Structurizr
{
    [DataContract]
    public abstract class StaticView : View
    {
        private IList<Animation> _animations = new List<Animation>();

        internal StaticView()
        {
        }

        internal StaticView(SoftwareSystem softwareSystem, string key, string description) : base(softwareSystem, key,
            description)
        {
        }

        [DataMember(Name = "animations", EmitDefaultValue = false)]
        public IList<Animation> Animations
        {
            get => new List<Animation>(_animations);

            internal set => _animations = new List<Animation>(value);
        }

        public abstract void AddAllElements();

        /// <summary>
        ///     Adds all software systems in the model to this view.
        /// </summary>
        public void AddAllSoftwareSystems()
        {
            foreach (var softwareSystem in Model.SoftwareSystems) Add(softwareSystem);
        }

        /// <summary>
        ///     Adds the given SoftwareSystem to this view.
        /// </summary>
        public virtual void Add(SoftwareSystem softwareSystem)
        {
            AddElement(softwareSystem, true);
        }

        /// <summary>
        ///     Removes the given SoftwareSystem from this view.
        /// </summary>
        /// <param name="softwareSystem"></param>
        public void Remove(SoftwareSystem softwareSystem)
        {
            RemoveElement(softwareSystem);
        }

        /// <summary>
        ///     Adds all people in the model to this view.
        /// </summary>
        public void AddAllPeople()
        {
            foreach (var person in Model.People) Add(person);
        }

        /// <summary>
        ///     Adds the given Person to this view.
        /// </summary>
        public void Add(Person person)
        {
            AddElement(person, true);
        }

        /// <summary>
        ///     Removes the given Person from this view.
        /// </summary>
        /// <param name="person"></param>
        public void Remove(Person person)
        {
            RemoveElement(person);
        }

        public abstract void AddNearestNeighbours(Element element);

        protected void AddNearestNeighbours(Element element, Type typeOfElement)
        {
            if (element == null) return;

            AddElement(element, true);

            var relationships = Model.Relationships;
            foreach (var relationship in relationships)
            {
                if (relationship.Source.Equals(element) && relationship.Destination.GetType() == typeOfElement)
                    AddElement(relationship.Destination, true);

                if (relationship.Destination.Equals(element) && relationship.Source.GetType() == typeOfElement)
                    AddElement(relationship.Source, true);
            }
        }

        public void AddAnimation(params Element[] elements)
        {
            if (elements == null || elements.Length == 0)
                throw new ArgumentException("One or more elements must be specified.");

            ISet<string> elementIdsInPreviousAnimationSteps = new HashSet<string>();
            ISet<Element> elementsInThisAnimationStep = new HashSet<Element>();
            ISet<Relationship> relationshipsInThisAnimationStep = new HashSet<Relationship>();

            foreach (var element in elements)
                if (IsElementInView(element))
                {
                    elementIdsInPreviousAnimationSteps.Add(element.Id);
                    elementsInThisAnimationStep.Add(element);
                }

            if (elementsInThisAnimationStep.Count == 0)
                throw new ArgumentException("None of the specified elements exist in this view.");

            foreach (var animation in Animations)
            foreach (var elementId in animation.Elements)
                elementIdsInPreviousAnimationSteps.Add(elementId);

            foreach (var relationshipView in Relationships)
                if (
                    elementsInThisAnimationStep.Contains(relationshipView.Relationship.Source) &&
                    elementIdsInPreviousAnimationSteps.Contains(relationshipView.Relationship.Destination.Id) ||
                    elementIdsInPreviousAnimationSteps.Contains(relationshipView.Relationship.Source.Id) &&
                    elementsInThisAnimationStep.Contains(relationshipView.Relationship.Destination)
                )
                    relationshipsInThisAnimationStep.Add(relationshipView.Relationship);

            _animations.Add(new Animation(Animations.Count + 1, elementsInThisAnimationStep,
                relationshipsInThisAnimationStep));
        }
    }
}