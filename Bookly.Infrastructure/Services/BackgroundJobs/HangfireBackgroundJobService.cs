using Bookly.Application.Contracts.Services.BackgroundJobs;
using Hangfire;
using Hangfire.Common;
using System.Linq.Expressions;

namespace Bookly.Infrastructure.Services.BackgroundJobs
{
    public class HangfireBackgroundJobService(IRecurringJobManager recurringJobManager,
        IBackgroundJobClient backgroundJobClient) : IBackgroundJobService
    {
        public void AddOrUpdate<T>(string jobId, Expression<Func<T, Task>> job, string cronExpression)
        {
            recurringJobManager.AddOrUpdate(
               jobId,
               Job.FromExpression(job),
               cronExpression);
        }

        public void Enqueue<T>(Expression<Func<T, Task>> job)
        {
            backgroundJobClient.Enqueue(job);
        }
    }
}
