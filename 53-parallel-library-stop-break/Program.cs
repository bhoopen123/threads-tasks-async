// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, Parallel Stop and Break Handling !");


int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Summing using ParallelFor
object lockSum = new object();
int sum = 0;

try
{
    Console.WriteLine("Parallel.For with Stop()");
    Parallel.For(0, array.Length, (i, state) =>
    {
        lock (lockSum)
        {
            // if we want that other threads to also stop when the state is changed to "Stopped" in any of the threads of the Parallel.For
            //if (state.IsStopped)
            //{
            //    return;
            //}

            if (i == 5)
            {
                state.Stop(); // changing the state to Stop().
            }

            sum += array[i];
            Console.WriteLine($"number : {array[i]}, Current task id: {Task.CurrentId}.");
        }
    });
    Console.WriteLine($"Sum using Parallel.For = {sum}");

    sum = 0;
    Console.WriteLine("Parallel.For with Break()");
    Parallel.For(0, array.Length, (i, state) =>
    {
        lock (lockSum)
        {
            if (i == 5)
            {
                state.Break(); // changing the state to Break().
            }

            sum += array[i];
            Console.WriteLine($"number : {array[i]}, Current task id: {Task.CurrentId}.");
        }
    });
}
catch (AggregateException ex)
{

    throw;
}
Console.WriteLine($"Sum using Parallel.For = {sum}");
Console.ReadLine();

