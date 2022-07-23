using System;
using System.Reflection;
using ShimmyMySherbet.DependencyInjection.Models.Exceptions;
using ShimmyMySherbet.DependencyInjection.Models.Interfaces;

namespace ShimmyMySherbet.DependencyInjection.Models
{
    public class DynamicInvoker : IDynamicInvoker
    {
        private readonly IContainerServiceCollection m_Services;

        public DynamicInvoker(IContainerServiceCollection services)
        {
            m_Services = services;
        }

        public object? Invoke(MethodInfo info, object? instance = null)
        {
            var parameters = info.GetParameters();
            var arguments = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var service = m_Services.GetService(parameter.ParameterType);

                if (service == null)
                {
                    throw new CannotInvokeException(info, parameter);
                }
            }

            return info.Invoke(instance, arguments);
        }

        public T? Invoke<T>(MethodInfo info, object? instance = null)
        {
            return (T?)Invoke(info, instance);
        }

        public object? Invoke(Delegate @delegate)
        {
            return Invoke(@delegate.Method, @delegate.Target);
        }

        public T? Invoke<T>(Delegate @delegate)
        {
            return (T?)Invoke(@delegate);
        }
    }
}