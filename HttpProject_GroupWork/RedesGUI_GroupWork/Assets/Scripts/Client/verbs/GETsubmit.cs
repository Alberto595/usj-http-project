using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GETsubmit : VerbSubmit
{
    private void Awake()
    {
        verb = "GET";
    }

    public override VideoGames_Data CreateGameInfo()
    {
        return new VideoGames_Data("", "", "");
    }

}
