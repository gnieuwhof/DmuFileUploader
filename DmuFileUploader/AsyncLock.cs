namespace DmuFileUploader
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AsyncLock : IDisposable
    {
        private readonly SemaphoreSlim semaphore;


        private AsyncLock(SemaphoreSlim semaphore)
        {
            this.semaphore = semaphore;
        }


        public static SemaphoreSlim CreateSemaphore()
        {
            var semaphore = new SemaphoreSlim(1, 1);

            return semaphore;
        }

        public static async Task<AsyncLock> Enter(SemaphoreSlim semaphore)
        {
            _ = semaphore ??
                throw new ArgumentNullException(nameof(semaphore));

            await semaphore.WaitAsync();

            var asyncLock = new AsyncLock(semaphore);

            return asyncLock;
        }

        public void Dispose()
        {
            this.semaphore.Release();
        }
    }
}
