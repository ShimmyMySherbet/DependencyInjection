using System;

namespace ShimmyMySherbet.DependencyInjection.Models.Exceptions
{
    public class FailedToActivateException : Exception
    {
        public FailedToActivateException(Type type) : base($"Failed to activate type '{type.FullName}'")
        {
        }
    }
}