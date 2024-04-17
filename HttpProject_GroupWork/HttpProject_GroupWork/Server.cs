using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HttpProject_GroupWork
{
    public class Server
    {
        public int port;
        private string verbType;
        private string filePath;
        private string httpVersion;
        
        public Server()
        {
            this.port = 3000;
            
        }
        
        public async Task ServerMethod()
        {
            IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            
            Socket listener = new Socket(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);
            
            Console.WriteLine("Socket Server Starting...");
            
            listener.Bind(ipEndPoint);
            listener.Listen(100);
            
            var handler = await listener.AcceptAsync();
            
            
            //Found: 172.23.64.1 available on port 9000.
            Console.WriteLine("Found: " + ipEndPoint.Address + "available on port " + ipEndPoint.Port);

            while (true)
            {
                // Receive message.
                byte[] buffer = new byte[1024];
                var bufferSegment = new ArraySegment<byte>(buffer);
                int received = await handler.ReceiveAsync(bufferSegment, SocketFlags.None);
                string response = Encoding.UTF8.GetString(bufferSegment.Array, bufferSegment.Offset, received);
                
                Console.WriteLine($"Socket server received message:\n\"{response}\"");

                // GETTING INFORMATION FROM THE REQUEST
                int count = response.Length;
                int positionInRequest = 0;
                for (int i = 0; i < count; i++) //THE VERB
                {
                    if (response[i] == ' ')
                    {
                        positionInRequest = i + 1;
                        break;
                    }
                    verbType += response[i];
                }

                for (int i = positionInRequest; i < count; i++) // THE FILE PATH
                {
                    if (response[i] == ' ')
                    {
                        positionInRequest = i + 1;
                        break;
                    }
                    filePath += response[i];
                }
                
                for (int i = positionInRequest; i < count; i++) // THE HTTP VERSION
                {
                    if (response[i] == '\r')
                    {
                        break;
                    }
                    httpVersion += response[i];
                }
                
                Console.WriteLine("Verb information: " + verbType);
                Console.WriteLine("File-Path information: " + filePath);
                Console.WriteLine("Http-Version information: " + httpVersion);
                
                //Getting the information from a file
                try
                {
                    using (StreamReader file = new StreamReader("../../../tetas.txt"))
                    {
                        string line;
                        // Read and display lines from the file until the end of
                        // the file is reached.
                        while ((line = await file.ReadLineAsync()) != null)
                        {
                            Console.WriteLine(line);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e + ". The file has not be found");
                    throw;
                }
                
                
                // string eom = "<|EOM|>";
                // if (response.IndexOf(eom) > -1 /* is end of message */)
                // {
                //     Console.WriteLine(
                //         $"Socket server received message: \"{response.Replace(eom, "")}\"");
                //
                //     var ackMessage = "<|ACK|>";
                //     byte[] echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                //     var echoBytesSegment = new ArraySegment<byte>(echoBytes);
                //     await handler.SendAsync(echoBytesSegment, 0);
                //     Console.WriteLine(
                //         $"Socket server sent acknowledgment: \"{ackMessage}\"");
                //
                //     break;
                // }
                // Sample output:
                //    Socket server received message: "Hi friends 👋!"
                //    Socket server sent acknowledgment: "<|ACK|>"
            }
        }
    }
}