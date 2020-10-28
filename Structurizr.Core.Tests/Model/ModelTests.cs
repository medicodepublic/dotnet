using System;
using System.Linq;
using Xunit;

namespace Structurizr.Core.Tests
{
    public class ModelTests : AbstractTestBase
    {
        [Fact]
        public void Test_AddContainerInstance_ThrowsAnException_WhenANullContainerIsSpecified()
        {
            try
            {
                Model.AddContainerInstance(null, null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("A container must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_AddContainerInstance_AddsAContainerInstance_WhenAContainerIsSpecified()
        {
            var developmentDeploymentNode =
                Model.AddDeploymentNode("Development", "Deployment Node", "Description", "Technology");
            var softwareSystem1 = Model.AddSoftwareSystem("Software System 1", "Description");
            var container1 = softwareSystem1.AddContainer("Container 1", "Description", "Technology");

            var softwareSystem2 = Model.AddSoftwareSystem("Software System 2", "Description");
            var container2 = softwareSystem2.AddContainer("Container 2", "Description", "Technology");

            var softwareSystem3 = Model.AddSoftwareSystem("Software System 3", "Description");
            var container3 = softwareSystem3.AddContainer("Container 3", "Description", "Technology");

            container1.Uses(container2, "Uses 1", "Technology 1", InteractionStyle.Synchronous);
            container2.Uses(container3, "Uses 2", "Technology 2", InteractionStyle.Asynchronous);

            var containerInstance1 = Model.AddContainerInstance(developmentDeploymentNode, container1);
            var containerInstance2 = Model.AddContainerInstance(developmentDeploymentNode, container2);
            var containerInstance3 = Model.AddContainerInstance(developmentDeploymentNode, container3);

            // the following live container instances should not affect the relationships of the development container instances
            var liveDeploymentNode = Model.AddDeploymentNode("Live", "Deployment Node", "Description", "Technology");
            liveDeploymentNode.Add(container1);
            liveDeploymentNode.Add(container2);
            liveDeploymentNode.Add(container3);

            Assert.Same(container2, containerInstance2.Container);
            Assert.Equal(container2.Id, containerInstance2.ContainerId);
            Assert.Same(softwareSystem2, containerInstance2.Parent);
            Assert.Equal("/Software System 2/Container 2[1]", containerInstance2.CanonicalName);
            Assert.Equal("Container Instance", containerInstance2.Tags);
            Assert.Equal("Development", containerInstance2.Environment);

            Assert.Equal(1, containerInstance1.Relationships.Count);
            var relationship = containerInstance1.Relationships.First();
            Assert.Same(containerInstance1, relationship.Source);
            Assert.Same(containerInstance2, relationship.Destination);
            Assert.Equal("Uses 1", relationship.Description);
            Assert.Equal("Technology 1", relationship.Technology);
            Assert.Equal(InteractionStyle.Synchronous, relationship.InteractionStyle);
            Assert.Equal("", relationship.Tags);

            Assert.Equal(1, containerInstance2.Relationships.Count);
            relationship = containerInstance2.Relationships.First();
            Assert.Same(containerInstance2, relationship.Source);
            Assert.Same(containerInstance3, relationship.Destination);
            Assert.Equal("Uses 2", relationship.Description);
            Assert.Equal("Technology 2", relationship.Technology);
            Assert.Equal(InteractionStyle.Asynchronous, relationship.InteractionStyle);
            Assert.Equal("", relationship.Tags);
        }

        [Fact]
        public void
            Test_AddImplicitRelationships_SetsTheDescriptionOfThePropagatedRelationship_WhenThereIsOnlyOnePossibleDescription()
        {
            var user = Model.AddPerson("Person", "Description");
            var softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            var webApplication = softwareSystem.AddContainer("Web Application", "Description", "Technology");

            user.Uses(webApplication, "Uses", "");
            Model.AddImplicitRelationships();

            Assert.Equal(2, user.Relationships.Count);

            var relationship = user.Relationships.First(r => r.Destination == softwareSystem);
            Assert.Equal("Uses", relationship.Description);
        }

        [Fact]
        public void
            Test_AddImplicitRelationships_DoeNotSetTheDescriptionOfThePropagatedRelationship_WhenThereIsMoreThanOnePossibleDescription()
        {
            var user = Model.AddPerson("Person", "Description");
            var softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            var webApplication = softwareSystem.AddContainer("Web Application", "Description", "Technology");

            user.Uses(webApplication, "Does something", "");
            user.Uses(webApplication, "Does something else", "");
            Model.AddImplicitRelationships();

            Assert.Equal(3, user.Relationships.Count);

            var relationship = user.Relationships.First(r => r.Destination == softwareSystem);
            Assert.Equal("", relationship.Description);
        }

        [Fact]
        public void
            Test_AddImplicitRelationships_SetsTheTechnologyOfThePropagatedRelationship_WhenThereIsOnlyOnePossibleTechnology()
        {
            var user = Model.AddPerson("Person", "Description");
            var softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            var webApplication = softwareSystem.AddContainer("Web Application", "Description", "Technology");

            user.Uses(webApplication, "Uses", "HTTPS");
            Model.AddImplicitRelationships();

            Assert.Equal(2, user.Relationships.Count);

            var relationship = user.Relationships.First(r => r.Destination == softwareSystem);
            Assert.Equal("HTTPS", relationship.Technology);
        }

        [Fact]
        public void
            Test_AddImplicitRelationships_DoeNotSetTheTechnologyOfThePropagatedRelationship_WhenThereIsMoreThanOnePossibleTechnology()
        {
            var user = Model.AddPerson("Person", "Description");
            var softwareSystem = Model.AddSoftwareSystem("Software System", "Description");
            var webApplication = softwareSystem.AddContainer("Web Application", "Description", "Technology");

            user.Uses(webApplication, "Does something", "Some technology");
            user.Uses(webApplication, "Does something else", "Some other technology");
            Model.AddImplicitRelationships();

            Assert.Equal(3, user.Relationships.Count);

            var relationship = user.Relationships.First(r => r.Destination == softwareSystem);
            Assert.Equal("", relationship.Technology);
        }

        [Fact]
        public void Test_AddRelationship_DisallowsTheSameRelationshipToBeAddedMoreThanOnce()
        {
            var element1 = Model.AddSoftwareSystem("Element 1", "Description");
            var element2 = Model.AddSoftwareSystem("Element 2", "Description");
            var relationship1 = element1.Uses(element2, "Uses", "");
            var relationship2 = element1.Uses(element2, "Uses", "");
            Assert.True(element1.Has(relationship1));
            Assert.Null(relationship2);
            Assert.Equal(1, element1.Relationships.Count);
        }

        [Fact]
        public void Test_AddRelationship_AllowsMultipleRelationshipsBetweenElements()
        {
            var element1 = Model.AddSoftwareSystem("Element 1", "Description");
            var element2 = Model.AddSoftwareSystem("Element 2", "Description");
            var relationship1 = element1.Uses(element2, "Uses in some way", "");
            var relationship2 = element1.Uses(element2, "Uses in another way", "");
            Assert.True(element1.Has(relationship1));
            Assert.True(element1.Has(relationship2));
            Assert.Equal(2, element1.Relationships.Count);
        }

        [Fact]
        public void Test_AddRelationship_ThrowsAnException_WhenTheDestinationIsAChildOfTheSource()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "");
            var container = softwareSystem.AddContainer("Container", "", "");
            var component = container.AddComponent("Component", "", "");

            try
            {
                softwareSystem.Uses(container, "Uses");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("Relationships cannot be added between parents and children.", ae.Message);
                Assert.Equal(0, softwareSystem.Relationships.Count);
            }

            try
            {
                container.Uses(component, "Uses");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("Relationships cannot be added between parents and children.", ae.Message);
                Assert.Equal(0, softwareSystem.Relationships.Count);
            }

            try
            {
                softwareSystem.Uses(component, "Uses");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("Relationships cannot be added between parents and children.", ae.Message);
                Assert.Equal(0, softwareSystem.Relationships.Count);
            }
        }

        [Fact]
        public void Test_AddRelationship_ThrowsAnException_WhenTheSourceIsAChildOfTheDestination()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "");
            var container = softwareSystem.AddContainer("Container", "", "");
            var component = container.AddComponent("Component", "", "");

            try
            {
                container.Uses(softwareSystem, "Uses");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("Relationships cannot be added between parents and children.", ae.Message);
                Assert.Equal(0, softwareSystem.Relationships.Count);
            }

            try
            {
                component.Uses(container, "Uses");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("Relationships cannot be added between parents and children.", ae.Message);
                Assert.Equal(0, softwareSystem.Relationships.Count);
            }

            try
            {
                component.Uses(softwareSystem, "Uses");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("Relationships cannot be added between parents and children.", ae.Message);
                Assert.Equal(0, softwareSystem.Relationships.Count);
            }
        }

