

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