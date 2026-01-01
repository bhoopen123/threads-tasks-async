// See https://aka.ms/new-console-template for more information

AutoResetEvent autoResetEvent = new AutoResetEvent(false);

// worker threads

for (int i = 0; i < 3; i++)
{
    Thread worker = new Thread(DoWork);
    worker.Name = $"Worker{i + 1}";
    worker.Start();
}
Console.WriteLine("Write 'go' to proceed.");

while (true)
{
    string userInput = Console.ReadLine() ?? "";

    // here the main thread is Producer
    // which send a signal for the workers thread(s) which are consumers to do some work
    if (userInput.Trim().ToLower().Equals("go"))
    {
        autoResetEvent.Set(); // Signal the worker thread to start
    }
}

void DoWork()
{
    while (true) // because we want threads to be ready after work is done, to pick the new work
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        string? threadName = Thread.CurrentThread.Name;

        Console.WriteLine($"Worker thread {threadId}:{threadName} is waiting for the Signal");

        autoResetEvent.WaitOne();

        Console.WriteLine($"Worker thread {threadId}:{threadName} preceeds.");

        Thread.Sleep(3000);
    }
}