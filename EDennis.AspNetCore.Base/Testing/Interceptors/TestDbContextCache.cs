using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Testing {

    public class TestDbContextCache<TContext> : Dictionary<string,TContext>
        where TContext : DbContext {

        public const int QUEUE_LIMIT = 100;
        public const int THREAD_COUNT = 10;
        private int DequeueCount = 0;


        private BlockingCollection<TContext> Queue { get; set; }
            = new BlockingCollection<TContext>();


        public TestDbContextCache() {
        }

        public TContext CreateInMemoryDatabase(string instanceName) {
            Interlocked.Increment(ref DequeueCount);
            TContext context = null;

            //only use the queue if more than one instance of a TContext is needed
            if (DequeueCount > 1) {
                Queue.TryTake(out context,10);
                if (context == null) {
                    FillQueue();
                    context = Queue.Take();
                }
            } else {
                context = CreateInMemoryDatabase().Result;
            }
            return context;            
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


        public static void DropInMemoryDatabase(TContext context) {
            context.Database.EnsureDeleted();
        }


        public static TContext GetReadonlyDatabase(IConfiguration config) {

            var contextName = typeof(TContext).Name;

            var cxnString = config[$"ConnectionStrings:{contextName}"];

            var options = new DbContextOptionsBuilder<TContext>()
                .UseSqlServer(cxnString)
                .Options;

            //using reflection, instantiate the DbContextBase subclass
            var context = Activator.CreateInstance(typeof(TContext),
                new object[] { options }) as TContext;

            return context;
        }


    }
}
