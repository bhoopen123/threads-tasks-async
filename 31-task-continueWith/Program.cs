// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
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

// Summation of all tasks as a separate task (Using "ContinueWith" and WhenAll)
int[] nums = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

Stopwatch stopwatch = Stopwatch.StartNew();
stopwatch.Start();

int numberOfTasks = 3;
Task<int>[] computeTasks = new Task<int>[numberOfTasks];
int segmentSize = nums.Length / numberOfTasks;

computeTasks[0] = Task.Run(() => { return SumSegment(nums, 0, segmentSize); });
computeTasks[1] = Task.Run(() => { return SumSegment(nums, segmentSize, 2 * segmentSize); });
computeTasks[2] = Task.Run(() => { return SumSegment(nums, 2 * segmentSize, nums.Length); });

_ = Task.WhenAll(computeTasks).ContinueWith(t =>
{
    stopwatch.Stop();

    Console.WriteLine($"The sum is {t.Result.Sum()}, time taken = {stopwatch.ElapsedMilliseconds}");
});

Console.WriteLine("End of the Summation");

int SumSegment(int[] nums, int start, int end)
{
    int segmentSum = 0;
    for (int i = start; i < end; i++)
    {
        segmentSum += nums[i];
        Task.Delay(100).Wait(); // Simulate some work
    }
    return segmentSum;
}

Console.ReadLine();