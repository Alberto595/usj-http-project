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

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            pathToSaveVideogameData = Application.persistentDataPath + "/VideoGame_Data.abuela";
            pathToSaveUsersData = Application.persistentDataPath + "/Users_Data.mitia";
        }
        
        DontDestroyOnLoad(this);
    }
}
