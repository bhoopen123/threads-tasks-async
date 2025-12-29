// See https://aka.ms/new-console-template for more information

Queue<string?> requestQueue = new Queue<string?>();
bool isRunning = true;
int availableTickets = 10;
object lockObj = new object();

// 2. Start Processing Monitor Queue requests
Thread monitorThread = new Thread(MonitorQueue);
monitorThread.Start();

Console.WriteLine("Hello,  Server is runing, " +
    "\r\n Type 'b' to book a ticket. " +
    "\r\n Type 'c' to cancel a booked a ticket. " +
    "\r\n Type 'exit' to stop. " +
    "\r\n");

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

// Process the request
void ProcessRequest(string? input)
{
    Thread.Sleep(2000);

    lock (lockObj)
    {
        Console.WriteLine($"Processing request : {input}");

        if (input == "b")
        {
            if (availableTickets > 0)
            {
                availableTickets--;
                Console.WriteLine($"\r Your seat is booked. {availableTickets} seats are still available.");
            }
            else
            {
                Console.WriteLine($"\r Tickets are not available.");
            }
        }
        else if (input == "c")
        {
            if (availableTickets < 10)
            {
                availableTickets++;
                Console.WriteLine($"\r Your booking is canceled. {availableTickets} seats are available.");
            }
            else
            {
                Console.WriteLine($"\r Error, can't cancel the ticket.");
            }
        }
        else
        {
            Console.WriteLine($"\r Error, invalid input: {input}.");
        }
    }
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