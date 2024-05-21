using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Random = System.Random;
using UnityEngine;

public class LogInAction : MonoBehaviour
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

    protected string verb1 = "PUT";
    protected string verb2 = "GET";
    protected string verb3 = "POST";
    protected string connectionToken;
    protected string userName = "";

    public Users_Data CreateUserData()
    {
        connectionToken = GenerateCode(4);
        userName = bodyFields[0].text;
        return new Users_Data(userName, bodyFields[1].text, connectionToken);
    }

    public void RequestLogIn()
    {
        client.client.login = "1";
        if (SendVerbRequest(verb2))
        {
            client.client.userName = userName;
            client.client.verificationCode = connectionToken;
        }
        client.client.login = "";
    }
    public void RequestSigIn()
    {
        client.client.login = "1";
        if (SendVerbRequest(verb3))
        {
            client.client.userName = bodyFields[0].text;
            client.client.verificationCode = connectionToken;
        }
        client.client.login = "";
    }
    public bool SendVerbRequest(string verb)
    {
        foreach (TMP_InputField b in bodyFields)
        {
            bodyToSend += b.text;
        }

        for (int i = 0; i < headerKeyFields.Length; i++)
        {
            headersToSend.Add(headerKeyFields[i], headerValueFields[i].text);
        }

        Users_Data userData = CreateUserData();

        if(userData.IsEmpty() && verb == "PUT")
        {
            ReinitializeData();
            return false;
        }

        client.AcceptRequestFromButton(verb, headersToSend, bodyToSend, userData);

        ReinitializeData();
        return true;
    }

    private void ReinitializeData()
    {
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
    
    public string GenerateCode(int p_CodeLength)
    {
        string result = ""; 
 
        string pattern = "+-_#!?0123456789abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
            
        Random myRndGenerator = new Random((int)DateTime.Now.Ticks);

        for(int i=0; i < p_CodeLength; i++)
        {
            int mIndex = myRndGenerator.Next(pattern.Length);
            result += pattern[mIndex];
        }

        return result;
    }



}
