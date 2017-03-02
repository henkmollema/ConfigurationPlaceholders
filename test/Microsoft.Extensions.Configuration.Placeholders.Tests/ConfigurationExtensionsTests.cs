using System.Collections.Generic;
using Xunit;

namespace Microsoft.Extensions.Configuration.Placeholders.Tests
{
    public class ConfigurationExtensionsTests
    {
        [Fact]
        public void ReplacePlaceholders_Replaces_PlaceholdersInConfigValue()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["foo:bar:baz"] = "qux",
                    ["foo:quux"] = "[foo:bar:baz]",
                    ["foo:baz"] = "[foo:bar:baz]-foo-[foo:quux]"
                })
                .Build();

            // Act
            config.ReplacePlaceholders();

            // Assert
            Assert.Equal("qux", config["foo:bar:baz"]);
            Assert.Equal("qux", config["foo:quux"]);
            Assert.Equal("qux-foo-qux", config["foo:baz"]);
        }
    }
}
