using System;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IDataReader reader = new DataReader();
                reader.ImportAndPrintData("data.csv");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }
    }
}
