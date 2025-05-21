using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        Master master = new Master();
        Thread thread = new Thread(() =>
        {
            master.start();
            master.setAffinity(2);
        });
        thread.Start();

        Scanner scannerA = new Scanner("C:\\Users\\smiki\\Desktop\\univero darbai", "agent1");
        Scanner scannerB = new Scanner("C:\\Users\\smiki\\Desktop\\univero darbai", "agent2");
        
        Thread threadA = new Thread(() =>
        {
            scannerA.start();
            scannerA.setAffinity(0);
            
        });
        Thread threadB = new Thread(() => 
        {
            scannerB.start();
            scannerB.setAffinity(1);
        });
        threadA.Start();
        threadB.Start();
        Console.ReadLine();
    }
}
