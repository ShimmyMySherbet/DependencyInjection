using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using ShimmyMySherbet.DependencyInjection.Models.Exceptions;
using ShimmyMySherbet.DependencyInjection.Models.Interfaces;
using ShimmyMySherbet.DependencyInjection.Models.Lifetimes;

namespace ShimmyMySherbet.DependencyInjection.Models
{
    public class TypeActivator : ITypeActivator
    {
        public T ActivateType<T>(IContainerServiceCollection services, params object[] extras)
        {
            return (T)ActivateType(services, typeof(T), extras);
        }

        public object ActivateType(IContainerServiceCollection services, Type type)
        {
            return ActivateType(services, type, Array.Empty<object>());
        }

        public object ActivateType(IContainerServiceCollection containerServices, Type type, object[] extras)
        {
            if (type.IsTypeLogger())
            {
                var logger = ActivateTypeLogger(containerServices, type);
                if (logger != null)
                {
                    return logger;
                }
            }

            foreach (var constructor in type.GetConstructors())
            {
                var parameters = constructor.GetParameters();
                var services = new IContainerService[parameters.Length];
                var valid = true;

                for (int i = 0; i < services.Length; i++)
                {
                    var parameter = parameters[i];

                    var overloadService = extras.FirstOrDefault(x => parameter.ParameterType.IsAssignableFrom(x.GetType()));
                    if (overloadService != null)
                    {
                        services[i] = new ScopeService(overloadService);
                        continue;
                    }

                    var service = containerServices.GetService(parameter.ParameterType);
                    if (service != null)
                    {
                        services[i] = service;
                        continue;
                    }

                    valid = false;
                    break;
                }

                if (!valid)
                {
                    continue;
                }

                var arguments = new object[services.Length];

                for (int i = 0; i < arguments.Length; i++)
                {
                    arguments[i] = services[i].GetInstance();
                }

                return constructor.Invoke(parameters: arguments);
            }

            throw new FailedToActivateException(type);
        }

        public static object? ActivateTypeLogger(IContainerServiceCollection containerServices, Type type)
        {
            var generics = type.GetGenericArguments();
            if (generics.Length != 1)
            {
                return null;
            }

            var loggerType = generics[0];

            var factory = containerServices.GetService(typeof(ILoggerFactory));
            if (factory == null)
            {
                return null;
            }

            var factoryInstance = (ILoggerFactory)factory.GetInstance();

            var logger = factoryInstance.CreateLogger(loggerType);

            if (type.IsAssignableFrom(logger.GetType())) // Safety check
            {
                return logger;
            }

            return null;
        }
    }
}