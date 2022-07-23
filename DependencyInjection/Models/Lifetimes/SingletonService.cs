using System;
using ShimmyMySherbet.DependencyInjection.Models.Interfaces;

namespace ShimmyMySherbet.DependencyInjection.Models.Lifetimes
{
    public class SingletonService : IContainerService
    {
        public Type Type { get; }

        private readonly ServiceHost? m_Container;

        private object? m_Instance = null;

        private readonly Func<object?>? m_Factory = null;

        public SingletonService(ServiceHost container, Type type)
        {
            Type = type;
            m_Container = container;
        }

        public SingletonService(Type type, object instance)
        {
            m_Instance = instance;
            Type = type;
        }

        public SingletonService(Func<object?> factory, Type type)
        {
            Type = type;
            m_Factory = factory;
        }

        public object GetInstance()
        {
            if (m_Instance != null)
            {
                return m_Instance;
            }

            if (m_Factory != null)
            {
                m_Instance = m_Factory();
            }

            if (m_Container != null)
            {
                m_Instance = m_Container.ActivateType(Type, Array.Empty<object>());
            }

            if (m_Instance == null)
            {
                throw new ArgumentNullException($"Service instantiator returned a null service for {Type.FullName}");
            }

            return m_Instance;
        }
    }
}