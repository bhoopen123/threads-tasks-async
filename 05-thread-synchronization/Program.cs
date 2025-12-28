// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, Thread Synchronization !");
object lockObject = new object();

// Lock is intorduced in C# 13 and .NET 9 
System.Threading.Lock lockObj = new Lock();

int counter = 0;

Thread thread1 = new Thread(IncrementCounter);
thread1.Start();

// thread1.Join(); // this will ask the main thread to wait until the thread1 is completed, hence the result will be expected even after using more than one threads.

Thread thread2 = new Thread(IncrementCounter);

thread2.Start();

thread1.Join();
thread2.Join();

Console.WriteLine($"Final counter value is: {counter}");

void IncrementCounter()
{
    for (int i = 0; i < 1000000; i++)
    {
        lock (lockObj) // of lock (lockObject) is fine for less than C# 13 or .NET 9 
        {
            counter++;
        }
    }
}