// See https://aka.ms/new-console-template for more information
using System.Text.Json;

Console.WriteLine("Hello, Task ContinueWith !!");

using var client = new HttpClient();
// creating a task to get result from the API
var task = client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");
// instead of using task.Result which is a blocking statement
// lets use ContinueWith to do something when the task is completed

var continuationTask = task.ContinueWith(static t =>
{
    var result = t.Result; // using task.Result inside ContinueWith which will start when task is completed
    var jsonDoc = JsonDocument.Parse(result);
    JsonElement root = jsonDoc.RootElement;
    JsonElement jsonResults = root.GetProperty("results");
    JsonElement firstPokemon = jsonResults[0];

    Console.WriteLine($"First pokemon name: {firstPokemon.GetProperty("name")}");
    Console.WriteLine($"First pokemon url: {firstPokemon.GetProperty("url")}");
});

Console.WriteLine("This is the end of the program"); // this print statement is to ensure that the main thread is NOT blocked

Console.ReadLine();