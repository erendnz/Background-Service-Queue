using System;
using System.Threading.Channels;

namespace BackgroundServiceQueue.Queues
{
    public class BackgroundQueue : IBackgroundQueue<String>
    {
		private readonly Channel<String> future_date_queue;
		private readonly Channel<string> message_queue;

		public BackgroundQueue(IConfiguration configuration)
		{
			int.TryParse(configuration["QueueCapacity"], out int capacity);

			BoundedChannelOptions options = new(capacity)
			{
				FullMode = BoundedChannelFullMode.Wait
			};

			future_date_queue = Channel.CreateBounded<string>(options);
			message_queue = Channel.CreateBounded<string>(options);
		}

		public async ValueTask AddQueueFutureDate(String workItem)
		{
			ArgumentNullException.ThrowIfNull(workItem);

			await future_date_queue.Writer.WriteAsync(workItem);
		}

		public ValueTask<string> DeQueueFutureDate(CancellationToken cancellationToken)
		{
			var workItem = future_date_queue.Reader.ReadAsync(cancellationToken);
			return workItem;
		}

		public async ValueTask AddQueueMessage(String workItem)
		{
			ArgumentNullException.ThrowIfNull(workItem);

			await message_queue.Writer.WriteAsync(workItem);
		}

		public ValueTask<string> DeQueueMessage(CancellationToken cancellationToken)
		{
			var workItem = message_queue.Reader.ReadAsync(cancellationToken);
			return workItem;
		}

        
    }
}

