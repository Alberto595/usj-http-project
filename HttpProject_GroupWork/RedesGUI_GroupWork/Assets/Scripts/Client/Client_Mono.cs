using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using HttpProject_GroupWork;
using UnityEditor.PackageManager;
using UnityEngine;

public class Client_Mono : MonoBehaviour
{
    public VideoGames_Data videoGamesData = new VideoGames_Data("Call of Duty", "2024", "Alberto&TuMadre");
    public ClientServerMessages UImessages;

    private Client client;
    
    // Start is called before the first frame update
    void Start()
    {
        InitiateClient();
    }

    public void InitiateClient()
    {
        client = new Client("localhost",3000, "GET", "", UrlManager.Instance.pathToSaveVideogameData);
        client.UImessages = UImessages;
        
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-type", "text/plain");
        
        //client.Request("DELETE",UrlManager.Instance.pathToSaveVideogameData,"localhost", headers,"", videoGamesData);
    }

    public void AcceptRequestFromButton(string verb, Dictionary<string, string> newHeaders, string body, VideoGames_Data gamedata)
    {

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-type", "text/plain");

        try
        {
            foreach (KeyValuePair<string, string> h in newHeaders)
            {
                headers.Add(h.Key, h.Value);
            }
        }
        catch (System.Exception)
        {

            
        }


        client.Request(verb, UrlManager.Instance.pathToSaveVideogameData, "localhost", headers, body, gamedata);

        
    }

}
