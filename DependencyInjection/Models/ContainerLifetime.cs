using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ShimmyMySherbet.DependencyInjection.Models.Interfaces;

namespace ShimmyMySherbet.DependencyInjection.Models
{
    public class ContainerLifetime : IContainerLifetime, IHostLifetime, IHostApplicationLifetime
    {
        public CancellationToken ApplicationStarted => m_StartedSource.Token;
        public CancellationToken ApplicationStopping => m_StoppingSource.Token;
        public CancellationToken ApplicationStopped => m_StoppedSource.Token;
        public CancellationToken Token => ApplicationStopping;

        private readonly ServiceHost m_Host;
        private readonly CancellationTokenSource m_StartedSource = new();
        private readonly CancellationTokenSource m_StoppingSource = new();
        private readonly CancellationTokenSource m_StoppedSource = new();

        private readonly TaskCompletionSource<ServiceHost> m_StoppingAwaiter = new();
        private readonly TaskCompletionSource<ServiceHost> m_StoppedAwaiter = new();
        private readonly TaskCompletionSource<ServiceHost> m_StartedAwaiter = new();

        public ContainerLifetime(ServiceHost host)
        {
            m_Host = host;
        }

        public void SendStarted()
        {
            m_StartedSource.Cancel();
            m_StartedAwaiter.SetResult(m_Host);
        }

        public void SendShutdownFinished()
        {
            Shutdown();
            m_StoppedSource.Cancel();
            m_StoppedAwaiter.SetResult(m_Host);
        }

        public void Shutdown()
        {
            m_StoppingSource.Cancel();
            m_StoppingAwaiter.SetResult(m_Host);
        }

        public void StopApplication()
        {
            Shutdown();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Shutdown();
            await m_StoppedAwaiter.Task;
        }

        public void WaitForShutdown()
        {
            SpinWait.SpinUntil(() => ApplicationStopping.IsCancellationRequested);
        }

        public async Task WaitForShutdownAsync()
        {
            await m_StoppingAwaiter.Task;
        }

        public async Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(async () => await m_StartedAwaiter.Task, cancellationToken);
        }
    }
}