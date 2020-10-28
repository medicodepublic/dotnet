using Xunit;

namespace Structurizr.Core.Tests
{
    public class ComponentViewTests : AbstractTestBase
    {
        private readonly SoftwareSystem softwareSystem;
        private ComponentView view;
        private readonly Container webApplication;

        public ComponentViewTests()
        {
            softwareSystem = Model.AddSoftwareSystem(Location.Internal, "The System", "Description");
            webApplication = softwareSystem.AddContainer("Web Application", "Does something", "Apache Tomcat");
            view = new ComponentView(webApplication, "Key", "Some description");
        }

        [Fact]
        public void Test_Sonstruction()
        {
            Assert.Equal("The System - Web Application - Components", view.Name);
            Assert.Equal("Some description", view.Description);
            Assert.Equal(0, view.Elements.Count);
            Assert.Same(softwareSystem, view.SoftwareSystem);
            Assert.Equal(softwareSystem.Id, view.SoftwareSystemId);
            Assert.Equal(webApplication.Id, view.ContainerId);
            Assert.Same(Model, view.Model);
        }

        [Fact]
        public void Test_AddAllSoftwareSystems_DoesNothing_WhenThereAreNoOtherSoftwareSystems()
        {
            Assert.Equal(0, view.Elements.Count);
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
            Assert.Equal(0, view.Elements.Count);
            view.AddAllPeople();
            Assert.Equal(0, view.Elements.Count);
        }

        [Fact]
        public void Test_AddAllPeople_AddsAllPeople_WhenThereAreSomePeopleInTheModel()
        {
            var userA = Model.AddPerson(Location.External, "User A", "Description");
            var userB = Model.AddPerson(Location.External, "User B", "Description");

            view.AddAllPeople();

            Assert.Equal(2, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(userA)));
            Assert.True(view.Elements.Contains(new ElementView(userB)));
        }

        [Fact]
        public void Test_AddAllElements_DoesNothing_WhenThereAreNoSoftwareSystemsOrPeople()
        {
            Assert.Equal(0, view.Elements.Count);
            view.AddAllElements();
            Assert.Equal(0, view.Elements.Count);
        }

