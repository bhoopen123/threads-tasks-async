// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, Parallel loop result !!");

Console.WriteLine("When doing Break !!");
ParallelLoopResult result = Parallel.For(0, 20, (i, state) =>
{
    Console.WriteLine($"Iteration {i}");

    if (i == 10)
    {
        Console.WriteLine("Break called at iteration " + i);
        state.Break();
    }
});

Console.WriteLine($"IsCompleted: {result.IsCompleted}");
Console.WriteLine($"LowestBreakIteration: {result.LowestBreakIteration}");

Console.WriteLine();

Console.WriteLine("When doing Stop !!");

ParallelLoopResult result1 = Parallel.For(0, 20, (i, state) =>
{
    Console.WriteLine($"Iteration {i}");

    if (i == 10)
    {
        Console.WriteLine("Stop called at iteration " + i);
        state.Stop();
    }
});

Console.WriteLine($"IsCompleted: {result1.IsCompleted}");
Console.WriteLine($"LowestBreakIteration: {result1.LowestBreakIteration}");

Console.ReadLine();