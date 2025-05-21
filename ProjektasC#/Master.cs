using System;
using System.Collections.Generic;
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
        // Simulate some work in the main thread
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
}
