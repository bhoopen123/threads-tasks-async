// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, AsParallel() with buffer !!");

var items = Enumerable.Range(1, 200);

var evenNumbers = items.AsParallel()
                 .WithMergeOptions(ParallelMergeOptions.NotBuffered)
                 .Where(i =>
                 {
                     // `where` is running in Parallel
                     Console.WriteLine($"Processing number {i} on Thread {Thread.CurrentThread.ManagedThreadId}");

                     return i % 2 == 0;
                 });

// use `foreach` to see the actual behaviour of buffer
//foreach (var item in evenNumbers)
//{
//    Console.WriteLine($"Number {item} : Thread {Thread.CurrentThread.ManagedThreadId}");
//}

// `ForAll` starts immediately when there is a item in the buffer and 
// runs in parallel
evenNumbers.ForAll(i =>
{
    Console.WriteLine($"Number {i} : Thread {Thread.CurrentThread.ManagedThreadId}");
});

Console.ReadLine();
