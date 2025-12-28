// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

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
int numberOfThreads = 3;
int segmentSize = nums.Length / numberOfThreads;
Thread[] threads = new Thread[numberOfThreads];
int sum1 = 0, sum2 = 0, sum3 = 0;

threads[0] = new Thread(() => { sum1 = SumSegment(nums, 0, segmentSize); });
threads[1] = new Thread(() => { sum2 = SumSegment(nums, segmentSize, 2 * segmentSize); });
threads[2] = new Thread(() => { sum3 = SumSegment(nums, 2 * segmentSize, nums.Length); });

foreach (var thread in threads)
{
    thread.Start();
    // thread.Join(); // Do not Join it immediately
}

foreach (var thread in threads) { thread.Join(); }

sum = 0;
sum = sum1 + sum2 + sum3;

stopwatch.Stop();
Console.WriteLine($"Sum using multi threads= {sum}, time taken = {stopwatch.ElapsedMilliseconds}");

Console.ReadLine();

int SumSegment(int[] nums, int start, int end)
{
    int segmentSum = 0;
    for (int i = start; i < end; i++)
    {
        segmentSum += nums[i];
        Thread.Sleep(100); // Simulate some work
    }
    return segmentSum;
}