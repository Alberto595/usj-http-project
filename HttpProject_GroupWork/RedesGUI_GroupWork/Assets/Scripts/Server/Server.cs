using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using Random = System.Random;

namespace HttpProject_GroupWork
{
    public class Server
    {
        public int port;
        private string verificationCode;
        private string userName;
        private string verbType;
       
        private string filePath;
        private string httpVersion;
        private string response;
        private string responseCode;
        private string requestContent;
        private Dictionary<string, string> headersFromRequest;
        private Dictionary<string, string> headersFromResponse;
        private DateTime ps5LastDate;
        private DateTime xboxLastDate;
        private DateTime switchLastDate;
        private DateTime modifyDate;
        private DateTime getDate;
        private bool firstTime =  true;
        public Server()
        {
            this.port = 3000;
            verificationCode = "";
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
            
            for (int i = 0; i < count; i++) //THE VERIFICATION
            {
                if (request[i] == ' ')
                {
                    positionInRequest = i + 1;
                    break;
                }
                userName += request[i];
            }
            
            for (int i = positionInRequest; i < count; i++) //THE UserName
            {
                if (request[i] == ' ')
                {
                    positionInRequest = i + 1;
                    break;
                }
                verificationCode += request[i];
            }

            for (int i = positionInRequest; i < count; i++) //THE VERB
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
                //Takes the date and converts it into a datetime variable
                if(key == "If-Modified-Since") { getDate = DateTime.Parse(value); }
                Debug.Log("la fecha del get: " + getDate);
                
                //Add to the dictionary the key and the value of the header
                headersFromRequest.Add(key, value);
                Debug.Log("Esta es la key: " + key + "\nEste es el value: " + value);
            }
            
            //Get the content of the request (the Json data)
            requestContent = request.Substring(headersEndIndex + 4); // +4 to omit the \r\n\r\n
            
            Debug.Log("User Name information: " + userName);
            Debug.Log("Verification Code information: " + verificationCode);
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
            userName = "";
            verificationCode = "";
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
                case "PUT":
                    await PUT_VerbAction();
                    break;
                case "DELETE":
                    DELETE_VerbAction();
                    break;
                case "HEAD":
                    HEAD_VerbAction();
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

