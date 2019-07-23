using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RecipeJungle.Contexts;
using System.Linq;

public class AutoVerifyService : IHostedService, IDisposable {
        private readonly IServiceProvider services;
        private Timer timer;
        private static TimeSpan treashold = TimeSpan.FromDays(4);

        public AutoVerifyService(IServiceProvider services) {
            this.services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken) {
            timer = new Timer(DoWork, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose() {
            timer.Dispose();
        }

        private void DoWork(object state) {
            var now = DateTime.UtcNow;
            using (var scope = services.CreateScope()) {
                var mainContext = scope.ServiceProvider.GetRequiredService<RecipeContext>();
                var items = mainContext.Recipes.ToList();
                foreach (var item in items) {
                    var delta = now - item.CreatedTime;
                    if (delta >= treashold) {
                        mainContext.Remove(item);
                    }
                }
                mainContext.SaveChanges();
            }
        }
    }
