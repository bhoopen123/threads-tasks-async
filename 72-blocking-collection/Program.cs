// See https://aka.ms/new-console-template for more information
using System.Collections.Concurrent;

Console.WriteLine("Hello, Blocking collection !!");

// Blocking Collection provides 2 features
// 1. Blocking means that when the maximun capacity is reached, the producer should stop producing and wait until the consumer consumes the products in the buffer and there is space in Buffer to store more items/products.
// lower bound : when the items in the buffer reaches lower bound value then consumer should be blocked.
// upper bound : when the items in the buffer reaches more than upper bound value then producer should be blocked.

// 2. Bounding means that there is a maximum capacity in the buffer which can be specified.

// The BlockingCollection, ConcurrentQueue, ConcurrentStack, ConcurrentBack are specific to Producer and Consumer pattern.

ConcurrentQueue<string?> requestQueue = new ConcurrentQueue<string?>();

BlockingCollection<string?> collection = new(requestQueue, boundedCapacity: 3);

Thread moniterThread = new Thread(MonitorQueue);
moniterThread.Start();

Console.WriteLine($"Server is running. Type 'exit' to stop.");
while (true)
{
    string? input = Console.ReadLine();

    if (input?.ToLower() == "exit")
    {
        //mark blocking collection as complete to not accept any new values
        collection.CompleteAdding();
        break;
    }

    collection.Add(input);

    Console.WriteLine($"Enqueued {input}, queue size {requestQueue.Count}");
}

void MonitorQueue()
{
    // this foreach will run for infinite times, until exited intentionally
    foreach (var request in collection.GetConsumingEnumerable())
    {
        // check if the BlockingCollection is marked as Completed
        if (collection.IsAddingCompleted)
        {
            Console.WriteLine("Exit requested by the user.");
            break;
        }

        Thread processingThread = new Thread(() => ProcessRequest(request));
        processingThread.Start();

        Thread.Sleep(1000);
    }
}

void ProcessRequest(string? input)
{
    Console.WriteLine($"Processing Request {input}");
    Thread.Sleep(2000);
}