using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Testing {
    public class InMemoryDatabaseQueue<TContext>
        where TContext : DbContext {


        public const int QUEUE_LIMIT = 100;
        public const int THREAD_COUNT = 10;
        private int DequeueCount = 0;

        public InMemoryDatabaseQueue() {
        }

        private BlockingCollection<TContext> Queue { get; set; }


        public TContext Dequeue() {
            if (DequeueCount > 1)
                FillQueue();
            Interlocked.Increment(ref DequeueCount);
            return Queue.Take();
        }

        private void FillQueue() {
            for (int i = 0; i < THREAD_COUNT; i++) {
                Task.Run(async () => {
                    await EnQueue();
                });
            }
        }

        private async Task EnQueue() {
            while (Queue.Count < QUEUE_LIMIT) {
                var context = await CreateInMemoryDatabase();
                Queue.Add(context);
            }
        }

        private async static Task<TContext> CreateInMemoryDatabase() {
            var options = new DbContextOptionsBuilder<TContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            //using reflection, instantiate the DbContext subclass
            var dbContext = Activator.CreateInstance(typeof(TContext),
                new object[] { options }) as TContext;

            await dbContext.Database.EnsureCreatedAsync();
            return dbContext;
        }


    }
}
