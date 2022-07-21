using System;

namespace ShimmyMySherbet.DependencyInjection.Models.Exceptions
{
    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(Type type) : base($"The service {type.FullName} was not found in the container")
        {
        }
    }
}