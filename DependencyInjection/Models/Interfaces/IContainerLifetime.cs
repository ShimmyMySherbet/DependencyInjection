using System.Threading;
using System.Threading.Tasks;

namespace ShimmyMySherbet.DependencyInjection.Models.Interfaces
{
    public interface IContainerLifetime
    {
        CancellationToken Token { get; }

        void Shutdown();

        Task WaitForShutdownAsync();

        void WaitForShutdown();
    }
}