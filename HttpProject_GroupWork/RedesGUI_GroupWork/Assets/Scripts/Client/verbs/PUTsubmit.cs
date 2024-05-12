using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUTsubmit : VerbSubmit
{
    private void Awake()
    {
        verb = "PUT";
    }
    public override VideoGames_Data CreateGameInfo()
    {
        bodyToSend = "";
        return new VideoGames_Data(bodyFields[0].text, bodyFields[1].text, bodyFields[2].text);
    }
}
