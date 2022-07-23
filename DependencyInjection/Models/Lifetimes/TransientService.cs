using System;
using ShimmyMySherbet.DependencyInjection.Models.Interfaces;

namespace ShimmyMySherbet.DependencyInjection.Models.Lifetimes
{
    public class TransientService : IContainerService
    {
        public Type Type { get; }

        private readonly ServiceHost? m_Container;

        private readonly Func<object?>? m_Factory;

        public TransientService(ServiceHost container, Type type)
        {
            Type = type;
            m_Container = container;
        }

        public TransientService(Func<object?> factory, Type type)
        {
            Type = type;
            m_Factory = factory;
        }

        public object GetInstance()
        {
            if (m_Factory != null)
            {
                var serviceInstance = m_Factory();
                if (serviceInstance == null)
                {
                    throw new ArgumentNullException($"Service instantiator returned a null service for {Type.FullName}");
                }
                return serviceInstance;
            }
            else if (m_Container != null)
            {
                return m_Container.ActivateType(Type);
            }
            throw new InvalidOperationException(); // Shouldn't be possible.
        }
    }
}