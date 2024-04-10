using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpProject_GroupWork
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Server server = new Server();
            
            server.ServerMethod().Wait();

        }
    }
    
    
}