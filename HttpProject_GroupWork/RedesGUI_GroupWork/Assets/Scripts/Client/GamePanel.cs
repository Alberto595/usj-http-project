using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GamePanel : MonoBehaviour
{

    [SerializeField] private GameObject gamePrefab = null;
    [SerializeField] private GameObject panelContent = null;

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

        foreach (Transform child in panelContent.transform)
        {
            children.Add(child.gameObject);
        }
        
        foreach (GameObject child in children)
        {
            Destroy(child);
        }
        
        foreach (VideoGames_Data d in games)
        {
            GameObject newgame = GameObject.Instantiate(gamePrefab, panelContent.transform);
            newgame.GetComponent<gameInfo>().UpdateGameInfo(d.name, d.developer, d.releaseYear);
        }
    }
    public void UpdateGamePanel(List<Users_Data> users)
    {
        //check if there are children and DESTROY THEM >:)
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in panelContent.transform)
        {
            children.Add(child.gameObject);
        }
        
        foreach (GameObject child in children)
        {
            Destroy(child);
        }
        
        foreach (Users_Data d in users)
        {
            GameObject newUser = GameObject.Instantiate(gamePrefab, panelContent.transform);
            newUser.GetComponent<gameInfo>().UpdateGameInfo(d.userName, d.password, d.verificationToken);
        }
    }
    
}
