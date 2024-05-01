using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using HttpProject_GroupWork;
using UnityEngine;

public class Client_Mono : MonoBehaviour
{
    public VideoGames_Data videoGamesData = new VideoGames_Data("Call of Duty", "2024", "Alberto&TuMadre");
    
    // Start is called before the first frame update
    void Start()
    {
        InitiateClient();
    }

    public void InitiateClient()
    {
        Client client = new Client("localhost",3000, "GET", "", UrlManager.Instance.pathToSaveVideogameData);
        
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-type", "text/plain");
        
        client.Request("DELETE",UrlManager.Instance.pathToSaveVideogameData,"localhost", headers,"", videoGamesData);
    }
}
