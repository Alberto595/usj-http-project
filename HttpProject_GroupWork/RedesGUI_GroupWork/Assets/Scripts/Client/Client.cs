using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Client
{
    //Connection variables
    public string server;

    public int port;

    //General variables
    public string request;
    public string verbType;

    public string body = "";

    //UI variables
    public ClientServerMessages UImessages;

    //Global Variables
    public string filePath = "/";

    public bool firstGet = true;
    public DateTime ps5Date;
    public DateTime xboxDate;
    public DateTime switchDate;
    public string console = null;
    public string dateHTTP = null;
    public Client(string verb = "GET", string server = "localhost")
    {
        this.server = server;
        this.port = 3000;
        this.verbType = verb;
        this.request = "HTTP/1.1\r\n"
                       + "Host: " + server + "\r\n"
                       + "Connection: close";
    }

    public Client(string server, int port, string verbType, string body, string filePath)
    {
        this.server = server;
        this.port = port;
        this.request = "";
        this.verbType = verbType;
        this.body = body;
        this.filePath = filePath;
    }

    public void ResetVariables()
    {
        request = "";
        verbType = "";
        body = "";
        filePath = "/";
    }

    public async Task Request(string verbType, string filepath, string url, Dictionary<string, string> headers,
        string body, VideoGames_Data data)
    {
        AdaptRequestData(verbType, filepath, url, headers, body, data);

        IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(server);
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
        
        Debug.Log("Client connecting...");

        Socket client = new Socket(
            ipEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);

        await client.ConnectAsync(ipEndPoint);
        
        Debug.Log("Client connected in: " + ipEndPoint);

        // Send message
        BuildRequest();
        byte[] messageBytes = Encoding.UTF8.GetBytes(request);
        var messageBytesSegment = new ArraySegment<byte>(messageBytes);
        _ = await client.SendAsync(messageBytesSegment, SocketFlags.None);
        Debug.Log($"Socket client sent message: \n\"{request}\"");

        //send the message to the text field in the UI
        UImessages.ChangeClientText(request);

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

            Debug.Log("The server sends the next information: \r\n" + message);

            //update the game panel info
            List<VideoGames_Data> gamesInfo = UnpackRequestClientInformation(message);
            GamePanel.Instance.UpdateGamePanel(gamesInfo);

            //update UI message for server
            UImessages.ChangeServerText(message);

        }
        catch (Exception e)
        {
            Debug.Log("Retrieved the server's data information fails: " + e);
            throw;
        }
        finally
        {
            ResetVariables();
            client.Shutdown(SocketShutdown.Both);
        }
    }

    public void BuildRequest()
    {
        
        if (verbType == "GET")
        {
           
            UpdateDate();
            Debug.Log("esta es la consola: " + console);
            if (firstGet)
            {
                //copiar cache
                firstGet = false;
          
            }
            // depending on the console you are accesing sends the request with its respective date
            switch (console)
            {
                case "atad.5SP":

                        dateHTTP = ps5Date.ToUniversalTime().ToString("R");                                                                         //transforms into http date time format(GMT)
                        request = verbType + " " + filePath + " " + request + "If-Modified-Since: " + dateHTTP + "\r\n\r\n" + body;
                        dateHTTP = null;                                                                                                            //resets the string into "null" in case it mixes
                    break;
                case "atad.xobX":
                        
                        dateHTTP = xboxDate.ToUniversalTime().ToString("R");
                        request = verbType + " " + filePath + " " + request + "If-Modified-Since: " + dateHTTP + "\r\n\r\n" + body;
                        dateHTTP = null;
                    break;
                case "atad.hctiwS":

                        dateHTTP = switchDate.ToUniversalTime().ToString("R");
                        request = verbType + " " + filePath + " " + request + "If-Modified-Since: " + dateHTTP + "\r\n\r\n" + body;
                        dateHTTP = null;
                    break;
                default: break;
            }
           
            
        }
        else
        {
            request = verbType + " " + filePath + " " + request + "\r\n" + body;
        }
       
        
        Debug.Log("this is the request: " + request);
    }
    
    /// <summary>
    /// Separate the client information to know what to send back. E.g. FilePath, VerbType, etc.
    /// </summary>
    public List<VideoGames_Data> UnpackRequestClientInformation(in string reply)
    {
        int headersEndIndex = reply.IndexOf("\r\n\r\n");
       
        string replycontent = reply.Substring(headersEndIndex + 4); // +4 to omit the \r\n\r\n
        
        Debug.Log("Content: \n" + replycontent);

        List<VideoGames_Data> gameList = new List<VideoGames_Data>();
        string[] games = replycontent.Split("\r\n");
        int count = games.Length - 1;
        for (int i = 0; i < count; i++)
        {
            string tmp = games[i];
            gameList.Add(JsonUtility.FromJson<VideoGames_Data>(games[i]));
        }

        return gameList;
    }

    public void AdaptRequestData(string verbType, string filepath, string url, Dictionary<string, string> headers,
        string body, VideoGames_Data data)
    {
        //VERB TYPE
        this.verbType = verbType;
        //FILE PATH
        this.filePath = filepath;
        //SERVER URL
        this.server = url;
        //BODY
        if (data.name != "") // The videogameData is not empty
        {
            this.body = JsonUtility.ToJson(data);
        }
        else if (body != "") // This is used for example when the client make a get operation, write in the body just the name to making the request
        {
            this.body = "name: " + body;
        }

        //HEADERS and make part of the REQUEST
        this.request = "HTTP/1.1\r\n";

        foreach (var tuple in headers)
        {
            this.request += tuple.Key + ": " + tuple.Value + "\r\n";
        }

        this.request += "Content-Length: " + this.body.Length + "\r\n";
    }
    public void UpdateDate()
    {
        
        console = null;
        //Takes the path ?.data
        for(int i = filePath.Length-1; i >= 0; i--)
        {
            if (filePath[i] != '/' )
            {
                console += filePath[i];
            }
            else { break; }
           
        }
        //Updates the time you are accesing to this info
        switch (console)
        {
            case "atad.5SP":
                    ps5Date = DateTime.Now;
                break;
            case "atad.xobX":
                xboxDate = DateTime.Now;
                break;
            case "atad.hctiwS":
                switchDate = DateTime.Now;
                break;
             default: break;
        }
    }

    public bool GetFirstTime()
    {
        return firstGet;
    }
}