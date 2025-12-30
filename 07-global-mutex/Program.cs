
string fileName = "counter.txt";

// normal `lock` will not work, when thread synchronization is needed amoung different processes.
//object normalLock = new object();
//for (int i = 0; i < 10000; i++)
//{
//    lock (normalLock)
//    {
//        int counter = ReadCounter(fileName);
//        counter++;
//        WriteCounter(fileName, counter);
//    }
//}

using (var mutex = new Mutex(false, $"GlobalMutex:{fileName}"))
{
    for (global::System.Int32 i = 0; i < 10000; i++)
    {
        try
        {
            mutex.WaitOne();
            int counter = ReadCounter(fileName);
            counter++;
            WriteCounter(fileName, counter);
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }
}

Console.WriteLine("Process finished.");
Console.ReadLine();

int ReadCounter(string fileName)
{
    using (var stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
    using (var reader = new StreamReader(stream))
    {
        string content = reader.ReadToEnd();
        return string.IsNullOrEmpty(content) ? 0 : int.Parse(content);
    }
}

void WriteCounter(string fileName, int counter)
{
    using (var stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
    using (var writer = new StreamWriter(stream))
    {
        writer.Write(counter);
    }
}