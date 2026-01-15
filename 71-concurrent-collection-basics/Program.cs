// See https://aka.ms/new-console-template for more information
using System.Collections.Concurrent;

Console.WriteLine("Hello, Concurrent Collection basics !!");

var queue = new ConcurrentQueue<int>();

queue.Enqueue(1);
queue.Enqueue(2);
queue.Enqueue(3);

// there is no Dequeue in Concurrent Collection
if (queue.TryDequeue(out var result))
{
    Console.WriteLine($"First value in Queue is {result}");
}

var stack = new ConcurrentStack<int>();
stack.Push(1);
stack.Push(2);
stack.Push(3);

if (stack.TryPop(out var result1))
{
    Console.WriteLine($"First value in Stack is {result1}");
}

Console.Read();

