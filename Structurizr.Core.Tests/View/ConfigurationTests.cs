using System;
using Xunit;

namespace Structurizr.Core.Tests
{
    public class ViewConfigurationTests : AbstractTestBase
    {
        [Fact]
        public void test_defaultView_DoesNothing_WhenPassedNull()
        {
            var configuration = new ViewConfiguration();
            configuration.SetDefaultView(null);
            Assert.Null(configuration.DefaultView);
        }

        [Fact]
        public void test_defaultView()
        {
            var view = Views.CreateSystemLandscapeView("key", "Description");
            var configuration = new ViewConfiguration();
            configuration.SetDefaultView(view);
            Assert.Equal("key", configuration.DefaultView);
        }

        [Fact]
        public void test_copyConfigurationFrom()
        {
            var source = new ViewConfiguration();
            source.LastSavedView = "someKey";

            var destination = new ViewConfiguration();
            destination.CopyConfigurationFrom(source);
            Assert.Equal("someKey", destination.LastSavedView);
        }

        [Fact]
        public void Test_SetTheme_WithAUrl()
        {
            var configuration = new ViewConfiguration();
            configuration.Theme = "https://example.com/theme.json";
            Assert.Equal("https://example.com/theme.json", configuration.Theme);
        }

        [Fact]
        public void Test_SetTheme_WithAUrlThatHasATrailingSpaceCharacter()
        {
            var configuration = new ViewConfiguration();
            configuration.Theme = "https://example.com/theme.json ";
            Assert.Equal("https://example.com/theme.json", configuration.Theme);
        }

        [Fact]
        public void Test_SetTheme_ThrowsAnIllegalArgumentException_WhenAnInvalidUrlIsSpecified()
        {
            var configuration = new ViewConfiguration();
            Assert.Throws<ArgumentException>(() =>
                configuration.Theme = "blah"
            );
        }

        [Fact]
        public void Test_SetTheme_DoesNothing_WhenANullUrlIsSpecified()
        {
            var configuration = new ViewConfiguration();
            configuration.Theme = null;
            Assert.Null(configuration.Theme);
        }

        [Fact]
        public void Test_SetTheme_DoesNothing_WhenAnEmptyUrlIsSpecified()
        {
            var configuration = new ViewConfiguration();
            configuration.Theme = " ";
            Assert.Null(configuration.Theme);
        }
    }
}