using System;
using System.Collections.Generic;

namespace Client
{
    internal class Program
    {
        static string verbType = "";
        static string filePath = "";
        static Dictionary<string, string> dictionary = new Dictionary<string, string>();
        static private string url = "";
        private static VideoGames_Data videoGamesData = new VideoGames_Data();
        
        public static void Main(string[] args)
        {
            Client client = new Client();
            char optionSelected = '\0';
            Console.WriteLine("Hello User!");
            
            SetURL();
            
            while (optionSelected == '\0')
            {
                Console.Write("What verb do you want to use?\n\t1. GET\n\t2. POST\n\t3. DELETE\n\t4. PUT\n");

                optionSelected = Console.ReadKey().KeyChar;

                switch (optionSelected)
                {
                    case '1':
                        verbType = "GET";
                        filePath = ObtainFilePath();
                        break;
                    case '2':
                        verbType = "POST";
                        filePath = ObtainFilePath();
                        GetGameInfo();
                        break;
                    case '3':
                        verbType = "DELETE";
                        break;
                    case '4':
                        verbType = "PUT";
                        break;
                    default:
                        Console.WriteLine("\nPlease, select a valid option.");
                        optionSelected = '\0';
                        break;
                }
            }
            
            FillHeaders();
            
            client.Request(verbType, filePath, url, dictionary, videoGamesData).Wait();
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

        public static void GetGameInfo()
        {
            Console.WriteLine("Write the information about the game you want to send to Server:");
            
            Console.Write("Title: ");
            videoGamesData.name = Console.ReadLine();
            
            Console.Write("Release Year: ");
            videoGamesData.releaseYear = Console.ReadLine();
            
            Console.Write("Developer: ");
            videoGamesData.developer = Console.ReadLine();
            
            Console.WriteLine("\nInformation saved.");
        }

        public static void SetURL()
        {
            Console.WriteLine("Write the URL of the server: ");
            url = Console.ReadLine();
        }

        public static void FillHeaders()
        {
            dictionary.Add("Content-Type", "text/plain");
        }
    }
}