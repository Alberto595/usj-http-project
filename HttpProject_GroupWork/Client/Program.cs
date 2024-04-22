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
                Console.Write("What verb do you want to use?\n\t1. GET\n\t2. POST\n\t3. DELETE\n\t4. PUT\n");

                optionSelected = Console.ReadKey().KeyChar;

                switch (optionSelected)
                {
                    case '1':
                        client.verbType = "GET";
                        client.filePath = ObtainFilePath();
                        break;
                    case '2':
                        client.verbType = "POST";
                        POST_Header(ref client);
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

        public static string ObtainFilePath()
        {
            string filePath;
            
            Console.Write("\nPlease, specify the file path: ");
            filePath = Console.ReadLine();

            if (filePath == "\n" || filePath == "\r")
            {
                filePath = "/";
            }
            
            return filePath;
        }

        public static void POST_Header(ref Client client)
        {
            char optionSelected = '\0';
            Console.WriteLine("\nPlease, specify the operation you want to use:");
            Console.WriteLine("\t1. Default\n\t2. Multipart Form\n\t3. Text plain");

            while (optionSelected == '\0')
            {
                optionSelected = Console.ReadKey().KeyChar;

                switch (optionSelected)
                {
                    case '1':
                        client.contentType = "application/x-www-form-urlencoded";
                        CreatePostRequest(ref client);
                        break;
                    case '2':
                        client.contentType = "multipart/form-data; boundary=\"" + client.boundary + "\"";
                        CreatePostRequest(ref client);
                        break;
                    case '3':
                        client.contentType = "text/plain";
                        client.filePath = ObtainFilePath();
                        PlainTextBodyWriter(ref client);
                        CreatePostRequest(ref client);
                        break;
                    default:
                        Console.WriteLine("PLEASE, SELECT A CORRECT ANSWER");
                        optionSelected = '\0';
                        break;
                }
            }
        }

        public static void PlainTextBodyWriter(ref Client client)
        {
            Console.WriteLine("Write the plain message you want to send to Server:");

            client.body = Console.ReadLine();
            
            Console.WriteLine("\nBody text saved.");
        }

        public static void CreatePostRequest(ref Client client)
        {
            client.request = "HTTP/1.1\r\n"
                             + "Host: " + client.server + "\r\n"
                             + "Content-Type: " + client.contentType + "\r\n"
                             + "Content-Length: " + client.body.Length + "\r\n\r\n";
        }
    }
}