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
            this.request = "HTTP/1.1\r\n"
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
            
            // Send message.
            BuildRequest();
            byte[] messageBytes = Encoding.UTF8.GetBytes(request);
            var messageBytesSegment = new ArraySegment<byte>(messageBytes);
            _ = await client.SendAsync(messageBytesSegment, SocketFlags.None);
            Console.WriteLine($"Socket client sent message: \n\"{request}\"");

            try
            {
                const int bufferSize = 1024;
                byte[] buffer = new byte[bufferSize];
            
                // StringBuilder to store the completed message
                StringBuilder fullMessage = new StringBuilder();

                int bytesRead;
                while ((bytesRead = await client.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None)) > 0)
                {
                    // Transform the received bytes into a string and append them to the final message
                    fullMessage.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                
                    //Check if the message has ended
                    if (bytesRead < bufferSize)
                        break;
                }

                string message = fullMessage.ToString();
            
                Console.WriteLine(message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Retrieved the server's data information fails: " + e);
                throw;
            }
            

            client.Shutdown(SocketShutdown.Both);
        }

        public void BuildRequest()
        {
            request = verbType + " " + filePath + " " + request /* + body */;
        }
    }
}