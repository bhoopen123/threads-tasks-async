// See https://aka.ms/new-console-template for more information

Queue<string?> requestQueue = new Queue<string?>();
bool isRunning = true;

SemaphoreSlim semaphore = new SemaphoreSlim(initialCount: 3, maxCount: 3); // means 3 threads can run parellel
object queueLock = new object();

// 2. Start Processing Monitor Queue requests
Task monitorTask = new Task(MonitorQueue);
monitorTask.Start();

Console.WriteLine("Hello,  Server is runing, Type 'exit' to stop.");
// 1. Enque the input request
while (true)
{
    string? input = Console.ReadLine();
    if (input?.ToLower() == "exit")
    {
        isRunning = false;
        monitorTask.Wait();
        break;
    }

    QueueRequest(input);
    //ProcessRequest(input); // Processing input requests on the Main thread will block the subsequent requests until the current request is processed
}

void QueueRequest(string? input)
{
    lock (queueLock)
    {
        requestQueue.Enqueue(input);
    }
}

void MonitorQueue()
{
    while (isRunning || requestQueue.Count > 0)
    {
        if (requestQueue.Count > 0)
        {
            string? inputRequest = null;

            lock (queueLock)
            {
                inputRequest = requestQueue.Dequeue();
            }

            semaphore.Wait(); // instead of creating a new task everytime, we use semaphore to ensure that only 3 Tasks can be created parallely. 
            // once, any of the previous task is completed 
            // we will release the semaphore, so a new task can be created
            Task processingTask = new Task(() => ProcessRequest(inputRequest));
            processingTask.Start();
        }

        Thread.Sleep(100);
    }
}

void ProcessRequest(string? input)
{
    try
    {
        Task.Delay(2000).Wait();
        Console.WriteLine($"Processed request : {input}");
    }
    finally
    {
        int previousCount = semaphore.Release();
        Console.WriteLine($"Task: {Task.CurrentId} released the semaphore. Previous count is {previousCount}.");
    }
}
