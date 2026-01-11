// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, Parallel Exception Handling !");

int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Summing using ParallelFor
object lockSum = new object();
int sum = 0;

try
{
    Parallel.For(0, array.Length, (i, state) =>
    {
        lock (lockSum)
        {
            // if we want that other threads to also stop when there is an exception in any of the threads of the Parallel.For
            if (state.IsExceptional)
            {
                return;
            }

            if (i == 5)
                throw new InvalidOperationException("this is on purpose.");
            sum += array[i];
            Console.WriteLine($"Current task id: {Task.CurrentId}.");
        }
    });
}
catch (AggregateException ex)
{

    throw;
}
Console.WriteLine($"Sum using Parallel.For = {sum}");
Console.ReadLine();
