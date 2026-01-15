// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, PLINQ Cancellation !");

var items = Enumerable.Range(1, 200);

using var cts = new CancellationTokenSource();

var evenNumbers = items.AsParallel()
    .WithCancellation(cts.Token)
    .Where(i =>
    {
        //Console.WriteLine($"Processing number {i} by Thread Id :    {Thread.CurrentThread.ManagedThreadId}");

        return i % 2 == 0;
    });


try
{
    evenNumbers.ForAll(even =>
    {
        if (even > 5) cts.Cancel();

        Console.WriteLine($"Even Number {even} by Thread {Thread.CurrentThread.ManagedThreadId}");
    });
}
catch (OperationCanceledException ex)
{
    Console.WriteLine("Query is cancelled...");
}
catch (AggregateException ex)
{
    Console.WriteLine("Error during Processing Number");

    ex.Handle(err =>
    {
        Console.WriteLine(err.Message);
        return true;
    });
}
Console.ReadLine();