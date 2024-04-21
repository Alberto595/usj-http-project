using System;

namespace Client
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Client client = new Client();
            char optionSelected = '\0';
            Console.WriteLine("Hello User!");
            
            while (optionSelected == '\0')
            {
                Console.Write("What verb do you want to use?\n1. GET\n2. POST\n3. DELETE\n4. PUT\n");

                optionSelected = Console.ReadKey().KeyChar;

                switch (optionSelected)
                {
                    case '1':
                        client.verbType = "GET";
                        client.filePath = GET_Header();
                        
                        break;
                    case '2':
                        client.verbType = "POST";
                        break;
                    case '3':
                        client.verbType = "DELETE";
                        break;
                    case '4':
                        client.verbType = "PUT";
                        break;
                    default:
                        Console.WriteLine("\nPlease, select a valid option.");
                        optionSelected = '\0';
                        break;
                }
            }
            
            client.ClientMethod().Wait();
        }

        public static string GET_Header()
        {
            string filePath;
            
            Console.Write("\nPlease, specify the file path: ");
            filePath = Console.ReadLine();
            return filePath;
        }
    }
}