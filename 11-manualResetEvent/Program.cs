// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, Manual Reset Event!");

using ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);

Console.WriteLine("Press enter to release all threads...");

for (int i = 0; i < 3; i++)
{
    Thread thread = new Thread(DoWork);
    thread.Name = $"Thread {i}";
    thread.Start();
}

Console.ReadLine();

manualResetEvent.Set();

Console.ReadLine();

void DoWork()
{
    Console.WriteLine($"{Thread.CurrentThread.Name} is waiting for the signal..");

    manualResetEvent.Wait();
    Thread.Sleep(1000);

    Console.WriteLine($"{Thread.CurrentThread.Name} has been released.");
}
