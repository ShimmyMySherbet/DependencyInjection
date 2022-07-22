using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace ShimmyMySherbet.DependencyInjection.Models
{
    public class CompatibleServiceCollection : IServiceCollection
    {
        private readonly ServiceHost m_Host;

        public CompatibleServiceCollection(ServiceHost host)
        {
            m_Host = host;
        }

        public ServiceDescriptor this[int index]
        {
            get => m_Host.ServiceCollection.GetAtIndex(index).ToDescriptor();
            set => m_Host.ServiceCollection.InsertAt(value.ToService(m_Host), index);
        }

        public int Count => m_Host.ServiceCollection.Count;
        public bool IsReadOnly => false;

        public void Add(ServiceDescriptor item)
        {
            var service = item.ToService(m_Host);
            m_Host.ServiceCollection.AddService(service);
        }

        [Obsolete("This container does not support service clearing")]
        public void Clear()
        {
            throw new NotSupportedException("Clearing is not supported");
        }

        [Obsolete("This container does not ServiceDescriptor checking")]
        public bool Contains(ServiceDescriptor item)
        {
            return false;
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < array.Length; i++)
            {
                var root = i - arrayIndex;
                var service = m_Host.ServiceCollection.GetAtIndex(root);
                array[i] = service.ToDescriptor();
            }
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            using var en = m_Host.ServiceCollection.GetServices();
            while (en.MoveNext())
            {
                yield return en.Current.ToDescriptor();
            }
        }

        [Obsolete("This container does not ServiceDescriptor checking")]
        public int IndexOf(ServiceDescriptor item)
        {
            return -1;
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            m_Host.ServiceCollection.InsertAt(item.ToService(m_Host), index);
        }

        [Obsolete("This container does not ServiceDescriptor checking")]
        public bool Remove(ServiceDescriptor item)
        {
            return false;
        }

        public void RemoveAt(int index)
        {
            m_Host.ServiceCollection.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}