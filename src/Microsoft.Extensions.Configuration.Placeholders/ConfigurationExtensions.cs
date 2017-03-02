using System;
using System.Text.RegularExpressions;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Prvodides extensions for <see cref="IConfiguration"/> instances.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// A regex which matches tokens in the following format: [Item:Sub1:Sub2].
        /// </summary>
        private static readonly Regex ConfigPlaceholderRegex = new Regex(@"\[([A-Za-z:]+?)\]");

        /// <summary>
        /// Replaces the placeholders in the specified <see cref="IConfiguration"/> instance.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance to replace placeholders in.</param>
        /// <returns>The given <see cref="IConfiguration"/> instance.</returns>
        public static IConfiguration ReplacePlaceholders(this IConfiguration configuration)
        {
            foreach (var kvp in configuration.AsEnumerable())
            {
                if (string.IsNullOrEmpty(kvp.Value))
                {
                    // Skip empty configuration values.
                    continue;
                }

                var match = ConfigPlaceholderRegex.Match(kvp.Value);
                if (!match.Success)
                {
                    // Skip non-matching configuration values.
                    continue;
                }

                if (match.Groups.Count != 2)
                {
                    // There is a match, but somehow no group for the placeholder.
                    throw InvalidPlaceholderException(match.ToString());
                }

                var placeholder = match.Groups[1].Value;
                if (placeholder.StartsWith(":") || placeholder.EndsWith(":"))
                {
                    // Placeholders cannot start of end with a colon.
                    throw InvalidPlaceholderException(placeholder);
                }

                // Replace the placeholder with the value in the configuration.
                var configValue = configuration[placeholder];
                configuration[kvp.Key] = configValue;
            }

            return configuration;
        }

        private static Exception InvalidPlaceholderException(string placeholder)
        {
            return new InvalidOperationException($"Invalid configuration placeholder: '{placeholder}'.");
        }
    }
}
