using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Client
{
    public class Client
    {
        //Connection variables
        public string server;
        public int port;
        //General variables
        public string request;
        public string verbType;
        public string body = "";
        //GET variables
        public string filePath = "/";
        //POST variables
        public string contentType = "";
        /// <summary>
        /// indicates the length of the message body, in bytes.
        /// </summary>
        public string contentLenght = "";
        /// <summary>
        /// In a multipart form, each value is sent as a block of data and a delimiter is used to separate each part.
        /// The delimiter between parts is called the Boundary
        /// </summary>
        public string boundary = "NextField";
        
        public Client(string verb = "GET", string server = "localhost")
        {
            this.server = server;
            this.port = 3000;
            this.verbType = verb;
            this.request = "HTTP/1.1\r\n"
                             + "Host: " + server + "\r\n"
                             + "Connection: close";
        }
        
        public async Task Request(string verbType, string filepath, string url, Dictionary<string,string> headers, VideoGames_Data data)
        {
            AdaptRequestData(verbType, filepath, url, headers, data);
            
            IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(server);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            
            Socket client = new Socket(
                ipEndPoint.AddressFamily, 
                SocketType.Stream, 
                ProtocolType.Tcp);
            
            await client.ConnectAsync(ipEndPoint);
            
            // Send message
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
            request = verbType + " " + filePath + " " + request + "\r\n" + body;
        }

        public void AdaptRequestData(string verbType, string filepath, string url, Dictionary<string,string> headers, VideoGames_Data data)
        {
            //VERB TYPE
            this.verbType = verbType;
            //FILE PATH
            this.filePath = filepath;
            //SERVER URL
            this.server = url;
            //BODY
            if (data.name != "")
            {
                this.body = JsonConvert.SerializeObject(data);
            }
            
            //HEADERS
            this.request = "HTTP/1.1\r\n";

            foreach (var tuple in headers)
            {
                this.request += tuple.Key + ": " + tuple.Value + "\r\n";
            }
            
            this.request += "Content-Length: " + this.body.Length + "\r\n";
        }
    }
}