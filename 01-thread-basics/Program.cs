// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, Threading...!");

Thread thread1 = new Thread(PrintThreadId);
Thread thread2 = new Thread(PrintThreadId);

thread1.Priority = ThreadPriority.Highest;
thread1.Name = "Thread1";

thread2.Priority = ThreadPriority.Lowest;
thread2.Name = "Thread2";

Thread.CurrentThread.Priority = ThreadPriority.Normal;
Thread.CurrentThread.Name = "MainThread";

thread1.Start();
thread2.Start();

// runing in main thread
PrintThreadId();

Console.ReadLine();

void PrintThreadId()
{
    for (int i = 0; i < 50; i++)
    {
        Console.WriteLine($"Thread ID: {Thread.CurrentThread.ManagedThreadId}, Name: {Thread.CurrentThread.Name}");
        Thread.Sleep(50);
    }
}