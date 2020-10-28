using Structurizr.Util;
using Xunit;

namespace Structurizr.Core.Tests
{
    public class UrlTests
    {
        [Fact]
        public void test_IsUrl_ReturnsFalse_WhenPassedNull()
        {
            Assert.False(Url.IsUrl(null));
        }

        [Fact]
        public void test_IsUrl_ReturnsFalse_WhenPassedAnEmptyString()
        {
            Assert.False(Url.IsUrl(""));
            Assert.False(Url.IsUrl(" "));
        }

        [Fact]
        public void test_IsUrl_ReturnsFalse_WhenPassedAnInvalidUrl()
        {
            Assert.False(Url.IsUrl("www.google.com"));
        }

        [Fact]
        public void test_IsUrl_ReturnsTrue_WhenPassedAValidUrl()
        {
            Assert.True(Url.IsUrl("https://www.google.com"));
        }
    }
}