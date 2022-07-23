using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using ShimmyMySherbet.DependencyInjection.Models.Interfaces;
using ShimmyMySherbet.DependencyInjection.Models.Lifetimes;

namespace ShimmyMySherbet.DependencyInjection.Models
{
    public class ConfigurationBuilder : IConfigurationBuilder
    {
        private readonly IContainerServiceCollection m_Services;

        public ConfigurationBuilder(IContainerServiceCollection services)
        {
            m_Services = services;
        }

        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();
        public IList<IConfigurationSource> Sources { get; } = new List<IConfigurationSource>();

        public List<IConfigurationSource> UncommitedSources { get; } = new List<IConfigurationSource>();

        public IConfigurationBuilder Add(IConfigurationSource source)
        {
            lock (UncommitedSources)
            {
                UncommitedSources.Add(source);
            }
            return this;
        }

        public IConfigurationRoot Build()
        {
            lock (UncommitedSources)
            {
                foreach (var source in UncommitedSources)
                    Sources.Add(source);
                UncommitedSources.Clear();
            }

            var configRoot = new ConfigurationRoot(Sources
                .Select(x => x.Build(this))
                .ToList());

            m_Services.RemoveService(typeof(IConfiguration));
            m_Services.AddService(new SingletonService(typeof(IConfiguration), configRoot));

            return configRoot;
        }
    }
}