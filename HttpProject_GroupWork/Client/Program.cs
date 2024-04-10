using System;

namespace Client
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Client client = new Client();

            client.ClientMethod().Wait();

            //Console.ReadKey();
        }
    }
}