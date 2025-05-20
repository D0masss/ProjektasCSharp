using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        Master master = new Master();
        master.start();

        Scanner scanner = new Scanner("C:\\Users\\smiki\\Desktop\\univero darbai", "agent1");
        Scanner scanner2 = new Scanner("C:\\Users\\smiki\\Desktop\\univero darbai", "agent2");

        scanner.start();
        scanner2.start();
        Console.ReadLine();
    }
}
