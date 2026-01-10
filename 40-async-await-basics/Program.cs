// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, async-await !");

Console.WriteLine("Starting to do work.");
await WorkAsync();
Console.WriteLine("Fetching data...");
var data = await FetchDataAsync();
Console.WriteLine($"Fetched : {data}");

Console.WriteLine("Press enter to exit.");
Console.ReadLine();

static async Task WorkAsync()
{
    await Task.Delay(2000);

    Console.WriteLine("Work is done.");
}

static async Task<string> FetchDataAsync()
{
    await Task.Delay(2000);

    return "My complex data";
}