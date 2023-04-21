using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BackgroundServiceQueue.Queues;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using BackgroundServiceQueue.Models;
using BackgroundServiceQueue.Controllers;
using BackgroundServiceQueue;
using BackgroundServiceQueue.Queues;

namespace BackgroundServiceQueue
{
    public class QueueHostedService : BackgroundService
    {
        private readonly ILogger<QueueHostedService> logger;
        private readonly IBackgroundQueue<string> future_date_queue;
        private readonly IBackgroundQueue<string> message_queue;

        public QueueHostedService(ILogger<QueueHostedService> logger, IBackgroundQueue<string> future_date_queue,IBackgroundQueue<string> message_queue)
        {
            this.logger = logger;
            this.future_date_queue = future_date_queue;
            this.message_queue = message_queue;
        }

        /*protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string sendingFutureDate = await future_date_queue.DeQueueFutureDate(stoppingToken);
                String sendingMessage = await message_queue.DeQueueMessage(stoppingToken);

                await Task.Delay(5000);

                var result = new DbController(future_date_queue,message_queue).AddQueue();

                logger.LogInformation($"Mail: {sendingMessage}" + " " + $"Mail {sendingFutureDate} tarihinde gönderildi.");


            }
            //Kuyruktan çıkarıp konsola basıyor
        }*/

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string sendingFutureDate = await future_date_queue.DeQueueFutureDate(stoppingToken);
                String sendingMessage = await message_queue.DeQueueMessage(stoppingToken);

                var result = new DbController(future_date_queue, message_queue).AddQueue();

                logger.LogInformation($"Mail: {sendingMessage}" + " " + $"Mail {sendingFutureDate} tarihinde gönderildi.");

                await Task.Delay(900000);
            }
            //Kuyruktan çıkarıp konsola basıyor
        }
    }
}