        [Fact]
        public void
            Test_AddAllElements_AddsAllSoftwareSystemsAndPeopleAndContainersAndComponents_WhenThereAreSomeSoftwareSystemsAndPeopleAndContainersAndComponentsInTheModel()
        {
            var softwareSystemA = Model.AddSoftwareSystem(Location.External, "System A", "Description");
            var softwareSystemB = Model.AddSoftwareSystem(Location.External, "System B", "Description");
            var userA = Model.AddPerson(Location.External, "User A", "Description");
            var userB = Model.AddPerson(Location.External, "User B", "Description");
            var database = softwareSystem.AddContainer("Database", "Does something", "MySQL");
            var componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            var componentB = webApplication.AddComponent("Component B", "Does something", "Java");

            view.AddAllElements();

            Assert.Equal(7, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemB)));
            Assert.True(view.Elements.Contains(new ElementView(userA)));
            Assert.True(view.Elements.Contains(new ElementView(userB)));
            Assert.True(view.Elements.Contains(new ElementView(database)));
            Assert.True(view.Elements.Contains(new ElementView(componentA)));
            Assert.True(view.Elements.Contains(new ElementView(componentB)));
        }

        [Fact]
        public void Test_AddAllContainers_DoesNothing_WhenThereAreNoContainers()
        {
            Assert.Equal(0, view.Elements.Count);
            view.AddAllContainers();
            Assert.Equal(0, view.Elements.Count);
        }

        [Fact]
        public void Test_AddAllContainers_AddsAllContainers_WhenThereAreSomeContainers()
        {
            var database = softwareSystem.AddContainer("Database", "Stores something", "MySQL");
            var fileSystem = softwareSystem.AddContainer("File System", "Stores something else", "");

            view.AddAllContainers();

            Assert.Equal(2, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(database)));
            Assert.True(view.Elements.Contains(new ElementView(fileSystem)));
        }

        [Fact]
        public void Test_AddAllComponents_DoesNothing_WhenThereAreNoComponents()
        {
            Assert.Equal(0, view.Elements.Count);
            view.AddAllComponents();
            Assert.Equal(0, view.Elements.Count);
        }

        [Fact]
        public void Test_AddAllComponents_AddsAllComponents_WhenThereAreSomeComponents()
        {
            var componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            var componentB = webApplication.AddComponent("Component B", "Does something", "Java");

            view.AddAllComponents();

            Assert.Equal(2, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(componentA)));
            Assert.True(view.Elements.Contains(new ElementView(componentB)));
        }

        [Fact]
        public void Test_Add_DoesNothing_WhenANullContainerIsSpecified()
        {
            Assert.Equal(0, view.Elements.Count);
            view.Add((Container) null);
            Assert.Equal(0, view.Elements.Count);
        }

        [Fact]
        public void Test_Add_AddsTheContainer_WhenTheContainerIsNoInTheViewAlready()
        {
            var database = softwareSystem.AddContainer("Database", "Stores something", "MySQL");

            Assert.Equal(0, view.Elements.Count);
            view.Add(database);
            Assert.Equal(1, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(database)));
        }

        [Fact]
        public void Test_Add_DoesNothing_WhenTheSpecifiedContainerIsAlreadyInTheView()
        {
            var database = softwareSystem.AddContainer("Database", "Stores something", "MySQL");
            view.Add(database);
            Assert.Equal(1, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(database)));

            view.Add(database);
            Assert.Equal(1, view.Elements.Count);
        }

        [Fact]
        public void Test_Remove_DoesNothing_WhenANullContainerIsPassed()
        {
            Assert.Equal(0, view.Elements.Count);
            view.Remove((Container) null);
            Assert.Equal(0, view.Elements.Count);
        }

        [Fact]
        public void Test_Remove_RemovesTheContainer_WhenTheContainerIsInTheView()
        {
            var database = softwareSystem.AddContainer("Database", "Stores something", "MySQL");
            view.Add(database);
            Assert.Equal(1, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(database)));

            view.Remove(database);
            Assert.Equal(0, view.Elements.Count);
        }

        [Fact]
        public void Test_Remove_DoesNothing_WhenTheContainerIsNotInTheView()
        {
            var database = softwareSystem.AddContainer("Database", "Stores something", "MySQL");
            var fileSystem = softwareSystem.AddContainer("File System", "Stores something else", "");

            view.Add(database);
            Assert.Equal(1, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(database)));

            view.Remove(fileSystem);
            Assert.Equal(1, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(database)));
        }

        [Fact]
        public void Test_Add_DoesNothing_WhenANullComponentIsSpecified()
        {
            Assert.Equal(0, view.Elements.Count);
            view.Add((Component) null);
            Assert.Equal(0, view.Elements.Count);
        }

        [Fact]
        public void Test_Add_AddsTheComponent_WhenTheComponentIsNotInTheViewAlready()
        {
            var componentA = webApplication.AddComponent("Component A", "Does something", "Java");

            Assert.Equal(0, view.Elements.Count);
            view.Add(componentA);
            Assert.Equal(1, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(componentA)));
        }

        [Fact]
        public void Test_Add_DoesNothing_WhenTheSpecifiedComponentIsAlreadyInTheView()
        {
            var componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            view.Add(componentA);
            Assert.Equal(1, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(componentA)));

            view.Add(componentA);
            Assert.Equal(1, view.Elements.Count);
        }

        [Fact]
        public void Test_Add_DoesNothing_WhenTheContainerOfTheViewIsAdded()
        {
            view.Add(webApplication);
            Assert.False(view.Elements.Contains(new ElementView(webApplication)));
        }

        [Fact]
        public void Test_Remove_DoesNothing_WhenANullComponentIsPassed()
        {
            Assert.Equal(0, view.Elements.Count);
            view.Remove((Component) null);
            Assert.Equal(0, view.Elements.Count);
        }

        [Fact]
        public void Test_Remove_RemovesTheComponent_WhenTheComponentIsInTheView()
        {
            var componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            view.Add(componentA);
            Assert.Equal(1, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(componentA)));

            view.Remove(componentA);
            Assert.Equal(0, view.Elements.Count);
        }

        [Fact]
        public void
            Test_Remove_RemovesTheComponentAndRelationships_WhenTheComponentIsInTheViewAndHasArelationshipToAnotherElement()
        {
            var componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            var componentB = webApplication.AddComponent("Component B", "Does something", "Java");
            componentA.Uses(componentB, "uses");

            view.Add(componentA);
            view.Add(componentB);
            Assert.Equal(2, view.Elements.Count);
            Assert.Equal(1, view.Relationships.Count);

            view.Remove(componentB);
            Assert.Equal(1, view.Elements.Count);
            Assert.Equal(0, view.Relationships.Count);
        }

        [Fact]
        public void Test_Remove_DoesNothing_WhenTheComponentIsNotInTheView()
        {
            var componentA = webApplication.AddComponent("Component A", "Does something", "Java");
            var componentB = webApplication.AddComponent("Component B", "Does something", "Java");

            view.Add(componentA);
            Assert.Equal(1, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(componentA)));

            view.Remove(componentB);
            Assert.Equal(1, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(componentA)));
        }

        [Fact]
        public void Test_AddNearestNeightbours_DoesNothing_WhenANullElementIsSpecified()
        {
            view.AddNearestNeighbours(null);

            Assert.Equal(0, view.Elements.Count);
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

            view = new ComponentView(webApplication, "components", "Description");
            view.AddNearestNeighbours(softwareSystemA);

            Assert.Equal(5, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(userA)));
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.True(view.Elements.Contains(new ElementView(softwareSystem)));
            Assert.True(view.Elements.Contains(new ElementView(webApplication)));
            Assert.True(view.Elements.Contains(new ElementView(controller)));

            view = new ComponentView(webApplication, "components", "Description");
            view.AddNearestNeighbours(webApplication);

            Assert.Equal(4, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.True(view.Elements.Contains(new ElementView(webApplication)));
            Assert.True(view.Elements.Contains(new ElementView(database)));
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemB)));

            view = new ComponentView(webApplication, "components", "Description");
            view.AddNearestNeighbours(controller);

            Assert.Equal(3, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemA)));
            Assert.True(view.Elements.Contains(new ElementView(controller)));
            Assert.True(view.Elements.Contains(new ElementView(service)));

            view = new ComponentView(webApplication, "components", "Description");
            view.AddNearestNeighbours(service);

            Assert.Equal(4, view.Elements.Count);
            Assert.True(view.Elements.Contains(new ElementView(controller)));
            Assert.True(view.Elements.Contains(new ElementView(service)));
            Assert.True(view.Elements.Contains(new ElementView(repository)));
            Assert.True(view.Elements.Contains(new ElementView(softwareSystemB)));
        }
    }
}