using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HttpProject_GroupWork
{
    public class Server
    {
        public int port;
        
        
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
                
                string eom = "<|EOM|>";
                if (response.IndexOf(eom) > -1 /* is end of message */)
                {
                    Console.WriteLine(
                        $"Socket server received message: \"{response.Replace(eom, "")}\"");

                    var ackMessage = "<|ACK|>";
                    byte[] echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                    var echoBytesSegment = new ArraySegment<byte>(echoBytes);
                    await handler.SendAsync(echoBytesSegment, 0);
                    Console.WriteLine(
                        $"Socket server sent acknowledgment: \"{ackMessage}\"");

                    break;
                }
                // Sample output:
                //    Socket server received message: "Hi friends 👋!"
                //    Socket server sent acknowledgment: "<|ACK|>"
            }
        }
    }
}