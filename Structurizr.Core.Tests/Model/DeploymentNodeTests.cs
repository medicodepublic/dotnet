using System;
using Structurizr.Core.Util;
using Xunit;

namespace Structurizr.Core.Tests
{
    public class DeploymentNodeTests : AbstractTestBase
    {
        [Fact]
        public void Test_CanonicalName_WhenTheDeploymentNodeHasNoParent()
        {
            var deploymentNode = new DeploymentNode();
            deploymentNode.Name = "Ubuntu Server";

            Assert.Equal("/Deployment/Default/Ubuntu Server", deploymentNode.CanonicalName);
        }

        [Fact]
        public void Test_CanonicalName_WhenTheDeploymentNodeHasAParent()
        {
            var parent = new DeploymentNode();
            parent.Name = "Ubuntu Server";

            var child = new DeploymentNode();
            child.Name = "Apache Tomcat";
            child.Parent = parent;

            Assert.Equal("/Deployment/Default/Ubuntu Server/Apache Tomcat", child.CanonicalName);
        }

        [Fact]
        public void Test_Parent_ReturnsTheParentDeploymentNode()
        {
            var parent = new DeploymentNode();
            Assert.Null(parent.Parent);

            var child = new DeploymentNode();
            child.Parent = parent;
            Assert.Same(parent, child.Parent);
        }

        [Fact]
        public void Test_RequiredTags()
        {
            var deploymentNode = new DeploymentNode();
            Assert.Equal(2, deploymentNode.GetRequiredTags().Count);
        }

        [Fact]
        public void Test_Tags()
        {
            var deploymentNode = new DeploymentNode();
            Assert.Equal("Element,Deployment Node", deploymentNode.Tags);
        }


        [Fact]
        public void Test_RemoveTags_DoesNotRemoveRequiredTags()
        {
            var deploymentNode = new DeploymentNode();
            Assert.True(deploymentNode.Tags.Contains(Tags.Element));
            Assert.True(deploymentNode.Tags.Contains(Tags.DeploymentNode));

            deploymentNode.RemoveTag(Tags.DeploymentNode);
            deploymentNode.RemoveTag(Tags.Element);

            Assert.True(deploymentNode.Tags.Contains(Tags.Element));
            Assert.True(deploymentNode.Tags.Contains(Tags.DeploymentNode));
        }

