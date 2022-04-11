using Microsoft.Extensions.DependencyInjection;

namespace Imagination.Handlers
{
    /// <summary>
    /// IServiceCollection extensions methods. Used to add handler dependencies to the project.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add handlers.
        /// </summary>
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IImagesFormatProvider, ConfigImagesFormatProvider>();
            services.AddSingleton<IExtensionBuilder, ExtensionBuilder>();
            services.AddSingleton<IImageConverter, ImageConverter>();
            return services;
        }
    }
}
