using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShimmyMySherbet.DependencyInjection.Models.Exceptions
{
    public class CannotInvokeException : Exception
    {
        public CannotInvokeException(MethodInfo method, ParameterInfo parameter) : base($"Cannot invoke method {method.Name}, cannot provide parameter '{parameter.ParameterType.Name} {parameter.Name}'")
        {
        }
    }
}