        [Fact]
        public void Test_Add_ThrowsAnException_WhenAContainerIsNotSpecified()
        {
            try
            {
                var deploymentNode = new DeploymentNode();
                deploymentNode.Add(null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("A container must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_Add_AddsAContainerInstance_WhenAContainerIsSpecified()
        {
            var softwareSystem = Model.AddSoftwareSystem("Software System", "");
            var container = softwareSystem.AddContainer("Container", "", "");
            var deploymentNode = Model.AddDeploymentNode("Deployment Node", "", "");
            var containerInstance = deploymentNode.Add(container);

            Assert.NotNull(containerInstance);
            Assert.Same(container, containerInstance.Container);
            Assert.True(deploymentNode.ContainerInstances.Contains(containerInstance));
        }

        [Fact]
        public void Test_AddDeploymentNode_ThrowsAnException_WhenANameIsNotSpecified()
        {
            try
            {
                var parent = Model.AddDeploymentNode("Parent", "", "");
                parent.AddDeploymentNode(null, "", "");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("A name must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_AddDeploymentNode_AddsAChildDeploymentNode_WhenANameIsSpecified()
        {
            var parent = Model.AddDeploymentNode("Parent", "", "");

            var child = parent.AddDeploymentNode("Child 1", "Description", "Technology");
            Assert.NotNull(child);
            Assert.Equal("Child 1", child.Name);
            Assert.Equal("Description", child.Description);
            Assert.Equal("Technology", child.Technology);
            Assert.Equal(1, child.Instances);
            Assert.Equal(0, child.Properties.Count);
            Assert.True(parent.Children.Contains(child));

            child = parent.AddDeploymentNode("Child 2", "Description", "Technology", 4);
            Assert.NotNull(child);
            Assert.Equal("Child 2", child.Name);
            Assert.Equal("Description", child.Description);
            Assert.Equal("Technology", child.Technology);
            Assert.Equal(4, child.Instances);
            Assert.Equal(0, child.Properties.Count);
            Assert.True(parent.Children.Contains(child));

            child = parent.AddDeploymentNode("Child 3", "Description", "Technology", 4,
                DictionaryUtils.Create("name=value"));
            Assert.NotNull(child);
            Assert.Equal("Child 3", child.Name);
            Assert.Equal("Description", child.Description);
            Assert.Equal("Technology", child.Technology);
            Assert.Equal(4, child.Instances);
            Assert.Equal(1, child.Properties.Count);
            Assert.Equal("value", child.Properties["name"]);
            Assert.True(parent.Children.Contains(child));
        }

        [Fact]
        public void Test_Uses_ThrowsAnException_WhenANullDestinationIsSpecified()
        {
            try
            {
                var deploymentNode = Model.AddDeploymentNode("Deployment Node", "", "");
                deploymentNode.Uses(null, "", "");
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("The destination must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_Uses_AddsARelationship()
        {
            var primaryNode = Model.AddDeploymentNode("MySQL - Primary", "", "");
            var secondaryNode = Model.AddDeploymentNode("MySQL - Secondary", "", "");
            var relationship = primaryNode.Uses(secondaryNode, "Replicates data to", "Some technology");

            Assert.NotNull(relationship);
            Assert.Same(primaryNode, relationship.Source);
            Assert.Same(secondaryNode, relationship.Destination);
            Assert.Equal("Replicates data to", relationship.Description);
            Assert.Equal("Some technology", relationship.Technology);
        }

        [Fact]
        public void Test_GetDeploymentNodeWithName_ThrowsAnException_WhenANameIsNotSpecified()
        {
            try
            {
                var deploymentNode = new DeploymentNode();
                deploymentNode.GetDeploymentNodeWithName(null);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("A name must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_GetDeploymentNodeWithName_ReturnsNull_WhenThereIsNoDeploymentNodeWithTheSpecifiedName()
        {
            var deploymentNode = new DeploymentNode();
            Assert.Null(deploymentNode.GetDeploymentNodeWithName("foo"));
        }

        [Fact]
        public void
            Test_GetDeploymentNodeWithName_ReturnsTheNamedDeploymentNode_WhenThereIsADeploymentNodeWithTheSpecifiedName()
        {
            var parent = Model.AddDeploymentNode("parent", "", "");
            var child = parent.AddDeploymentNode("child", "", "");
            Assert.Same(child, parent.GetDeploymentNodeWithName("child"));
        }

        [Fact]
        public void Test_GetInfrastructureNodeWithName_ReturnsNull_WhenThereIsNoInfrastructureNodeWithTheSpecifiedName()
        {
            var deploymentNode = new DeploymentNode();
            Assert.Null(deploymentNode.GetInfrastructureNodeWithName("foo"));
        }

        [Fact]
        public void
            Test_GetInfrastructureNodeWithName_ReturnsTheNamedDeploymentNode_WhenThereIsAInfrastructureNodeWithTheSpecifiedName()
        {
            var parent = Model.AddDeploymentNode("parent", "", "");
            var child = parent.AddInfrastructureNode("child", "", "");
            Assert.Same(child, parent.GetInfrastructureNodeWithName("child"));
        }

        [Fact]
        public void Test_Instances()
        {
            var deploymentNode = new DeploymentNode();
            deploymentNode.Instances = 8;

            Assert.Equal(8, deploymentNode.Instances);
        }

        [Fact]
        public void Test_Instances_ThrowsAnException_WhenZeroIsSpecified()
        {
            try
            {
                var deploymentNode = new DeploymentNode();
                deploymentNode.Instances = 0;
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("Number of instances must be a positive integer.", ae.Message);
            }
        }

        [Fact]
        public void Test_Instances_ThrowsAnException_WhenANegativeNumberIsSpecified()
        {
            try
            {
                var deploymentNode = new DeploymentNode();
                deploymentNode.Instances = -1;
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("Number of instances must be a positive integer.", ae.Message);
            }
        }
    }
}