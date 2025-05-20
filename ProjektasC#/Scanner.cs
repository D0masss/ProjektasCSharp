using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Scanner
{
    private string directoryPath;
    private string pipeName;

    public Scanner(string directoryPath, string pipeName)
    {
        this.directoryPath = directoryPath;
        this.pipeName = pipeName;
        assignCPUCore();
    }

    public void start()
    {
        Task.Run(() => readFiles());
        // Simulate some work in the main thread
        Console.WriteLine($"Scanner {pipeName} is running...");
        Console.ReadLine();
    }

    private void readFiles()
    {
        using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.Out))
        {
            pipeClient.Connect();
            using (StreamWriter writer = new StreamWriter(pipeClient))
            {
                foreach (string file in Directory.GetFiles(directoryPath, "*.txt"))
                {
                    var wordCount = File.ReadAllLines(file)
                        .SelectMany(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                        .GroupBy(word => word)
                        .ToDictionary(group => group.Key, group => group.Count());

                    foreach (var entry in wordCount)
                    {
                        writer.WriteLine($"{Path.GetFileName(file)}:{entry.Key}:{entry.Value}");
                    }
                }
                writer.Flush();
            }
        }

    }

    private void assignCPUCore()
    {
        Process currentProcess = Process.GetCurrentProcess();
        ProcessThread thread = currentProcess.Threads[0];
        thread.ProcessorAffinity = (IntPtr)(1 << 0);
    }
}