        public bool Verification()
        {
            string usersDataFilePath = UrlManager.Instance.pathToSaveUsersData;
            using (FileStream fs = new FileStream(usersDataFilePath, FileMode.Open))
            {
                using (StreamReader file = new StreamReader(fs))
                {
                    List<Users_Data> usersDatas = new List<Users_Data>();
                    string jSonData;
                    while (!file.EndOfStream)
                    {
                        jSonData = file.ReadLine();
                        usersDatas.Add(JsonUtility.FromJson<Users_Data>(jSonData));
                    }

                    response = "";
                    foreach (var usersData in usersDatas)
                    {
                        bool isContained = usersData.userName.IndexOf(userName, StringComparison.OrdinalIgnoreCase) > 0;
                        bool verificationCorrect = usersData.verificationToken.IndexOf(verificationCode, StringComparison.OrdinalIgnoreCase) > 0;
                        if (isContained && verificationCorrect)
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
        }
        
        public string GenerateCode(int p_CodeLength)
        {
            string result = ""; 
 
            string pattern = "+-_#!?0123456789abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
            
            Random myRndGenerator = new Random((int)DateTime.Now.Ticks);

            for(int i=0; i < p_CodeLength; i++)
            {
                int mIndex = myRndGenerator.Next(pattern.Length);
                result += pattern[mIndex];
            }

            return result;
        }
        
        #region GET_VerbType
        public async Task GET_VerbAction()
        {
            if (Verification())
            {
                // takes the date depending of the console
                DateTime comparingDate = GetDateFromPath();
                
                //If the request Content is empty, the client wants the data file completed. Otherwise, he wants to know some Videogame-Data
                if (requestContent == "")
                {
                    
                    if (comparingDate > getDate || firstTime)
                    {
                        await GetDataInformationFile();
                        modifyDate = DateTime.Now;
                        firstTime = false;
                    }
                    
                }
                else
                {
                    if (comparingDate > getDate || firstTime) {
                        int indexOfName = requestContent.IndexOf(": ") + 2;

                        string gameName = requestContent.Substring(indexOfName);
                    
                        GetVideogameDataFromFile(gameName);
                        modifyDate =  DateTime.Now;
                        firstTime = false;
                    }
                }
            }
            else
            {
                response = "";
                responseCode = "";
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
                        videogamesData.Add(JsonUtility.FromJson<VideoGames_Data>(jSonData));
                    }

                    response = "";
                    foreach (var videogameData in videogamesData)
                    {
                        //bool isContained = videogameData.name.Contains(nameOfVideogame);
                        bool isContained = videogameData.name.IndexOf(nameOfVideogame, StringComparison.OrdinalIgnoreCase) >= 0;
                        if (isContained)
                        {
                            response += JsonUtility.ToJson(videogameData) + "\r\n";
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
            if (Verification())
            {
                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Append))
                    {
                        // Write (append) in the file selected, if it isn't exists it creates a new file
                        await file.WriteLineAsync(requestContent);
                        //Updates the date of the file depending of the console
                        UpdateDate();
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
            else
            {
                response = "";
                responseCode = "";
            }
        }

        #endregion

        #region PUT_VerbType

        public async Task PUT_VerbAction()
        {
            if (Verification())
            {
                try
                {
                    VideoGames_Data gameDataFromFile;
                    VideoGames_Data inputGameData = JsonUtility.FromJson<VideoGames_Data>(requestContent);
                    List<string> jsonData = new List<string>();

                    bool fileIsEmpty = false;
                    bool isNewData = true;

                    if (!File.Exists(filePath))
                    {
                        throw new Exception();
                    }

                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            if (sr.EndOfStream)
                            {
                                fileIsEmpty = true;
                            }

                            while (!sr.EndOfStream)
                            {
                                jsonData.Add(await sr.ReadLineAsync());
                            }
                        }
                    }

                    using (FileStream fs = new FileStream(filePath, FileMode.Truncate))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            await sw.WriteLineAsync(data);
                        }
                        //Updates the date of the file depending of the console
                        UpdateDate();
                        responseCode = "214 Transformation Applied";
                        response = "Transformation Applied.";
                        
                        if (fileIsEmpty)
                        {
                            await sw.WriteLineAsync(requestContent);

                            responseCode = "201 Created";
                            response = "File created successfully.";
                            return;
                        }

                        int count = jsonData.Count;
                        for (int i = 0; i < count; i++)
                        {
                            gameDataFromFile = JsonUtility.FromJson<VideoGames_Data>(jsonData[i]);

                            if (gameDataFromFile.name == inputGameData.name)
                            {
                                jsonData[i] = requestContent;
                                isNewData = false;
                                break;
                            }
                        }
                        
                        if (isNewData)
                        {
                            jsonData.Add(requestContent);
                        }

                        foreach (var data in jsonData)
                        {
                            await sw.WriteLineAsync(data);
                        }

                        responseCode = "214 Transformation Applied";
                        response = "Transformation Applied.";
                    }
                }
                catch
                {
                    responseCode = "406 Not Acceptable";
                    response = "Error 406 Not Acceptable";
                    Debug.Log("The file " + filePath + " has not be found");
                }
            }
            else
            {
                response = "";
                responseCode = "";
            }
        }

        #endregion
        
        #region DELETE_VerbType

        private void DELETE_VerbAction()
        {
            if (Verification())
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);

                    responseCode = "204 No Content";
                    response = "No Content";
                }
                else
                {
                    responseCode = "404 Not Found";
                    response = "Error 404 Not Found";
                }
            }
            else
            {
                response = "";
                responseCode = "";
            }
            ps5LastDate = DateTime.Now;
            xboxLastDate = DateTime.Now;
            switchLastDate = DateTime.Now;
        }
  
        #endregion

        #region HEAD_verbType

        private void HEAD_VerbAction()
        {
            if (Verification())
            {
                if (File.Exists(filePath))
                {

                    responseCode = "200 OK";
                    response = "OK";
                }
                else
                {
                    responseCode = "404 Not Found";
                    response = "Error 404 Not Found";
                }
            }else
            {
                response = "";
                responseCode = "";
            }
        }

        #endregion

        #region GetDateFromPath
        private DateTime GetDateFromPath()
        {
            string console = null;
            for (int i = filePath.Length - 1; i >= 0; i--)
            {
                if (filePath[i] != '/')
                {
                    console += filePath[i];
                }
                else { break; }

            }
            switch (console)
            {
                case "atad.5SP":
                    return ps5LastDate;
                    
                case "atad.xobX":
                    return xboxLastDate;
                    
                case "atad.hctiwS":
                    return switchLastDate;
                
                
            }
            
            return DateTime.Now;
        }
        #endregion

        #region UpdateDate
        public void UpdateDate()
        {

            string console = null;
            //Takes the path ?.data
            for (int i = filePath.Length - 1; i >= 0; i--)
            {
                if (filePath[i] != '/')
                {
                    console += filePath[i];
                }
                else { break; }

            }
            //Updates the time you are accesing to this info
            switch (console)
            {
                case "atad.5SP":
                    ps5LastDate = DateTime.Now;
                    break;
                case "atad.xobX":
                    xboxLastDate = DateTime.Now;
                    break;
                case "atad.hctiwS":
                    switchLastDate = DateTime.Now;
                    break;
                default: break;
            }
        }
        #endregion
    }

}
