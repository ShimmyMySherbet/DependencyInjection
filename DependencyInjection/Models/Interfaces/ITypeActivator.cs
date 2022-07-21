using System;

namespace ShimmyMySherbet.DependencyInjection.Models.Interfaces
{
    public interface ITypeActivator
    {
        T ActivateType<T>(IContainerServiceCollection services, params object[] extras);

        object ActivateType(IContainerServiceCollection services, Type type);

        object ActivateType(IContainerServiceCollection containerServices, Type type, object[] extras);
    }
}