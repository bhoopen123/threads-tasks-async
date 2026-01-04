using System.Text.Json;

using var client = new HttpClient();

// first task to get Pokemon list
var taskPokemonListJson = client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");

// Continuation task to get First Pokemon url
var taskGetFirstPokemonUrl = taskPokemonListJson.ContinueWith(t =>
{
    var result = t.Result;
    var doc = JsonDocument.Parse(result);
    JsonElement root = doc.RootElement;
    JsonElement results = root.GetProperty("results");
    JsonElement firstPokemon = results[0];
    return firstPokemon.GetProperty("url").ToString();
});

// further Continuation task to get Pokemon details
var taskGetFirstPokemonDetailsJson = taskGetFirstPokemonUrl.ContinueWith(t =>
{
    var result = t.Result;
    return client.GetStringAsync(result);
}).Unwrap();    // taskGetFirstPokemonUrl.ContinueWith returns Task<Task<>>, so need to use Unwrap() function to get Task<>

_ = taskGetFirstPokemonDetailsJson.ContinueWith(t =>
{
    var result = t.Result;
    var doc = JsonDocument.Parse(result);
    JsonElement root = doc.RootElement;
    Console.WriteLine($"Name: {root.GetProperty("name").ToString()}");
    Console.WriteLine($"Weight: {root.GetProperty("weight").ToString()}");
    Console.WriteLine($"Height: {root.GetProperty("height")}");
});

Console.WriteLine("This is the end of the program.");

Console.ReadLine();
