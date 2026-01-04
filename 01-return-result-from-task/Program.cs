// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, Tasks in C# !");

// Creating a new Task
Task task = new Task(DoWork);

// start a task
task.Start();

Console.WriteLine("Press any key to continue..");
Console.ReadLine();

void DoWork()
{
    Console.WriteLine("Task Started !!");

    Task.Delay(1000).Wait();

    Console.WriteLine("Task Completed !!");
}

#region Divide and Conquer using Tasks

int[] nums = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

int sum = 0;

Stopwatch stopwatch = Stopwatch.StartNew();
stopwatch.Start();
foreach (var num in nums)
{
    sum += num;
    Thread.Sleep(100); // Simulate some work
}

stopwatch.Stop();
Console.WriteLine($"Sum using single thread= {sum}, time taken: {stopwatch.ElapsedMilliseconds}");

stopwatch.Restart();

int numberOfTasks = 3;
Task<int>[] computeTasks = new Task<int>[numberOfTasks];
int segmentSize = nums.Length / numberOfTasks;

computeTasks[0] = Task.Run(() => { return SumSegment(nums, 0, segmentSize); });
computeTasks[1] = Task.Run(() => { return SumSegment(nums, segmentSize, 2 * segmentSize); });
computeTasks[2] = Task.Run(() => { return SumSegment(nums, 2 * segmentSize, nums.Length); });

sum = 0;
foreach (var computedTask in computeTasks)
{
    sum += computedTask.Result;
}
stopwatch.Stop();

Console.WriteLine($"Sum using multi Tasks = {sum}, time taken = {stopwatch.ElapsedMilliseconds}");

Console.ReadLine();

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

#endregion