using System;
using ShimmyMySherbet.DependencyInjection;
using ShimmyMySherbet.DependencyInjection.Models.Interfaces;

namespace ShimmyMySherbet.DependencyInjection.Models.Lifetimes
{
    public class TransientService : IContainerService
    {
        public Type Type { get; }

        private ServiceHost m_Container;

        public TransientService(ServiceHost container, Type type)
        {
            Type = type;
            m_Container = container;
        }

        public object GetInstance()
        {
            return m_Container.ActivateType(Type);
        }
    }
}