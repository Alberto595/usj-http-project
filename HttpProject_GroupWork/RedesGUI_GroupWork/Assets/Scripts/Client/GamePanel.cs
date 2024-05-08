using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : MonoBehaviour
{

    [SerializeField] private GameObject gamePrefab = null;

    private static GamePanel instance = null;
    public static GamePanel Instance
    {
        get { return instance; }
        private set { instance = value; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void UpdateGamePanel(List<VideoGames_Data> games)
    {
        //check if there are children and DESTROY THEM >:)
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in this.transform)
        {
            children.Add(child.gameObject);
        }
        
        foreach (GameObject child in children)
        {
            Destroy(child);
        }
        
        foreach (VideoGames_Data d in games)
        {
            GameObject newgame = GameObject.Instantiate(gamePrefab, this.transform);
            newgame.GetComponent<gameInfo>().UpdateGameInfo(d.name, d.developer, d.releaseYear);
        }
    }
    
}
