// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, Parallel For !!");

int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

Stopwatch stopwatch = Stopwatch.StartNew();
// Summing without Parallel
int sum = 0;
stopwatch.Start();

for (int i = 0; i < array.Length; i++)
    sum += array[i];

stopwatch.Stop();
Console.WriteLine($"Sum = {sum}, time taken = {stopwatch.ElapsedMilliseconds} ms");

// Summing using ParallelFor
object lockSum = new object();
sum = 0;

stopwatch.Restart();

Parallel.For(0, array.Length, i =>
{
    lock (lockSum)
    {
        // now because 'sum' is a shared resources, we need to use lock. because this section is executed parallely by multiple threads
        sum += array[i];
        Console.WriteLine($"Current task id: {Task.CurrentId}.");
    }
});

stopwatch.Stop();
Console.WriteLine($"Sum using Parallel.For = {sum}, time taken = {stopwatch.ElapsedMilliseconds} ms");


// using Parallel.Foreach
sum = 0;
stopwatch.Restart();

Parallel.ForEach(array, item =>
{
    lock (lockSum)
    {
        sum += item;
        Console.WriteLine($"Current task id: {Task.CurrentId}.");
    }
});

stopwatch.Stop();

Console.WriteLine($"Sum using Parallel.Foreach = {sum}, time taken = {stopwatch.ElapsedMilliseconds} ms");


// Parallel.Invoke example
Parallel.Invoke(
() =>
{
    Console.WriteLine($"Task A running by TaskId:{Task.CurrentId}");
},
() =>
{
    Console.WriteLine($"Task B running by TaskId:{Task.CurrentId}");
},
() =>
{
    Console.WriteLine($"Task C running by TaskId:{Task.CurrentId}");
});

Console.ReadLine();