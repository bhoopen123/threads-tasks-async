// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, Parallel Linq basics !");

#region AsParallel()

//// when using a very big list
//var veryLongList = Enumerable.Range(1, 2000000000);

//// now the items is a very very long list
//// and takes time to process
//// this can optimized using ParallelLinq
////var evenNumbers = veryLongList.Where((i) =>
////{
////    //Console.WriteLine($"Precessing number {i}");

////    return i % 2 == 0;
////});

//// using AsParallel()
//var evenNumbers = veryLongList.AsParallel().Where(i =>
//{
//    return i % 2 == 0;
//});

//Console.WriteLine($"There are {evenNumbers.Count()} even numbers in the collections.");

//// printing the even numbers
//foreach (var evenNumber in evenNumbers)
//{
//    Console.WriteLine($"Even number {evenNumber}, thread id {Thread.CurrentThread.ManagedThreadId}");
//}

//Console.ReadLine();

#endregion


#region AsParallel(), ForAll()

var items = Enumerable.Range(1, 20);

var evenNums = items.AsParallel().Where(i =>
{
    Console.WriteLine($"Processing Number {i}, Thread Id {Thread.CurrentThread.ManagedThreadId}");
    return i % 2 == 0;
});

// printing the even numbers asynchronously
evenNums.ForAll(i =>
{
    Console.WriteLine($"Even number {i}");
});

Console.ReadLine();

#endregion

