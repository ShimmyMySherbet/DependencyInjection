using System;
using System.Collections.Generic;
using System.Linq;
using ShimmyMySherbet.DependencyInjection.Models.Interfaces;

namespace ShimmyMySherbet.DependencyInjection.Models
{
    public class ServicesContainer : IContainerServiceCollection
    {
        private List<IContainerService> m_Services = new List<IContainerService>();

        public IContainerService? GetService(Type type)
        {
            lock (m_Services)
            {
                return m_Services.FirstOrDefault(x => type.IsAssignableFrom(x.Type));
            }
        }

        public IContainerService[] GetServices(Type type)
        {
            lock (m_Services)
            {
                return m_Services.Where(x => type.IsAssignableFrom(x.Type)).ToArray();
            }
        }

        public void AddService(IContainerService service)
        {
            lock (m_Services)
            {
                m_Services.Add(service);
            }
        }

        public void RemoveService(IContainerService service)
        {
            lock (m_Services)
            {
                m_Services.Remove(service);
            }
        }

        public IEnumerator<object> GetServiceIterator(Type type)
        {
            for (int i = 0; i < m_Services.Count; i++)
            {
                var service = m_Services[i];

                if (type.IsAssignableFrom(service.Type))
                {
                    yield return service.GetInstance();
                }
            }
        }

        public IEnumerator<T> GetServiceIterator<T>()
        {
            for (int i = 0; i < m_Services.Count; i++)
            {
                var service = m_Services[i];

                if (typeof(T).IsAssignableFrom(service.Type))
                {
                    yield return (T)service.GetInstance();
                }
            }
        }
    }
}