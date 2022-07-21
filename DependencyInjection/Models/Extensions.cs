using System;
using Microsoft.Extensions.Logging;

namespace ShimmyMySherbet.DependencyInjection.Models
{
    public static class Extensions
    {
        public static bool IsTypeLogger(this Type t)
        {
            return t.GetGenericTypeDefinition() == typeof(ILogger<>);
        }
    }
}