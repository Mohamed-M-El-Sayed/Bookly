using System.Linq.Expressions;

namespace Bookly.Application.Contracts.Services.BackgroundJobs
{
    public interface IBackgroundJobService
    {
        void AddOrUpdate<T>(string jobId, Expression<Func<T, Task>> job, string cronExpression);

        void Enqueue<T>(Expression<Func<T, Task>> job);
    }
}
