// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, async-await !");

Console.WriteLine($"Starting to do work on thread {Thread.CurrentThread.ManagedThreadId}");

await WorkAsync();
Console.WriteLine($"Fetching data on thread {Thread.CurrentThread.ManagedThreadId}");

var data = await FetchDataAsync();
Console.WriteLine($"Fetched : {data} on thread {Thread.CurrentThread.ManagedThreadId}");

Console.WriteLine("Press enter to exit.");
Console.ReadLine();

static async Task WorkAsync()
{
    Console.WriteLine($"Inside WorkAsync {Thread.CurrentThread.ManagedThreadId}");

    await Task.Delay(2000);

    Console.WriteLine($"WorkAsync completed {Thread.CurrentThread.ManagedThreadId}");

    Console.WriteLine($"Work is done {Thread.CurrentThread.ManagedThreadId}.");
}

static async Task<string> FetchDataAsync()
{
    Console.WriteLine($"Inside FetchDataAsync {Thread.CurrentThread.ManagedThreadId}");

    await Task.Delay(2000);

    Console.WriteLine($"FetchDataAsync completed {Thread.CurrentThread.ManagedThreadId}");

    return "My complex data";
}