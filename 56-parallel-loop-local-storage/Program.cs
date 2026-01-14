// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

int[] array = Enumerable.Range(1, 1000000).ToArray();

int sum = 0;

object lockSum = new object();

Parallel.For(
    0, array.Length,
    () => 0,    // initialized the Thread Local variable
    (i, state, threadLocalStorage) =>   // using thread loal variable by parameter name 'threadLocalStorage'
    {
        threadLocalStorage += array[i];
        return threadLocalStorage;
    },
    localFinally: lf => // aggregate that using shared variable 
    {
        lock (lockSum)
        {
            sum += lf;
            Console.WriteLine($"The task id : {Task.CurrentId}");
        }
    });

Console.WriteLine($"The sum is {sum}");

Console.ReadLine();
