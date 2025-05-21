using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Master
{
    private string[] pipeNames = { "agent1", "agent2"};

    public void start()
    {
        foreach (var pipeName in pipeNames)
        {
            Task.Run(() => listenToPipe(pipeName));
        }
        Console.WriteLine("Master is running...");
      
    }

    private void listenToPipe(string pipeName)
    {
        using (NamedPipeServerStream pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.In))
        {
            Console.WriteLine($"[{pipeName}] Waiting for connection...");
            pipeServer.WaitForConnection();
            using (StreamReader reader = new StreamReader(pipeServer))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }

    public void setAffinity(int core)
    {
        if (core < 0 || core >= Environment.ProcessorCount)
        {
            throw new ArgumentOutOfRangeException(nameof(core), "Invalid core number.");
        }
        else
        {
            Process currentProcess = Process.GetCurrentProcess();
            IntPtr mask = (IntPtr)(1 << core);
            currentProcess.ProcessorAffinity = mask;
            Console.WriteLine($"Master is running on core {core}.");
        }
    }
}
