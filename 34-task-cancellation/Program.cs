// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, Task Cancellation !!");

// always diapose CancellationTokenSource after usages
using var cts = new CancellationTokenSource();
var token = cts.Token;

var task = Task.Run(Work, token);

Console.WriteLine("To cancel, press 'c'");
var input = Console.ReadLine();
if (input == "c")
{
    cts.Cancel();
}


task.Wait();
Console.WriteLine($"Task status is: {task.Status}");
Console.ReadLine();

void Work()
{
    Console.WriteLine("Started doing the work.");

    for (int i = 0; i < 100000; i++)
    {
        Console.WriteLine($"{DateTime.Now}");

        // Option 1: this will check if the cancellation is requested and throw exception
        token.ThrowIfCancellationRequested();

        // Option 2: 
        // check `token.IsCancellationRequested`
        //if (token.IsCancellationRequested)
        //{
        //    Console.WriteLine($"User requested cancellation at iteration: {i}");

        //    // Option 2-1: use break; to exit the condition. 
        //    //break; 

        //    // Option 2-2: throw OperationCanceledException(); to exit the condition. 
        //    //throw new OperationCanceledException();
        //}

        Thread.SpinWait(30000000);
    }

    Console.WriteLine("Work is done.");
}

#region cts.CancelAfter() example

//using CancellationTokenSource source = new CancellationTokenSource();
//source.CancelAfter(10);

//using var client = new HttpClient();
//var taskPokemonListJson = client.GetStringAsync("https://pokeapi.co/api/v2/pokemon", source.Token);

//Console.WriteLine(taskPokemonListJson.Result);

#endregion