using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShimmyMySherbet.DependencyInjection.Models.Interfaces;
using ShimmyMySherbet.DependencyInjection.Models.Lifetimes;

namespace ShimmyMySherbet.DependencyInjection.Models
{
    public static class Extensions
    {
        public static bool IsTypeLogger(this Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ILogger<>);
        }

        public static IContainerService ToService(this ServiceDescriptor descriptor, ServiceHost provider)
        {
            var type = descriptor.ImplementationType ?? descriptor.ServiceType;
            switch (descriptor.Lifetime)
            {
                case ServiceLifetime.Scoped:
                case ServiceLifetime.Singleton:
                    if (descriptor.ImplementationFactory != null)
                    {
                        return new SingletonService(() => descriptor.ImplementationFactory(provider), type);
                    }
                    else if (descriptor.ImplementationInstance != null)
                    {
                        return new SingletonService(type, descriptor.ImplementationInstance);
                    }
                    else
                    {
                        return new SingletonService(provider, type);
                    }

                case ServiceLifetime.Transient:
                    if (descriptor.ImplementationFactory != null)
                    {
                        return new TransientService(() => descriptor.ImplementationFactory(provider), type);
                    }
                    else
                    {
                        return new TransientService(provider, type);
                    }

                default:
                    throw new NotSupportedException($"Unknown service lifetime '{descriptor.Lifetime}'");
            }
        }

        public static ServiceDescriptor ToDescriptor(this IContainerService service)
        {
            ServiceLifetime lifetime;
            if (service is SingletonService)
            {
                lifetime = ServiceLifetime.Singleton;
            }
            else if (service is TransientService)
            {
                lifetime = ServiceLifetime.Transient;
            }
            else if (service is ScopeService)
            {
                lifetime = ServiceLifetime.Scoped;
            }
            else
            {
                lifetime = ServiceLifetime.Singleton;
            }
            return new ServiceDescriptor(service.Type, (_) => service.GetInstance(), lifetime);
        }
    }
}