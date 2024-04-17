using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Client
    {
        public string server;
        public int port;
        public string request;
        public string verbType;
        public string filePath = "";
        
        public Client(string verb = "GET", string server = "localhost")
        {
            this.server = server;
            this.port = 3000;
            this.verbType = verb;
            this.request = " HTTP/1.1\r\n"
                             + "Host: " + server + "\r\n"
                             + "Connection: close";
        }
        
        public async Task ClientMethod()
        {
            IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(server);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            
            Socket client = new Socket(
                ipEndPoint.AddressFamily, 
                SocketType.Stream, 
                ProtocolType.Tcp);
            
            await client.ConnectAsync(ipEndPoint);
            
            while (true)
            {
                // Send message.
                // var message = "Hi friends 👋!<|EOM|>";
                BuildRequest();
                byte[] messageBytes = Encoding.UTF8.GetBytes(request);
                var messageBytesSegment = new ArraySegment<byte>(messageBytes);
                _ = await client.SendAsync(messageBytesSegment, SocketFlags.None);
                Console.WriteLine($"Socket client sent message: \n\"{request}\"");

                // Receive ack.
                byte[] buffer = new byte[1024];
                var bufferSegment = new ArraySegment<byte>(buffer);
                var received = await client.ReceiveAsync(bufferSegment, SocketFlags.None);
                var response = Encoding.UTF8.GetString(bufferSegment.Array, bufferSegment.Offset, received);
                if (response == "<|ACK|>")
                {
                    Console.WriteLine("Socket client received acknowledgment: \n"+ response);
                    break;
                }
                // Sample output:
                //     Socket client sent message: "Hi friends 👋!<|EOM|>"
                //     Socket client received acknowledgment: "<|ACK|>"
            }

            client.Shutdown(SocketShutdown.Both);
            // try
            // {
            //     //create and connect a dual-stack socket
            //     Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            //     socket.Connect(server, port);
            //
            //     //convert request to buffer
            //     byte[] myReadBuffer = Encoding.UTF8.GetBytes(request);
            //
            //     Console.WriteLine(socket.Send(myReadBuffer));
            //
            //     Console.WriteLine(socket.Receive(myReadBuffer));
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            //     throw;
            // }
        }

        public void BuildRequest()
        {
            request = verbType + " " + filePath + request /* + body */;
        }
    }
}