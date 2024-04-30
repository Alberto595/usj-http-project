using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HttpProject_GroupWork
{
    public class Server
    {
        public int port;
        private string verbType;
        private string filePath;
        private string httpVersion;
        private string response;
        private string responseCode;
        private string requestContent;
        private Dictionary<string, string> headersFromRequest;
        private Dictionary<string, string> headersFromResponse;
        
        public Server()
        {
            this.port = 3000;
            verbType = "";
            filePath = "";
            httpVersion = "";
            response = "";
            responseCode = "";
            headersFromRequest = new Dictionary<string, string>();
            headersFromResponse = new Dictionary<string, string>();
        }

        public Server(int port)
        {
            this.port = port;
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
            
            Debug.Log("Socket Server Starting...");
            
            listener.Bind(ipEndPoint);
            listener.Listen(100);
            
            while (true)
            {
                var handler = await listener.AcceptAsync();

                //Found: 172.23.64.1 available on port 9000.
                Debug.Log("Found: " + ipEndPoint.Address + " available on port " + ipEndPoint.Port);
                
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
                            Debug.Log("Client close connection.");
                            break;
                        }
                    }
                    catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        Debug.Log("Client close connection.");
                        break;
                    }

                    string request = Encoding.UTF8.GetString(bufferSegment.Array, bufferSegment.Offset, received);

                    UnpackRequestClientInformation(in request);

                    //Getting the information from a file
                    await DoActionBasedOnVerb();
                    
                    //Make the response
                    MakeTheResponse();
                    
                    // Send Response to the client
                    byte[] echoBytes = Encoding.UTF8.GetBytes(response);
                    var echoBytesSegment = new ArraySegment<byte>(echoBytes);
                    await handler.SendAsync(echoBytesSegment, 0);

                    Debug.Log("The server sends the following information: " + response);
                }
            }
        }

        /// <summary>
        /// Separate the client information to know what to send back. E.g. FilePath, VerbType, etc.
        /// </summary>
        public void UnpackRequestClientInformation(in string request)
        {
            Debug.Log($"Socket server received message:\n\"{request}\"");

            // GETTING INFORMATION FROM THE REQUEST
            int count = request.Length;
            int positionInRequest = 0;

            for (int i = 0; i < count; i++) //THE VERB
            {
                if (request[i] == ' ')
                {
                    positionInRequest = i + 1;
                    break;
                }
                verbType += request[i];
            }
            
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
                    positionInRequest = i + 2; // we jump the \r and \n
                    break;
                }
                httpVersion += request[i];
            }

            //HEADERS 
            int headersEndIndex = request.IndexOf("\r\n\r\n");
            int index = positionInRequest;

            while (index < headersEndIndex ) // Read all the headers
            {
                string key = "";
                string value = "";
                
                while (index < 100000) // Read each header-KEY and add it to the dictionary
                {
                    if (request[index] == ':')
                    {
                        index += 2; // we jump the : and (space)
                        break;
                    }
                    
                    key += request[index];
                    
                    index++;
                }
                
                while (index < 100000) // Read each header-VALUE and add it to the dictionary
                {
                    if (request[index] == '\r')
                    {
                        index += 2; // we jump the \r and \n
                        break;
                    }

                    value += request[index];
                    
                    index++;
                }
                
                //Add to the dictionary the key and the value of the header
                headersFromRequest.Add(key, value);
            }
            
            //Get the content of the request (the Json data)
            requestContent = request.Substring(headersEndIndex + 4); // +4 to omit the \r\n\r\n
            
            Debug.Log("Verb information: " + verbType);
            Debug.Log("File-Path information: " + filePath);
            Debug.Log("Http-Version information: " + httpVersion);
            Debug.Log("Headers:");
            foreach (var o in headersFromRequest)
            {
                Debug.Log(o.Key + ": " + o.Value);
            }
            Debug.Log("Content: \n" + requestContent);
        }

        public void ResetVariables()
        {
            verbType = "";
            filePath = "";
            httpVersion = "";
            response = "";
            responseCode = "";
            headersFromRequest = new Dictionary<string, string>();
            headersFromResponse = new Dictionary<string, string>();
        }

        public async Task DoActionBasedOnVerb()
        {
            switch (verbType)
            {
                case "GET":
                    await GET_VerbAction();
                    break;
                case "POST":
                    await POST_VerbAction();
                    break;
            }
        }

        public void MakeTheResponse()
        {
            //Create the minimum required headers
            headersFromResponse.Add("Content-Type", "text/plain");
            headersFromResponse.Add("Content-Length", response.Length.ToString());
            
            string tmpResponse = httpVersion + " " + responseCode + "\r\n";

            foreach (var header in headersFromResponse)
            {
                tmpResponse += header.Key + ": " + header.Value + "\r\n";
            }

            response = tmpResponse + "\r\n" + response;
        }
        
        #region GET_VerbType
        public async Task GET_VerbAction()
        {
            //If the request Content is empty, the client wants the data file completed. Otherwise, he wants to know some Videogame-Data
            if (requestContent == "")
            {
                await GetDataInformationFile();
            }
            else
            {
                int indexOfName = requestContent.IndexOf(": ") + 2;

                string gameName = requestContent.Substring(indexOfName);
                
                GetVideogameDataFromFile(gameName);
            }
        }

        public async Task GetDataInformationFile()
        {
            try
            {
                using (StreamReader file = new StreamReader(filePath))
                {
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    response = await file.ReadToEndAsync();
                    responseCode = "200 OK";
                }
            }
            catch
            {
                response = "Error 404 not found";
                responseCode = "404 not found";
                Console.WriteLine("The file " + filePath + " has not be found");
            }
        }
        public async void GetVideogameDataFromFile(string nameOfVideogame)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                using (StreamReader file = new StreamReader(fs))
                {
                    List<VideoGames_Data> videogamesData = new List<VideoGames_Data>();
                    string jSonData;
                    while (!file.EndOfStream)
                    {
                        jSonData = await file.ReadLineAsync();
                        //videogamesData.Add(JsonConvert.DeserializeObject<Dictionary<string,string>>(jSonData));
                        videogamesData.Add(JsonUtility.FromJson<VideoGames_Data>(jSonData));
                    }

                    foreach (var videogameData in videogamesData)
                    {
                        if (videogameData.name == nameOfVideogame)
                        {
                            response = JsonUtility.ToJson(videogameData);
                            responseCode = "200 OK";
                        }
                    }

                    if (response == "")
                    {
                        responseCode = "400 Bad Request";
                        response = "Error 400 Bad Request";
                    }
                }
            }
        }
        
        #endregion

        #region POST_VerbType
        
        public async Task POST_VerbAction()
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Append))
                {
                    using (StreamWriter file = new StreamWriter(fs))
                    {
                        // Write (append) in the file selected, if it isn't exists it creates a new file
                        await file.WriteAsync(requestContent);
                    }   
                }
                
                responseCode = "201 Created";
                response = "File created successfully.";
            }
            catch
            {
                responseCode = "406 Not Acceptable";
                response = "Error 406 Not Acceptable";
                Debug.Log("The file " + filePath + " has not be found");
            }
        }

        #endregion
    }
}