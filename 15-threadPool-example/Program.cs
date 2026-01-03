// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, ThreadPool!!");

ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxIOThreads);

Console.WriteLine($"Max worker threads {maxWorkerThreads}; Max I/O threads {maxIOThreads}");

ThreadPool.GetAvailableThreads(out var availableWorkerThreads, out var availableIOThreads);

Console.WriteLine($"Active worker threads {maxWorkerThreads - availableWorkerThreads}; Active I/O threads {maxIOThreads - availableIOThreads}");

//https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadpool.queueuserworkitem?view=net-10.0&devlangs=csharp&f1url=%3FappId%3DDev17IDEF1%26l%3DEN-US%26k%3Dk(System.Threading.ThreadPool.QueueUserWorkItem)%3Bk(DevLang-csharp)%26rd%3Dtrue

Queue<string?> requestQueue = new();
bool isRunning = true;

// 2. Start Processing Monitor Queue requests
Thread monitorThread = new Thread(MonitorQueue);
monitorThread.Start();

Console.WriteLine("Hello,  Server is runing, Type 'exit' to stop.");
// 1. Enque the input request
while (true)
{
    string? input = Console.ReadLine();
    if (input?.ToLower() == "exit")
    {
        isRunning = false;
        monitorThread.Join();
        break;
    }

    QueueRequest(input);
    //ProcessRequest(input); // Processing input requests on the Main thread will block the subsequent requests until the current request is processed
}

void QueueRequest(string? input)
{
    requestQueue.Enqueue(input);
}

void ProcessRequest(object? input)
{
    Thread.Sleep(2000);
    Console.WriteLine($"Processed request : {input}, IsThreadPoolThread {Thread.CurrentThread.IsThreadPoolThread}");
}

void MonitorQueue()
{
    while (isRunning || requestQueue.Count > 0)
    {
        if (requestQueue.Count > 0)
        {
            string? inputRequest = requestQueue.Dequeue();

            ThreadPool.QueueUserWorkItem(ProcessRequest, inputRequest);
            //Thread processingThread = new Thread(() => ProcessRequest(inputRequest));
            //processingThread.Start();
        }

        Thread.Sleep(100);
    }
}
