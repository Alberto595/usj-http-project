using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEADsubmit : VerbSubmit
{
    private void Awake()
    {
        verb = "HEAD";
    }

    public override VideoGames_Data CreateGameInfo()
    {
        return new VideoGames_Data("", "", "");
    }
}
