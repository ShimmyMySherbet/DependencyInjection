using System.Threading;
using System.Threading.Tasks;
using ShimmyMySherbet.DependencyInjection.Models.Interfaces;

namespace ShimmyMySherbet.DependencyInjection.Models
{
    public class ContainerLifetime : IContainerLifetime
    {
        private CancellationTokenSource m_Source = new CancellationTokenSource();
        public CancellationToken Token => m_Source.Token;

        private TaskCompletionSource<ContainerLifetime> m_Awaiter = new TaskCompletionSource<ContainerLifetime>();

        public async Task WaitForShutdownAsync()
        {
            await m_Awaiter.Task;
        }

        public void WaitForShutdown()
        {
            SpinWait.SpinUntil(() => Token.IsCancellationRequested);
        }

        public void Shutdown()
        {
            m_Awaiter.SetResult(this);
            m_Source.Cancel();
        }
    }
}