﻿using System;
using Xunit;

namespace Structurizr.Core.Tests
{
    public class SystemLandscapeViewTests : AbstractTestBase
    {
        private readonly SystemLandscapeView view;

        public SystemLandscapeViewTests()
        {
            view = Workspace.Views.CreateSystemLandscapeView("context", "Description");
        }

        [Fact]
        public void Test_Construction()
        {
            Assert.Equal("System Landscape", view.Name);
            Assert.Equal(0, view.Elements.Count);
            Assert.Same(Model, view.Model);
        }

        [Fact]
        public void Test_GetName_WhenNoEnterpriseIsSpecified()
        {
            Assert.Equal("System Landscape", view.Name);
        }

        [Fact]
        public void Test_GetName_WhenAnEnterpriseIsSpecified()
        {
            Model.Enterprise = new Enterprise("Widgets Limited");
            Assert.Equal("System Landscape for Widgets Limited", view.Name);
        }

        [Fact]
        public void Test_GetName_WhenAnEmptyEnterpriseNameIsSpecified()
        {
            Assert.Throws<ArgumentException>(() =>
                Model.Enterprise = new Enterprise("")
            );
        }

        [Fact]
        public void Test_GetName_WhenANullEnterpriseNameIsSpecified()
        {
            Assert.Throws<ArgumentException>(() =>
                Model.Enterprise = new Enterprise(null)
            );
        }

        [Fact]
        public void Test_AddAllSoftwareSystems_DoesNothing_WhenThereAreNoOtherSoftwareSystems()
        {
            view.AddAllSoftwareSystems();
            Assert.Equal(0, view.Elements.Count);
        }

        [Fact]
        public void Test_AddAllSoftwareSystems_AddsAllSoftwareSystems_WhenThereAreSomeSoftwareSystemsInTheModel()
        {
            var softwareSystemA = Model.AddSoftwareSystem(Location.External, "System A", "Description");
            var softwareSystemB = Model.AddSoftwareSystem(Location.External, "System B", "Description");

            view.AddAllSoftwareSystems();

            Assert.Equal(2, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemB)));
        }

        [Fact]
        public void Test_AddAllPeople_DoesNothing_WhenThereAreNoPeople()
        {
            view.AddAllPeople();
            Assert.Equal(0, view.Elements.Count);
        }

        [Fact]
        public void Test_AddAllPeople_AddsAllPeople_WhenThereAreSomePeopleInTheModel()
        {
            var userA = Model.AddPerson("User A", "Description");
            var userB = Model.AddPerson("User B", "Description");

            view.AddAllPeople();

            Assert.Equal(2, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(userA)));
            Assert.True(view.Elements.Contains(new ElementView(userB)));
        }

        [Fact]
        public void Test_AddAllElements_DoesNothing_WhenThereAreNoSoftwareSystemsOrPeople()
        {
            view.AddAllElements();
            Assert.Equal(0, view.Elements.Count);
        }

        [Fact]
        public void
            Test_AddAllElements_AddsAllSoftwareSystemsAndPeople_WhenThereAreSomeSoftwareSystemsAndPeopleInTheModel()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            var person = Model.AddPerson("Person", "Description");

            view.AddAllElements();

            Assert.Equal(2, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(softwareSystem)));
            Assert.True(view.Elements.Contains(new ElementView(person)));
        }
    }
}