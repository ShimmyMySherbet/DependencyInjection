using System;
using System.Reflection;

namespace ShimmyMySherbet.DependencyInjection.Models.Interfaces
{
    public interface IDynamicInvoker
    {
        object? Invoke(MethodInfo info, object? instance);

        T? Invoke<T>(MethodInfo info, object? instance);

        object? Invoke(Delegate @delegate);
        T? Invoke<T>(Delegate @delegate);
    }
}