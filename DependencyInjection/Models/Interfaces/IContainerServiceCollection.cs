using System;
using System.Collections.Generic;

namespace ShimmyMySherbet.DependencyInjection.Models.Interfaces
{
    public interface IContainerServiceCollection
    {
        IContainerService? GetService(Type type);

        IContainerService[] GetServices(Type type);

        void AddService(IContainerService service);

        void RemoveService(IContainerService service);

        IEnumerator<object> GetServiceIterator(Type t);
        IEnumerator<T> GetServiceIterator<T>();
    }
}