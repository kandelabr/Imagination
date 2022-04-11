using Imagination.Configuration;
using Imagination.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Imagination.Server.Test
{
    public class DependencyInjectorForTestProjects
    {
        private readonly ServiceCollection _services;

        public DependencyInjectorForTestProjects()
        {
            _services = new ServiceCollection();
            _services.AddLogging();
            _services.AddHandlers();
            AddConfiguration();
        }

        public TService GetService<TService>()
        {
            ServiceProvider serviceProvider = _services.BuildServiceProvider();
            return serviceProvider.GetService<TService>();
        }

        private void AddConfiguration()
        {
            IConfiguration configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false)
               .Build();

            _services.AddConfigurations(configuration);
        }
    }
}
