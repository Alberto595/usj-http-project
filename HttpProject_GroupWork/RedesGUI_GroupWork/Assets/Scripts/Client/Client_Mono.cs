using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using HttpProject_GroupWork;
using UnityEngine;

public class Client_Mono : MonoBehaviour
{
    public VideoGames_Data videoGamesData = new VideoGames_Data("juego1", "2024", "Gabriel");
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
        
        client.Request("GET",UrlManager.Instance.pathToSaveVideogameData,"localhost", headers,"", new VideoGames_Data());
    }
}
