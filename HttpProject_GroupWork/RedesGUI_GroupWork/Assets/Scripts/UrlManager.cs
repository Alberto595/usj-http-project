using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlManager : MonoBehaviour
{
    private static UrlManager instance;
    public static UrlManager Instance {
        get{ return instance; }
        private set { instance = value; }
    }

    public string pathToSaveVideogameData;
    public string pathToSaveUsersData;

    private static List<string> files;
    public static List<string> Files { get { return files; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            pathToSaveVideogameData = Application.dataPath + "/PS5.data";
            pathToSaveUsersData = Application.dataPath + "/Users_Data.data";
        }
        
        DontDestroyOnLoad(this);

        files = new List<string> { "PS5", "Xbox", "Switch" };
        

    }

    public void ChangePath(string newFile)
    {
        pathToSaveVideogameData = Application.dataPath + "/" + newFile + ".data";

    }

}
