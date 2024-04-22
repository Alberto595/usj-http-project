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
        private string response = "";
        
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

                    string request = Encoding.UTF8.GetString(bufferSegment.Array, bufferSegment.Offset, received);

                    UnpackRequestClientInformation(in request);

                    //Getting the information from a file
                    DoActionBasedOnVerb();
                    
                    // Send Response to the client
                    byte[] echoBytes = Encoding.UTF8.GetBytes(response);
                    var echoBytesSegment = new ArraySegment<byte>(echoBytes);
                    await handler.SendAsync(echoBytesSegment, 0);

                    Console.WriteLine("The server sends the following information: " + response);
                }
            }
        }

        /// <summary>
        /// Separate the client information to know what to send back. E.g. FilePath, VerbType, etc.
        /// </summary>
        public void UnpackRequestClientInformation(in string request)
        {
            Console.WriteLine($"Socket server received message:\n\"{request}\"");

            // GETTING INFORMATION FROM THE REQUEST
            int count = request.Length;
            int positionInRequest = 0;

            UnpackVerbType(in request, count, ref positionInRequest);

            // Get the information of the request depending on the verb type
            switch (verbType)
            {
                case "GET":
                    UnpackGET_VerbType(in request, count, ref positionInRequest);
                    break;
                case "POST":
                    UnpackPOST_VerbType(in request, count, ref positionInRequest);
                    break;
                default:
                    break;
            }
        }

        public void ResetVariables()
        {
            verbType = "";
            filePath = "";
            httpVersion = "";
            response = "";
        }

        public void UnpackVerbType(in string request, int count, ref int positionInRequest)
        {
            for (int i = 0; i < count; i++) //THE VERB
            {
                if (request[i] == ' ')
                {
                    positionInRequest = i + 1;
                    break;
                }
                verbType += request[i];
            }
            
            Console.WriteLine("Verb information: " + verbType);
        }

        public void DoActionBasedOnVerb()
        {
            switch (verbType)
            {
                case "GET":
                    response = GET_VerbAction().Result;
                    break;
                case "POST":
                    response = POST_VerbAction().Result;
                    break;
                default:
                    break;
            }
        }
        
        #region GET_VerbType
        
        public void UnpackGET_VerbType(in string request, int count, ref int positionInRequest)
        {
            for (int i = positionInRequest; i < count; i++) // THE FILE PATH
            {
                if (request[i] == ' ')
                {
                    positionInRequest = i + 1;
                    break;
                }
                filePath += request[i];
            }
                
            for (int i = positionInRequest; i < count; i++) // THE HTTP VERSION
            {
                if (request[i] == '\r')
                {
                    break;
                }
                httpVersion += request[i];
            }
            
            Console.WriteLine("File-Path information: " + filePath);
            Console.WriteLine("Http-Version information: " + httpVersion);
        }

        public async Task<string> GET_VerbAction()
        {
            try
            {
                using (StreamReader file = new StreamReader("../../../" + filePath))
                {
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    response = await file.ReadToEndAsync();
                }
            }
            catch
            {
                response = "Error 404 not found";
                Console.WriteLine("The file " + filePath + " has not be found");
            }

            return response;
        }
        
        #endregion

        #region POST_VerbType

        public void UnpackPOST_VerbType(in string request, int count, ref int positionInRequest)
        {
            
        }
        
        public async Task<string> POST_VerbAction()
        {
            try
            {
                using (StreamReader file = new StreamReader("../../../" + filePath))
                {
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    response = await file.ReadToEndAsync();
                }
            }
            catch
            {
                response = "Error 404 not found";
                Console.WriteLine("The file " + filePath + " has not be found");
            }

            return response;
        }

        #endregion
    }
}