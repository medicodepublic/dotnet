﻿using Xunit;

namespace Structurizr.Core.Tests
{
    public class PersonTests
    {
        private readonly Model model;
        private readonly Person person;

        private readonly Workspace workspace;

        public PersonTests()
        {
            workspace = new Workspace("Name", "Description");
            model = workspace.Model;
            person = model.AddPerson("Name", "Description");
        }

        [Fact]
        public void Test_CanonicalName()
        {
            Assert.Equal("/Name", person.CanonicalName);
        }

        [Fact]
        public void Test_CanonicalName_WhenNameContainsASlashCharacter()
        {
            person.Name = "Name1/Name2";
            Assert.Equal("/Name1Name2", person.CanonicalName);
        }

        [Fact]
        public void Test_Parent_ReturnsNull()
        {
            Assert.Null(person.Parent);
        }

        [Fact]
        public void Test_RemoveTags_DoesNotRemoveRequiredTags()
        {
            Assert.True(person.Tags.Contains(Tags.Element));
            Assert.True(person.Tags.Contains(Tags.Person));

            person.RemoveTag(Tags.Person);
            person.RemoveTag(Tags.Element);

            Assert.True(person.Tags.Contains(Tags.Element));
            Assert.True(person.Tags.Contains(Tags.Person));
        }
    }
}