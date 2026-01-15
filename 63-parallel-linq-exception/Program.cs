// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, PLINQ exception !!");

var items = Enumerable.Range(1, 20);

var evenNumbers = items.AsParallel().Where(i =>
{
    Console.WriteLine($"Precessing number {i} by Thread Id : {Thread.CurrentThread.ManagedThreadId}");

    if (i == 5) throw new InvalidDataException("This is intentional error at 5");

    if (i == 10) throw new InvalidOperationException("This is intentional error at 10");

    return i % 2 == 0;
});

try
{
    evenNumbers.ForAll(even =>
    {
        Console.WriteLine($"Even Number {even} by Thread {Thread.CurrentThread.ManagedThreadId}");
    });
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
