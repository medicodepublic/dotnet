using Xunit;

namespace Structurizr.Core.Tests
{
    public class SystemContextViewTests : AbstractTestBase
    {
        private readonly SoftwareSystem softwareSystem;
        private SystemContextView view;

        public SystemContextViewTests()
        {
            softwareSystem = Model.AddSoftwareSystem("The System", "Description");
            view = Workspace.Views.CreateSystemContextView(softwareSystem, "context", "Description");
        }

        [Fact]
        public void Test_Construction()
        {
            Assert.Equal("The System - System Context", view.Name);
            Assert.Equal(1, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(softwareSystem)));
            Assert.Same(softwareSystem, view.SoftwareSystem);
            Assert.Equal(softwareSystem.Id, view.SoftwareSystemId);
            Assert.Same(Model, view.Model);
        }

        [Fact]
        public void test_AddAllSoftwareSystems_DoesNothing_WhenThereAreNoOtherSoftwareSystems()
        {
            Assert.Equal(1, view.Elements.Count);
            view.AddAllSoftwareSystems();
            Assert.Equal(1, view.Elements.Count);
        }

        [Fact]
        public void test_AddAllSoftwareSystems_AddsAllSoftwareSystems_WhenThereAreSomeSoftwareSystemsInTheModel()
        {
            var softwareSystemA = Model.AddSoftwareSystem(Location.External, "System A", "Description");
            var softwareSystemB = Model.AddSoftwareSystem(Location.External, "System B", "Description");

            view.AddAllSoftwareSystems();

            Assert.Equal(3, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(softwareSystem)));
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemB)));
        }

        [Fact]
        public void test_AddAllPeople_DoesNothing_WhenThereAreNoPeople()
        {
            Assert.Equal(1, view.Elements.Count);
            view.AddAllPeople();
            Assert.Equal(1, view.Elements.Count);
        }

        [Fact]
        public void test_AddAllPeople_AddsAllPeople_WhenThereAreSomePeopleInTheModel()
        {
            var userA = Model.AddPerson(Location.External, "User A", "Description");
            var userB = Model.AddPerson(Location.External, "User B", "Description");

            view.AddAllPeople();

            Assert.Equal(3, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(softwareSystem)));
            Assert.True(view.Elements.Contains(new ElementView(userA)));
            Assert.True(view.Elements.Contains(new ElementView(userB)));
        }

        [Fact]
        public void test_AddAllElements_DoesNothing_WhenThereAreNoSoftwareSystemsOrPeople()
        {
            Assert.Equal(1, view.Elements.Count);
            view.AddAllElements();
            Assert.Equal(1, view.Elements.Count);
        }

        [Fact]
        public void
            test_AddAllElements_AddsAllSoftwareSystemsAndPeople_WhenThereAreSomeSoftwareSystemsAndPeopleInTheModel()
        {
            var softwareSystemA = Model.AddSoftwareSystem(Location.External, "System A", "Description");
            var softwareSystemB = Model.AddSoftwareSystem(Location.External, "System B", "Description");
            var userA = Model.AddPerson(Location.External, "User A", "Description");
            var userB = Model.AddPerson(Location.External, "User B", "Description");

            view.AddAllElements();

            Assert.Equal(5, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(softwareSystem)));
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemB)));
            Assert.True(view.Elements.Contains(new ElementView(userA)));
            Assert.True(view.Elements.Contains(new ElementView(userB)));
        }

        [Fact]
        public void Test_AddNearestNeightbours_DoesNothing_WhenANullElementIsSpecified()
        {
            view.AddNearestNeighbours(null);

            Assert.Equal(1, view.Elements.Count);
        }

        [Fact]
        public void Test_AddNearestNeighbours_DoesNothing_WhenThereAreNoNeighbours()
        {
            view.AddNearestNeighbours(softwareSystem);

            Assert.Equal(1, view.Elements.Count);
        }

        [Fact]
        public void Test_AddNearestNeighbours_AddsNearestNeighbours_WhenThereAreSomeNearestNeighbours()
        {
            var softwareSystemA = Model.AddSoftwareSystem("System A", "Description");
            var softwareSystemB = Model.AddSoftwareSystem("System B", "Description");
            var userA = Model.AddPerson("User A", "Description");
            var userB = Model.AddPerson("User B", "Description");

            // userA -> systemA -> system -> systemB -> userB
            userA.Uses(softwareSystemA, "");
            softwareSystemA.Uses(softwareSystem, "");
            softwareSystem.Uses(softwareSystemB, "");
            softwareSystemB.Delivers(userB, "");

            // userA -> systemA -> web application -> systemB -> userB
            // web application -> database
            var webApplication = softwareSystem.AddContainer("Web Application", "", "");
            var database = softwareSystem.AddContainer("Database", "", "");
            softwareSystemA.Uses(webApplication, "");
            webApplication.Uses(softwareSystemB, "");
            webApplication.Uses(database, "");

            // userA -> systemA -> controller -> service -> repository -> database
            var controller = webApplication.AddComponent("Controller", "");
            var service = webApplication.AddComponent("Service", "");
            var repository = webApplication.AddComponent("Repository", "");
            softwareSystemA.Uses(controller, "");
            controller.Uses(service, "");
            service.Uses(repository, "");
            repository.Uses(database, "");

            // userA -> systemA -> controller -> service -> systemB -> userB
            service.Uses(softwareSystemB, "");

            view.AddNearestNeighbours(softwareSystem);

            Assert.Equal(3, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.True(view.Elements.Contains(new ElementView(softwareSystem)));
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemB)));

            view = new SystemContextView(softwareSystem, "context", "Description");
            view.AddNearestNeighbours(softwareSystemA);

            Assert.Equal(3, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(userA)));
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.True(view.Elements.Contains(new ElementView(softwareSystem)));
        }
    }
}