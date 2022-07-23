using System;
using System.Collections.Generic;

namespace ShimmyMySherbet.DependencyInjection.Models.Interfaces
{
    public interface IContainerServiceCollection
    {
        int Count { get; }

        IContainerService? GetService(Type type);

        IContainerService[] GetServices(Type type);

        void AddService(IContainerService service);

        void RemoveService(IContainerService service);

        int RemoveService(Type type);

        IEnumerator<object> GetServiceIterator(Type t);

        IEnumerator<T> GetServiceIterator<T>();

        IContainerService GetAtIndex(int index);

        void RemoveAt(int index);

        IEnumerator<IContainerService> GetServices();

        void InsertAt(IContainerService service, int index);
    }
}