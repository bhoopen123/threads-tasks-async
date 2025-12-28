// See https://aka.ms/new-console-template for more information

Queue<string?> requestQueue = new Queue<string?>();
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

void ProcessRequest(string? input)
{
    Thread.Sleep(2000);
    Console.WriteLine($"Processed request : {input}");
}

void MonitorQueue()
{
    while (isRunning || requestQueue.Count > 0)
    {
        if (requestQueue.Count > 0)
        {
            string? inputRequest = requestQueue.Dequeue();
            Thread processingThread = new Thread(() => ProcessRequest(inputRequest));
            processingThread.Start();
        }

        Thread.Sleep(100);
    }
}