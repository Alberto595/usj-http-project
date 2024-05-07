using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GETsubmit : MonoBehaviour
{
    [SerializeField]
    TMP_Text[] bodyFields;

    [SerializeField]
    string bodyToSend;

    [SerializeField]
    TMP_Text[] headerValueFields; //fields must be in order

    [SerializeField]
    string[] headerKeyFields; //fields must be in order

    [SerializeField]
    Dictionary<string,string> headersToSend;

    [SerializeField]
    Client_Mono client;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendGET()
    {
        foreach(TMP_Text b in bodyFields)
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
        bodyToSend = "";
        headersToSend = new Dictionary<string, string>();

    }

}
