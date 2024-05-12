using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class VerbSubmit : MonoBehaviour
{
    [SerializeField]
    protected GameObject sentBanner;

    [SerializeField]
    protected TMP_InputField[] bodyFields;

    [SerializeField]
    protected string bodyToSend;

    [SerializeField]
    protected TMP_InputField[] headerValueFields; //fields must be in order

    [SerializeField]
    protected string[] headerKeyFields; //fields must be in order

    [SerializeField]
    protected Dictionary<string, string> headersToSend;

    [SerializeField]
    protected Client_Mono client;

    protected string verb = "";

    public abstract VideoGames_Data CreateGameInfo();
    public void SendVerbRequest()
    {
        foreach (TMP_InputField b in bodyFields)
        {
            bodyToSend += b.text;
        }

        for (int i = 0; i < headerKeyFields.Length; i++)
        {
            headersToSend.Add(headerKeyFields[i], headerValueFields[i].text);
        }

        VideoGames_Data gamedata = CreateGameInfo();
        client.AcceptRequestFromButton(verb, headersToSend, bodyToSend, gamedata);

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

        //create animation of request sent
        sentBanner.GetComponent<SentBanner>().Appear();

    }

    


}