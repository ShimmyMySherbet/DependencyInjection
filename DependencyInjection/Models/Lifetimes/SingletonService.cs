using System;
using ShimmyMySherbet.DependencyInjection;
using ShimmyMySherbet.DependencyInjection.Models.Interfaces;

namespace ShimmyMySherbet.DependencyInjection.Models.Lifetimes
{
    public class SingletonService : IContainerService
    {
        public Type Type { get; }

        private ServiceHost? m_Container;

        private object? m_Instance = null;

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

        public object GetInstance()
        {
            if (m_Instance == null)
            {
                if (m_Container == null)
                    throw new InvalidOperationException();

                m_Instance = m_Container.ActivateType(Type, new object[0]);
            }

            return m_Instance;
        }
    }
}