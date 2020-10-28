using System;
using Xunit;

namespace Structurizr.Core.Tests
{
    public class ElementTests : AbstractTestBase
    {
        [Fact]
        public void Test_HasEfferentRelationshipWith_ReturnsFalse_WhenANullElementIsSpecified()
        {
            var softwareSystem1 = Model.AddSoftwareSystem("System 1", "");
            Assert.False(softwareSystem1.HasEfferentRelationshipWith(null));
        }

        [Fact]
        public void
            Test_HasEfferentRelationshipWith_ReturnsFalse_WhenTheSameElementIsSpecifiedAndNoCyclicRelationshipExists()
        {
            var softwareSystem1 = Model.AddSoftwareSystem("System 1", "");
            Assert.False(softwareSystem1.HasEfferentRelationshipWith(softwareSystem1));
        }

        [Fact]
        public void
            Test_HasEfferentRelationshipWith_ReturnsTrue_WhenTheSameElementIsSpecifiedAndACyclicRelationshipExists()
        {
            var softwareSystem1 = Model.AddSoftwareSystem("System 1", "");
            softwareSystem1.Uses(softwareSystem1, "uses");
            Assert.True(softwareSystem1.HasEfferentRelationshipWith(softwareSystem1));
        }

        [Fact]
        public void Test_HasEfferentRelationshipWith_ReturnsTrue_WhenThereIsARelationship()
        {
            var softwareSystem1 = Model.AddSoftwareSystem("System 1", "");
            var softwareSystem2 = Model.AddSoftwareSystem("System 2", "");
            softwareSystem1.Uses(softwareSystem2, "uses");
            Assert.True(softwareSystem1.HasEfferentRelationshipWith(softwareSystem2));
        }

        [Fact]
        public void Test_GetEfferentRelationshipWith_ReturnsNull_WhenANullElementIsSpecified()
        {
            var softwareSystem1 = Model.AddSoftwareSystem("System 1", "");
            Assert.Null(softwareSystem1.GetEfferentRelationshipWith(null));
        }

        [Fact]
        public void
            Test_GetEfferentRelationshipWith_ReturnsNull_WhenTheSameElementIsSpecifiedAndNoCyclicRelationshipExists()
        {
            var softwareSystem1 = Model.AddSoftwareSystem("System 1", "");
            Assert.Null(softwareSystem1.GetEfferentRelationshipWith(softwareSystem1));
        }

        [Fact]
        public void
            Test_GetEfferentRelationshipWith_ReturnsCyclicRelationship_WhenTheSameElementIsSpecifiedAndACyclicRelationshipExists()
        {
            var softwareSystem1 = Model.AddSoftwareSystem("System 1", "");
            softwareSystem1.Uses(softwareSystem1, "uses");

            var relationship = softwareSystem1.GetEfferentRelationshipWith(softwareSystem1);
            Assert.Same(softwareSystem1, relationship.Source);
            Assert.Equal("uses", relationship.Description);
            Assert.Same(softwareSystem1, relationship.Destination);
        }

        [Fact]
        public void Test_GetEfferentRelationshipWith_ReturnsTheRelationship_WhenThereIsARelationship()
        {
            var softwareSystem1 = Model.AddSoftwareSystem("System 1", "");
            var softwareSystem2 = Model.AddSoftwareSystem("System 2", "");
            softwareSystem1.Uses(softwareSystem2, "uses");

            var relationship = softwareSystem1.GetEfferentRelationshipWith(softwareSystem2);
            Assert.Same(softwareSystem1, relationship.Source);
            Assert.Equal("uses", relationship.Description);
            Assert.Same(softwareSystem2, relationship.Destination);
        }

        [Fact]
        public void Test_HasAfferentRelationships_ReturnsFalse_WhenThereAreNoIncomingRelationships()
        {
            var softwareSystem1 = Model.AddSoftwareSystem("System 1", "");
            var softwareSystem2 = Model.AddSoftwareSystem("System 2", "");
            softwareSystem1.Uses(softwareSystem2, "Uses");

            Assert.False(softwareSystem1.HasAfferentRelationships());
        }

        [Fact]
        public void Test_HasAfferentRelationships_ReturnsTrue_WhenThereAreIncomingRelationships()
        {
            var softwareSystem1 = Model.AddSoftwareSystem("System 1", "");
            var softwareSystem2 = Model.AddSoftwareSystem("System 2", "");
            softwareSystem1.Uses(softwareSystem2, "Uses");

            Assert.True(softwareSystem2.HasAfferentRelationships());
        }

        [Fact]
        public void Test_SetUrl_DoesNotThrowAnException_WhenANullUrlIsSpecified()
        {
            var element = Model.AddSoftwareSystem("Name", "Description");
            element.Url = null;
        }

        [Fact]
        public void Test_SetUrl_DoesNotThrowAnException_WhenAnEmptyUrlIsSpecified()
        {
            var element = Model.AddSoftwareSystem("Name", "Description");
            element.Url = "";
        }

        [Fact]
        public void Test_SetUrl_ThrowsAnException_WhenAnInvalidUrlIsSpecified()
        {
            try
            {
                var element = Model.AddSoftwareSystem("Name", "Description");
                element.Url = "www.somedomain.com";
                throw new TestFailedException();
            }
            catch (Exception e)
            {
                Assert.Equal("www.somedomain.com is not a valid URL.", e.Message);
            }
        }

        [Fact]
        public void Test_SetUrl_DoesNotThrowAnException_WhenAValidUrlIsSpecified()
        {
            var element = Model.AddSoftwareSystem("Name", "Description");
            element.Url = "http://www.somedomain.com";
            Assert.Equal("http://www.somedomain.com", element.Url);
        }
    }
}