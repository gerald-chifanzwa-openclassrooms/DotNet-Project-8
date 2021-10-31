using RedLockNet.SERedis;

namespace CalendarApi;

public interface IConcurrencyManager
{
    Task<T> Execute<T>(Func<Task<T>> action);
}

public class ConcurrencyManager : IConcurrencyManager
{
    private readonly RedLockFactory _lockFactory;

    public ConcurrencyManager(RedLockFactory redLockFactory)
    {
        _lockFactory = redLockFactory;
    }

    public async Task<T> Execute<T>(Func<Task<T>> action)
    {
        var resource = "blocking-action";
        var expiry = TimeSpan.FromSeconds(30);
        var wait = TimeSpan.FromSeconds(10);
        var retry = TimeSpan.FromSeconds(1);

        // blocks until acquired or 'wait' timeout
        await using (var redLock = await _lockFactory.CreateLockAsync(resource, expiry, wait, retry))
        {
            return redLock.IsAcquired ? await action() : throw new ConcurrencyException("Failed to obtain lock");
        }
        // the lock is automatically released at the end of the using block



    }
}