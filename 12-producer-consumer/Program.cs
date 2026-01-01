// See https://aka.ms/new-console-template for more information
using System.Collections.Concurrent;

Console.WriteLine("Hello, Consumer Producer using Manual Reset Event!");

ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
ManualResetEventSlim consumeEvent = new ManualResetEventSlim(false);

// initial state should be true
ManualResetEventSlim produceEvent = new ManualResetEventSlim(true);

object eventLock = new object();

for (int i = 0; i < 3; i++)
{
    Thread thread = new Thread(Consume);
    thread.Name = $"Thread {i + 1}";
    thread.Start();
}

// Producer
while (true)
{
    produceEvent.Wait();
    produceEvent.Reset(); // After Reset, the Producer will wait for signal to Produce again

    Console.WriteLine("To produce, enter 'p'");

    var input = Console.ReadLine() ?? "";

    if (input.ToLower() == "p")
    {
        for (int i = 1; i <= 10; i++)
        {
            queue.Enqueue(i);
            Console.WriteLine($"Produced: {i}");
        }
        consumeEvent.Set();
    }
}

// Consumer's behavior
void Consume()
{
    while (true)
    {
        consumeEvent.Wait();

        while (queue.TryDequeue(out int item))
        {
            // work on the items produced

            Thread.Sleep(500);
            Console.WriteLine($"Consumed: {item} from thread: {Thread.CurrentThread.Name}");
        }

        lock (eventLock)
        {
            if (queue.IsEmpty && consumeEvent.IsSet)
            {
                consumeEvent.Reset();   // Signal Consumer to Reset, that means wait for the Consume Signal
                produceEvent.Set();     // Signal Producer to produce more

                Console.WriteLine("******************");
                Console.WriteLine("****** More Please ******");
                Console.WriteLine("******************");
            }
        }
    }
}
