﻿using Xunit;

namespace Structurizr.Core.Tests
{
    public class ContainerTests
    {
        private readonly Container container;
        private readonly Model model;
        private readonly SoftwareSystem softwareSystem;

        private readonly Workspace workspace;

        public ContainerTests()
        {
            workspace = new Workspace("Name", "Description");
            model = workspace.Model;
            softwareSystem = model.AddSoftwareSystem("System", "Description");
            container = softwareSystem.AddContainer("Container", "Description", "Some technology");
        }

        [Fact]
        public void Test_CanonicalName()
        {
            Assert.Equal("/System/Container", container.CanonicalName);
        }

        [Fact]
        public void Test_CanonicalName_WhenNameContainsASlashCharacter()
        {
            container.Name = "Name1/Name2";
            Assert.Equal("/System/Name1Name2", container.CanonicalName);
        }

        [Fact]
        public void Test_Parent_ReturnsTheParentSoftwareSystem()
        {
            Assert.Equal(softwareSystem, container.Parent);
        }

        [Fact]
        public void Test_SoftwareSystem_ReturnsTheParentSoftwareSystem()
        {
            Assert.Equal(softwareSystem, container.SoftwareSystem);
        }

        [Fact]
        public void Test_RemoveTags_DoesNotRemoveRequiredTags()
        {
            Assert.True(container.Tags.Contains(Tags.Element));
            Assert.True(container.Tags.Contains(Tags.Container));

            container.RemoveTag(Tags.Container);
            container.RemoveTag(Tags.Element);

            Assert.True(container.Tags.Contains(Tags.Element));
            Assert.True(container.Tags.Contains(Tags.Container));
        }
    }
}