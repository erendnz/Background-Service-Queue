using System;

namespace BackgroundServiceQueue.Queues
{

}

 public interface IBackgroundQueue<T>
 {
	ValueTask AddQueueFutureDate(T workItem);
	ValueTask<string> DeQueueFutureDate(CancellationToken cancellationToken);

	ValueTask AddQueueMessage(T workItem);
	ValueTask<string> DeQueueMessage(CancellationToken cancellationToken);
}


