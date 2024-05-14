using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELETEsubmit : VerbSubmit
{
    private void Awake()
    {
        verb = "DELETE";
    }

    public override VideoGames_Data CreateGameInfo()
    {
        return new VideoGames_Data("", "", "");
    }
}
