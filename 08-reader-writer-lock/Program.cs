class Program
{
    private static ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
    private static Dictionary<int, string> sharedCache = new Dictionary<int, string>();

    static void Reader(int id)
    {
        for (int i = 0; i < 5; i++)
        {
            rwLock.EnterReadLock();
            try
            {
                if (sharedCache.TryGetValue(i, out string value))
                {
                    Console.WriteLine($"Reader {id} read: Key={i}, Value={value}");
                }
                else
                {
                    Console.WriteLine($"Reader {id} found no value for Key={i}");
                }
            }
            finally
            {
                rwLock.ExitReadLock();
            }
            Thread.Sleep(100);
        }
    }

    static void Writer(int id)
    {
        for (int i = 0; i < 5; i++)
        {
            rwLock.EnterWriteLock();
            try
            {
                sharedCache[i] = $"Value from Writer {id}";
                Console.WriteLine($"Writer {id} wrote: Key={i}, Value={sharedCache[i]}");
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
            Thread.Sleep(150);
        }
    }

    static void UpgradeableReader(int id)
    {
        for (int i = 0; i < 5; i++)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                if (!sharedCache.ContainsKey(i))
                {
                    Console.WriteLine($"UpgradeableReader {id} sees Key={i} missing, upgrading to write...");
                    rwLock.EnterWriteLock();
                    try
                    {
                        sharedCache[i] = $"Added by UpgradeableReader {id}";
                        Console.WriteLine($"UpgradeableReader {id} wrote: Key={i}, Value={sharedCache[i]}");
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
                else
                {
                    Console.WriteLine($"UpgradeableReader {id} read existing: Key={i}, Value={sharedCache[i]}");
                }
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
            Thread.Sleep(120);
        }
    }

    static void Main()
    {
        Thread[] readers = new Thread[2];
        Thread[] writers = new Thread[1];
        Thread[] upgradeables = new Thread[1];

        for (int i = 0; i < readers.Length; i++)
        {
            int id = i;
            readers[i] = new Thread(() => Reader(id));
            readers[i].Start();
        }

        for (int i = 0; i < writers.Length; i++)
        {
            int id = i;
            writers[i] = new Thread(() => Writer(id));
            writers[i].Start();
        }

        for (int i = 0; i < upgradeables.Length; i++)
        {
            int id = i;
            upgradeables[i] = new Thread(() => UpgradeableReader(id));
            upgradeables[i].Start();
        }

        foreach (var r in readers) r.Join();
        foreach (var w in writers) w.Join();
        foreach (var u in upgradeables) u.Join();

        Console.WriteLine("All threads finished.");
    }
}

//- Readers freely read values in parallel.
//- Writers block everyone else while writing.
//- UpgradeableReader starts with a read lock, checks if a key exists, and only upgrades to a write lock if it needs to insert.
