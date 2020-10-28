using System;
using System.Linq;
using Xunit;

namespace Structurizr.Core.Tests.View
{
    public class StaticViewTests : AbstractTestBase
    {
        [Fact]
        public void Test_AddAnimationStep_ThrowsAnException_WhenNoElementsAreSpecified()
        {
            try
            {
                var view = Workspace.Views.CreateSystemLandscapeView("key", "Description");
                view.AddAnimation();
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("One or more elements must be specified.", ae.Message);
            }
        }

        [Fact]
        public void Test_AddAnimationStep()
        {
            var element1 = Model.AddSoftwareSystem("Software System 1", "");
            var element2 = Model.AddSoftwareSystem("Software System 2", "");
            var element3 = Model.AddSoftwareSystem("Software System 3", "");

            var relationship1_2 = element1.Uses(element2, "uses");
            var relationship2_3 = element2.Uses(element3, "uses");

            var view = Workspace.Views.CreateSystemLandscapeView("key", "Description");
            view.AddAllElements();

            view.AddAnimation(element1);
            view.AddAnimation(element2);
            view.AddAnimation(element3);

            var step1 = view.Animations.First(step => step.Order == 1);
            Assert.Equal(1, step1.Elements.Count);
            Assert.True(step1.Elements.Contains(element1.Id));
            Assert.Equal(0, step1.Relationships.Count);

            var step2 = view.Animations.First(step => step.Order == 2);
            Assert.Equal(1, step2.Elements.Count);
            Assert.True(step2.Elements.Contains(element2.Id));
            Assert.Equal(1, step2.Relationships.Count);
            Assert.True(step2.Relationships.Contains(relationship1_2.Id));

            var step3 = view.Animations.First(step => step.Order == 3);
            Assert.Equal(1, step3.Elements.Count);
            Assert.True(step3.Elements.Contains(element3.Id));
            Assert.Equal(1, step3.Relationships.Count);
            Assert.True(step3.Relationships.Contains(relationship2_3.Id));
        }

        [Fact]
        public void Test_AddAnimationStep_IgnoresElementsThatDoNotExistInTheView()
        {
            var element1 = Model.AddSoftwareSystem("Software System 1", "");
            var element2 = Model.AddSoftwareSystem("Software System 2", "");

            var view = Workspace.Views.CreateSystemLandscapeView("key", "Description");
            view.Add(element1);
            view.AddAnimation(element1, element2);

            var step1 = view.Animations.First(step => step.Order == 1);
            Assert.Equal(1, step1.Elements.Count);
            Assert.True(step1.Elements.Contains(element1.Id));
        }

        [Fact]
        public void Test_AddAnimationStep_ThrowsAnException_WhenElementsAreSpecifiedButNoneOfThemExistInTheView()
        {
            try
            {
                var element1 = Model.AddSoftwareSystem("Software System 1", "");

                var view = Workspace.Views.CreateSystemLandscapeView("key", "Description");
                view.AddAnimation(element1);
                throw new TestFailedException();
            }
            catch (ArgumentException ae)
            {
                Assert.Equal("None of the specified elements exist in this view.", ae.Message);
            }
        }
    }
}