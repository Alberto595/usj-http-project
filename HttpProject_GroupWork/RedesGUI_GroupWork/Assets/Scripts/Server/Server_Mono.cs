using System.Collections;
using System.Collections.Generic;
using HttpProject_GroupWork;
using UnityEngine;

public class Server_Mono : MonoBehaviour
{
    private Server server = new Server();
    // Start is called before the first frame update
    void Start()
    {
        server.ServerMethod();
    }
}
