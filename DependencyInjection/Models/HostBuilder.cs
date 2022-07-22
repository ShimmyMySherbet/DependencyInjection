using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ShimmyMySherbet.DependencyInjection.Models
{
    public class HostBuilder : IHostBuilder
    {
        private readonly ServiceHost m_Host;

        public HostBuilder()
        {
            m_Host = new ServiceHost();
        }

        public HostBuilder(ServiceHost host)
        {
            m_Host = host;
        }

        public HostBuilderContext GetContext()
        {
            var config = m_Host.TryGetService<IConfiguration>();
            return new HostBuilderContext(Properties)
            {
                Configuration = config
            };
        }

        public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

        public IHost Build()
        {
            return m_Host;
        }

        public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            configureDelegate(GetContext(), m_Host.Configuration);
            return this;
        }

        public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            configureDelegate(GetContext(), m_Host.ActivateType<TContainerBuilder>());
            return this;
        }

        public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            configureDelegate(m_Host.Configuration);
            return this;
        }

        public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            var passThrough = new CompatibleServiceCollection(m_Host); // Compatability layer
            configureDelegate(GetContext(), passThrough);
            return this;
        }

        [Obsolete("Not Supported")]
        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> _) where TContainerBuilder : notnull
        {
            throw new NotSupportedException("Custom Service Provider factories are not supported.");
        }

        [Obsolete("Not Supported")]
        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory) where TContainerBuilder : notnull
        {
            throw new NotSupportedException("Custom Service Provider factories are not supported.");
        }
    }
}