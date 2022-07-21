using System;
using ShimmyMySherbet.DependencyInjection.Models.Interfaces;

namespace ShimmyMySherbet.DependencyInjection.Models.Lifetimes
{
    /// <summary>
    /// Used as a temporary singleton service
    /// </summary>
    public class ScopeService : IContainerService
    {
        public object Instance { get; }
        public Type Type { get; }

        public ScopeService(object instance)
        {
            Instance = instance;
            Type = instance.GetType();
        }

        public object GetInstance()
        {
            return Instance;
        }
    }
}