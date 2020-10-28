using System;
using System.Linq;
using Xunit;

namespace Structurizr.Core.Tests
{
    public class DeploymentViewTests : AbstractTestBase
    {
        private DeploymentView deploymentView;

        [Fact]
        public void Test_Name_WithNoSoftwareSystemAndNoEnvironment()
        {
            deploymentView = Views.CreateDeploymentView("deployment", "Description");
            Assert.Equal("Deployment", deploymentView.Name);
        }

        [Fact]
        public void Test_Name_WithNoSoftwareSystemAndAnEnvironment()
        {
            deploymentView = Views.CreateDeploymentView("deployment", "Description");
            deploymentView.Environment = "Live";
            Assert.Equal("Deployment - Live", deploymentView.Name);
        }

        [Fact]
        public void Test_Name_WithASoftwareSystemAndNoEnvironment()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "");
            deploymentView = Views.CreateDeploymentView(softwareSystem, "deployment", "Description");
            Assert.Equal("Software System - Deployment", deploymentView.Name);
        }

        [Fact]
        public void Test_Name_WithASoftwareSystemAndAnEnvironment()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "");
            deploymentView = Views.CreateDeploymentView(softwareSystem, "deployment", "Description");
            deploymentView.Environment = "Live";
            Assert.Equal("Software System - Deployment - Live", deploymentView.Name);
        }

        [Fact]
        public void Test_AddDeploymentNode_ThrowsAnException_WhenPassedNull()
        {
            try
            {
                deploymentView = Views.CreateDeploymentView("key", "Description");
                deploymentView.Add(null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("A deployment node must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_AddRelationship_ThrowsAnException_WhenPassedNull()
        {
            try
            {
                deploymentView = Views.CreateDeploymentView("key", "Description");
                deploymentView.Add((Relationship) null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("A relationship must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_AddAllDeploymentNodes_DoesNothing_WhenThereAreNoTopLevelDeploymentNodes()
        {
            deploymentView = Views.CreateDeploymentView("deployment", "Description");

            deploymentView.AddAllDeploymentNodes();
            Assert.Equal(0, deploymentView.Elements.Count);
        }

        [Fact]
        public void Test_AddAllDeploymentNodes_DoesNothing_WhenThereAreTopLevelDeploymentNodesButNoContainerInstances()
        {
            deploymentView = Views.CreateDeploymentView("deployment", "Description");
            Model.AddDeploymentNode("Deployment Node", "Description", "Technology");

            deploymentView.AddAllDeploymentNodes();
            Assert.Equal(0, deploymentView.Elements.Count);
        }

        [Fact]
        public void Test_AddAllDeploymentNodes_DoesNothing_WhenThereNoDeploymentNodesForTheDeploymentEnvironment()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "");
            var container = softwareSystem.AddContainer("Container", "Description", "Technology");
            var deploymentNode = Model.AddDeploymentNode("Deployment Node", "Description", "Technology");
            var containerInstance = deploymentNode.Add(container);

            deploymentView = Views.CreateDeploymentView(softwareSystem, "deployment", "Description");
            deploymentView.Environment = "Live";
            deploymentView.AddAllDeploymentNodes();
            Assert.Equal(0, deploymentView.Elements.Count);
        }

        [Fact]
        public void
            Test_AddAllDeploymentNodes_AddsDeploymentNodesAndContainerInstances_WhenThereAreTopLevelDeploymentNodesWithContainerInstances()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "");
            var container = softwareSystem.AddContainer("Container", "Description", "Technology");
            var deploymentNode = Model.AddDeploymentNode("Deployment Node", "Description", "Technology");
            var containerInstance = deploymentNode.Add(container);

            deploymentView = Views.CreateDeploymentView(softwareSystem, "deployment", "Description");
            deploymentView.AddAllDeploymentNodes();
            Assert.Equal(2, deploymentView.Elements.Count);
            Assert.True(deploymentView.Elements.Contains(new ElementView(deploymentNode)));
            Assert.True(deploymentView.Elements.Contains(new ElementView(containerInstance)));
        }

        [Fact]
        public void
            Test_AddAllDeploymentNodes_AddsDeploymentNodesAndContainerInstances_WhenThereAreChildDeploymentNodesWithContainerInstances()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "");
            var container = softwareSystem.AddContainer("Container", "Description", "Technology");
            var deploymentNodeParent = Model.AddDeploymentNode("Deployment Node", "Description", "Technology");
            var deploymentNodeChild =
                deploymentNodeParent.AddDeploymentNode("Deployment Node", "Description", "Technology");
            var containerInstance = deploymentNodeChild.Add(container);

            deploymentView = Views.CreateDeploymentView(softwareSystem, "deployment", "Description");
            deploymentView.AddAllDeploymentNodes();
            Assert.Equal(3, deploymentView.Elements.Count);
            Assert.True(deploymentView.Elements.Contains(new ElementView(deploymentNodeParent)));
            Assert.True(deploymentView.Elements.Contains(new ElementView(deploymentNodeChild)));
            Assert.True(deploymentView.Elements.Contains(new ElementView(containerInstance)));
        }

        [Fact]
        public void Test_AddAllDeploymentNodes_AddsDeploymentNodesAndContainerInstancesOnlyForTheSoftwareSystemInScope()
        {
            var softwareSystem1 = Model.AddSoftwareSystem("Software System 1", "");
            var container1 = softwareSystem1.AddContainer("Container 1", "Description", "Technology");
            var deploymentNode1 = Model.AddDeploymentNode("Deployment Node 1", "Description", "Technology");
            var containerInstance1 = deploymentNode1.Add(container1);

            var softwareSystem2 = Model.AddSoftwareSystem("Software System 2", "");
            var container2 = softwareSystem2.AddContainer("Container 2", "Description", "Technology");
            var deploymentNode2 = Model.AddDeploymentNode("Deployment Node 2", "Description", "Technology");
            var containerInstance2 = deploymentNode2.Add(container2);

            // two containers from different software systems on the same deployment node
            deploymentNode1.Add(container2);

            deploymentView = Views.CreateDeploymentView(softwareSystem1, "deployment", "Description");
            deploymentView.AddAllDeploymentNodes();

            Assert.Equal(2, deploymentView.Elements.Count);
            Assert.True(deploymentView.Elements.Contains(new ElementView(deploymentNode1)));
            Assert.True(deploymentView.Elements.Contains(new ElementView(containerInstance1)));
        }

        [Fact]
        public void Test_AddDeploymentNode_AddsTheParentToo()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "");
            var container = softwareSystem.AddContainer("Container", "Description", "Technology");
            var deploymentNodeParent = Model.AddDeploymentNode("Deployment Node", "Description", "Technology");
            var deploymentNodeChild =
                deploymentNodeParent.AddDeploymentNode("Deployment Node", "Description", "Technology");
            var containerInstance = deploymentNodeChild.Add(container);

            deploymentView = Views.CreateDeploymentView(softwareSystem, "deployment", "Description");
            deploymentView.Add(deploymentNodeChild);
            Assert.Equal(3, deploymentView.Elements.Count);
            Assert.True(deploymentView.Elements.Contains(new ElementView(deploymentNodeParent)));
            Assert.True(deploymentView.Elements.Contains(new ElementView(deploymentNodeChild)));
            Assert.True(deploymentView.Elements.Contains(new ElementView(containerInstance)));
        }

        [Fact]
        public void Test_AddAnimationStep_ThrowsAnException_WhenNoContainerInstancesAreSpecified()
        {
            try
            {
                deploymentView = Views.CreateDeploymentView("deployment", "Description");
                deploymentView.AddAnimation((ContainerInstance[]) null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("One or more container instances must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_AddAnimationStep_ThrowsAnException_WhenNoInfrastructureNodesAreSpecified()
        {
            try
            {
                deploymentView = Views.CreateDeploymentView("deployment", "Description");
                deploymentView.AddAnimation((InfrastructureNode[]) null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("One or more infrastructure nodes must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_AddAnimationStep_ThrowsAnException_WhenNoContainerInstancesOrInfrastructureNodesAreSpecified()
        {
            try
            {
                deploymentView = Views.CreateDeploymentView("deployment", "Description");
                deploymentView.AddAnimation(null, (InfrastructureNode[]) null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("One or more container instances/infrastructure nodes must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_AddAnimationStep()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "");
            var webApplication = softwareSystem.AddContainer("Web Application", "Description", "Technology");
            var database = softwareSystem.AddContainer("Database", "Description", "Technology");
            webApplication.Uses(database, "Reads from and writes to", "JDBC/HTTPS");

            var developerLaptop = Model.AddDeploymentNode("Developer Laptop", "Description", "Technology");
            var apacheTomcat = developerLaptop.AddDeploymentNode("Apache Tomcat", "Description", "Technology");
            var oracle = developerLaptop.AddDeploymentNode("Oracle", "Description", "Technology");
            var webApplicationInstance = apacheTomcat.Add(webApplication);
            var databaseInstance = oracle.Add(database);

            deploymentView = Views.CreateDeploymentView(softwareSystem, "deployment", "Description");
            deploymentView.Add(developerLaptop);

            deploymentView.AddAnimation(webApplicationInstance);
            deploymentView.AddAnimation(databaseInstance);

            var step1 = deploymentView.Animations.First(step => step.Order == 1);
            Assert.Equal(3, step1.Elements.Count);
            Assert.True(step1.Elements.Contains(developerLaptop.Id));
            Assert.True(step1.Elements.Contains(apacheTomcat.Id));
            Assert.True(step1.Elements.Contains(webApplicationInstance.Id));
            Assert.Equal(0, step1.Relationships.Count);

            var step2 = deploymentView.Animations.First(step => step.Order == 2);
            Assert.Equal(2, step2.Elements.Count);
            Assert.True(step2.Elements.Contains(oracle.Id));
            Assert.True(step2.Elements.Contains(databaseInstance.Id));
            Assert.Equal(1, step2.Relationships.Count);
            Assert.True(step2.Relationships.Contains(webApplicationInstance.Relationships.First().Id));
        }

        [Fact]
        public void Test_AddAnimationStep_IgnoresContainerInstancesThatDoNotExistInTheView()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "");
            var webApplication = softwareSystem.AddContainer("Web Application", "Description", "Technology");
            var database = softwareSystem.AddContainer("Database", "Description", "Technology");
            webApplication.Uses(database, "Reads from and writes to", "JDBC/HTTPS");

            var developerLaptop = Model.AddDeploymentNode("Developer Laptop", "Description", "Technology");
            var apacheTomcat = developerLaptop.AddDeploymentNode("Apache Tomcat", "Description", "Technology");
            var oracle = developerLaptop.AddDeploymentNode("Oracle", "Description", "Technology");
            var webApplicationInstance = apacheTomcat.Add(webApplication);
            var databaseInstance = oracle.Add(database);

            deploymentView = Views.CreateDeploymentView(softwareSystem, "deployment", "Description");
            deploymentView.Add(apacheTomcat);

            deploymentView.AddAnimation(webApplicationInstance, databaseInstance);

            var step1 = deploymentView.Animations.First(step => step.Order == 1);
            Assert.Equal(3, step1.Elements.Count);
            Assert.True(step1.Elements.Contains(developerLaptop.Id));
            Assert.True(step1.Elements.Contains(apacheTomcat.Id));
            Assert.True(step1.Elements.Contains(webApplicationInstance.Id));
            Assert.Equal(0, step1.Relationships.Count);
        }

        [Fact]
        public void
            Test_AddAnimationStep_ThrowsAnException_WhenContainerInstancesAreSpecifiedButNoneOfThemExistInTheView()
        {
            try
            {
                var softwareSystem = Model.AddSoftwareSystem("Software System", "");
                var webApplication = softwareSystem.AddContainer("Web Application", "Description", "Technology");
                var database = softwareSystem.AddContainer("Database", "Description", "Technology");
                webApplication.Uses(database, "Reads from and writes to", "JDBC/HTTPS");

                var developerLaptop = Model.AddDeploymentNode("Developer Laptop", "Description", "Technology");
                var apacheTomcat = developerLaptop.AddDeploymentNode("Apache Tomcat", "Description", "Technology");
                var oracle = developerLaptop.AddDeploymentNode("Oracle", "Description", "Technology");
                var webApplicationInstance = apacheTomcat.Add(webApplication);
                var databaseInstance = oracle.Add(database);

                deploymentView = Views.CreateDeploymentView(softwareSystem, "deployment", "Description");

                deploymentView.AddAnimation(webApplicationInstance, databaseInstance);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("None of the specified container instances exist in this view.", ae.Message);
            }
        }

        [Fact]
        public void Test_Remove_RemovesTheInfrastructureNode()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "");
            var container = softwareSystem.AddContainer("Container", "Description", "Technology");
            var deploymentNodeParent = Model.AddDeploymentNode("Deployment Node", "Description", "Technology");
            var deploymentNodeChild =
                deploymentNodeParent.AddDeploymentNode("Deployment Node", "Description", "Technology");
            var infrastructureNode = deploymentNodeChild.AddInfrastructureNode("Infrastructure Node");
            var containerInstance = deploymentNodeChild.Add(container);

            deploymentView = Views.CreateDeploymentView(softwareSystem, "deployment", "Description");
            deploymentView.AddAllDeploymentNodes();
            Assert.Equal(4, deploymentView.Elements.Count);

            deploymentView.Remove(infrastructureNode);
            Assert.Equal(3, deploymentView.Elements.Count);
            Assert.True(deploymentView.Elements.Contains(new ElementView(deploymentNodeParent)));
            Assert.True(deploymentView.Elements.Contains(new ElementView(deploymentNodeChild)));
            Assert.True(deploymentView.Elements.Contains(new ElementView(containerInstance)));
        }

        [Fact]
        public void Test_Remove_RemovesTheContainerInstance()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "");
            var container = softwareSystem.AddContainer("Container", "Description", "Technology");
            var deploymentNodeParent = Model.AddDeploymentNode("Deployment Node", "Description", "Technology");
            var deploymentNodeChild =
                deploymentNodeParent.AddDeploymentNode("Deployment Node", "Description", "Technology");
            var infrastructureNode = deploymentNodeChild.AddInfrastructureNode("Infrastructure Node");
            var containerInstance = deploymentNodeChild.Add(container);

            deploymentView = Views.CreateDeploymentView(softwareSystem, "deployment", "Description");
            deploymentView.AddAllDeploymentNodes();
            Assert.Equal(4, deploymentView.Elements.Count);

            deploymentView.Remove(containerInstance);
            Assert.Equal(3, deploymentView.Elements.Count);
            Assert.True(deploymentView.Elements.Contains(new ElementView(deploymentNodeParent)));
            Assert.True(deploymentView.Elements.Contains(new ElementView(deploymentNodeChild)));
            Assert.True(deploymentView.Elements.Contains(new ElementView(infrastructureNode)));
        }

        [Fact]
        public void Test_Remove_RemovesTheDeploymentNodeAndChildren()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "");
            var container = softwareSystem.AddContainer("Container", "Description", "Technology");
            var deploymentNodeParent = Model.AddDeploymentNode("Deployment Node", "Description", "Technology");
            var deploymentNodeChild =
                deploymentNodeParent.AddDeploymentNode("Deployment Node", "Description", "Technology");
            var infrastructureNode = deploymentNodeChild.AddInfrastructureNode("Infrastructure Node");
            var containerInstance = deploymentNodeChild.Add(container);

            deploymentView = Views.CreateDeploymentView(softwareSystem, "deployment", "Description");
            deploymentView.AddAllDeploymentNodes();
            Assert.Equal(4, deploymentView.Elements.Count);

            deploymentView.Remove(deploymentNodeChild);
            Assert.Equal(1, deploymentView.Elements.Count);
            Assert.True(deploymentView.Elements.Contains(new ElementView(deploymentNodeParent)));
        }

        [Fact]
        public void Test_Remove_RemovesTheChildDeploymentNodeAndChildren()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "");
            var container = softwareSystem.AddContainer("Container", "Description", "Technology");
            var deploymentNodeParent = Model.AddDeploymentNode("Deployment Node", "Description", "Technology");
            var deploymentNodeChild =
                deploymentNodeParent.AddDeploymentNode("Deployment Node", "Description", "Technology");
            var infrastructureNode = deploymentNodeChild.AddInfrastructureNode("Infrastructure Node");
            var containerInstance = deploymentNodeChild.Add(container);

            deploymentView = Views.CreateDeploymentView(softwareSystem, "deployment", "Description");
            deploymentView.AddAllDeploymentNodes();
            Assert.Equal(4, deploymentView.Elements.Count);

            deploymentView.Remove(deploymentNodeParent);
            Assert.Equal(0, deploymentView.Elements.Count);
        }
    }
}