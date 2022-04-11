using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Imagination.Configuration
{
    /// <summary>
    /// IServiceCollection extensions methods. Used to add configuration to the project.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add configuration.
        /// </summary>
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

            services.AddOptions<ImageExtensionConfigurationCollection>()
                .Bind(configuration.GetSection(nameof(ImageExtensionConfigurationCollection)));

            return services;
        }
    }
}