        [Fact]
        public void Test_ModifyRelationship_ThrowsAnException_WhenARelationshipIsNotSpecified()
        {
            try
            {
                Model.ModifyRelationship(null, "Uses", "Technology");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("A relationship must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_ModifyRelationship_ModifiesAnExistingRelationship_WhenThatRelationshipDoesNotAlreadyExist()
        {
            var element1 = Model.AddSoftwareSystem("Element 1", "Description");
            var element2 = Model.AddSoftwareSystem("Element 2", "Description");
            var relationship = element1.Uses(element2, "", "");

            Model.ModifyRelationship(relationship, "Uses", "Technology");
            Assert.Equal("Uses", relationship.Description);
            Assert.Equal("Technology", relationship.Technology);
        }

        [Fact]
        public void Test_ModifyRelationship_ThrowsAnException_WhenThatRelationshipDoesAlreadyExist()
        {
            var element1 = Model.AddSoftwareSystem("Element 1", "Description");
            var element2 = Model.AddSoftwareSystem("Element 2", "Description");
            var relationship = element1.Uses(element2, "Uses", "Technology");

            try
            {
                Model.ModifyRelationship(relationship, "Uses", "Technology");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal(
                    "This relationship exists already: {1 | Element 1 | Description} ---[Uses]---> {2 | Element 2 | Description}",
                    ae.Message);
            }
        }

        [Fact]
        public void Test_GetElementWithCanonicalName_ThrowsAnException_WhenANullCanonicalNameIsSpecified()
        {
            try
            {
                Model.GetElementWithCanonicalName(null);
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("A canonical name must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_ElementWithCanonicalName_ThrowsAnException_WhenAnEmptyCanonicalNameIsSpecified()
        {
            try
            {
                Model.GetElementWithCanonicalName(" ");
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("A canonical name must be specified.", ae.Message);
            }
        }

        [Fact]
        public void
            Test_GetElementWithCanonicalName_ReturnsNull_WhenAnElementWithTheSpecifiedCanonicalNameDoesNotExist()
        {
            Assert.Null(Model.GetElementWithCanonicalName("Software System"));
        }

        [Fact]
        public void Test_ElementWithCanonicalName_ReturnsTheElement_WhenAnElementWithTheSpecifiedCanonicalNameExists()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "Description");

            Assert.Same(Model.GetElementWithCanonicalName("/Software System"), softwareSystem);
            Assert.Same(Model.GetElementWithCanonicalName("Software System"), softwareSystem);

            var container = softwareSystem.AddContainer("Web Application", "Description", "Technology");
            Assert.Same(container, Model.GetElementWithCanonicalName("/Software System/Web Application"));
            Assert.Same(container, Model.GetElementWithCanonicalName("Software System/Web Application"));
        }

        [Fact]
        public void Test_AddDeploymentNode_ThrowsAnException_WhenAChildDeploymentNodeWithTheSameNameAlreadyExists()
        {
            var deploymentNode = Model.AddDeploymentNode("Amazon Web Services");
            deploymentNode.AddDeploymentNode("AWS Region");
            try
            {
                deploymentNode.AddDeploymentNode("AWS Region");
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("A deployment/infrastructure node named 'AWS Region' already exists.", iae.Message);
            }
        }

        [Fact]
        public void Test_AddDeploymentNode_ThrowsAnException_WhenAChildInfrastructureNodeWithTheSameNameAlreadyExists()
        {
            var deploymentNode = Model.AddDeploymentNode("Amazon Web Services");
            deploymentNode.AddInfrastructureNode("Node");
            try
            {
                deploymentNode.AddDeploymentNode("Node");
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("A deployment/infrastructure node named 'Node' already exists.", iae.Message);
            }
        }

        [Fact]
        public void Test_AddInfrastructureNode_ThrowsAnException_WhenAChildDeploymentNodeWithTheSameNameAlreadyExists()
        {
            var deploymentNode = Model.AddDeploymentNode("Amazon Web Services");
            deploymentNode.AddDeploymentNode("Node");
            try
            {
                deploymentNode.AddInfrastructureNode("Node");
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("A deployment/infrastructure node named 'Node' already exists.", iae.Message);
            }
        }

        [Fact]
        public void
            Test_AddInfrastructureNode_ThrowsAnException_WhenAChildInfrastructureNodeWithTheSameNameAlreadyExists()
        {
            var deploymentNode = Model.AddDeploymentNode("Amazon Web Services");
            deploymentNode.AddInfrastructureNode("Node");
            try
            {
                deploymentNode.AddInfrastructureNode("Node");
                throw new TestFailedException();
            }
            catch (ArgumentException iae)
            {
                Assert.Equal("A deployment/infrastructure node named 'Node' already exists.", iae.Message);
            }
        }
    }
}