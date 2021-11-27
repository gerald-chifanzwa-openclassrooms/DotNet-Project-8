namespace CalendarApi;

public interface IConcurrencyManager
{
    Task<T> Execute<T>(string taskId, Func<Task<T>> action);
}
