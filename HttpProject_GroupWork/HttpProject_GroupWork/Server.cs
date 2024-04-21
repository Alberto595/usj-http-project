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
        private string verbType = "";
        private string filePath = "";
        private string httpVersion = "";
        
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
            
            while (true)
            {
                var handler = await listener.AcceptAsync();

                //Found: 172.23.64.1 available on port 9000.
                Console.WriteLine("Found: " + ipEndPoint.Address + " available on port " + ipEndPoint.Port);
                
                while (true)
                {
                    ResetVariables();
                    
                    // Receive message.
                    byte[] buffer = new byte[1024];
                    var bufferSegment = new ArraySegment<byte>(buffer);
                    int received = 0;

                    //Check if the client close the connection
                    try
                    {
                        received = await handler.ReceiveAsync(bufferSegment, SocketFlags.None);
                        if (received == 0)
                        {
                            Console.WriteLine("Client close connection.");
                            break;
                        }
                    }
                    catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        Console.WriteLine("Client close connection.");
                        break;
                    }

                    string response = Encoding.UTF8.GetString(bufferSegment.Array, bufferSegment.Offset, received);

                    UnpackRequestClientInformation(response);

                    //Getting the information from a file
                    string line = "";
                    try
                    {
                        using (StreamReader file = new StreamReader("../../../" + filePath))
                        {
                            // Read and display lines from the file until the end of
                            // the file is reached.
                            line = await file.ReadToEndAsync();
                        }
                    }
                    catch
                    {
                        line = "Error 404 not found";
                        Console.WriteLine("The file " + filePath + " has not be found");
                    }
                    
                    byte[] echoBytes = Encoding.UTF8.GetBytes(line);
                    var echoBytesSegment = new ArraySegment<byte>(echoBytes);
                    await handler.SendAsync(echoBytesSegment, 0);

                    Console.WriteLine("The server sends the following information: " + line);
                }
            }
        }

        /// <summary>
        /// Separate the client information to know what to send back. E.g. FilePath, VerbType, etc.
        /// </summary>
        public void UnpackRequestClientInformation(string response)
        {
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
        }

        public void ResetVariables()
        {
            verbType = "";
            filePath = "";
            httpVersion = "";
        }
    }
}