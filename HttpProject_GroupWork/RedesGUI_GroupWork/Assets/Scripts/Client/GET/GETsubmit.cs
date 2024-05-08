using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GETsubmit : MonoBehaviour
{
    [SerializeField]
    TMP_InputField[] bodyFields;

    [SerializeField]
    string bodyToSend;

    [SerializeField]
    TMP_InputField[] headerValueFields; //fields must be in order

    [SerializeField]
    string[] headerKeyFields; //fields must be in order

    [SerializeField]
    Dictionary<string,string> headersToSend;

    [SerializeField]
    Client_Mono client;

    public void SendGET()
    {
        foreach(TMP_InputField b in bodyFields)
        {
            bodyToSend += b.text;
        }

        for (int i = 0; i < headerKeyFields.Length; i++)
        {
            headersToSend.Add(headerKeyFields[i], headerValueFields[i].text);
        }

        VideoGames_Data gamedata = new VideoGames_Data("", "", "");
        client.AcceptRequestFromButton("GET", headersToSend, bodyToSend, gamedata);

        //re-initialize all data
        foreach (TMP_InputField t in bodyFields)
        {
            t.text = "";
        }
        bodyToSend = "";

        foreach (TMP_InputField t in headerValueFields)
        {
            t.text = "";
        }
        headersToSend = new Dictionary<string, string>();

    }

}
