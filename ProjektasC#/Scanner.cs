using System;
using System.Collections.Generic;
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
    }

    private void readFiles()
    {
        using(NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.Out))
        {
            pipeClient.Connect();
            foreach(string file in Directory.GetFiles(directoryPath,"*.txt"))
            {
                var wordCount = File.ReadAllLines(file)
                    .SelectMany(line => line.Split(' '))
                    .GroupBy(word => word)
                    .ToDictionary(group => group.Key, group => group.Count());

                using(StreamWriter writer = new StreamWriter(pipeClient))
                {
                    foreach (var entry in wordCount)
                    {
                        writer.WriteLine($"{Path.GetFileName(file)}:{entry.Key}:{entry.Value}");
                    }
                }
            }
        }
    }
}