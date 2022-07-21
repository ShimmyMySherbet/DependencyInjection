using System;

namespace ShimmyMySherbet.DependencyInjection.Models.Interfaces
{
    public interface IContainerService
    {
        Type Type { get; }

        object GetInstance();
    }
}