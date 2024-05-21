using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClientServerMessages : MonoBehaviour
{
    [SerializeField]
    GameObject clientMessage;

    [SerializeField]
    GameObject serverMessage;

    [SerializeField]
    TMP_Text client;

    [SerializeField]
    TMP_Text server;

    [SerializeField]
    GameObject notmodified;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeClientText(string text)
    {
        client.text = text;
    }

    public void ChangeServerText(string text)
    {
        server.text = text;
    }

    public void ToggleClientMessage()
    {
        clientMessage.SetActive(!clientMessage.activeSelf);
    }

    public void ToggleServerMessage()
    {
        serverMessage.SetActive(!serverMessage.activeSelf);
    }

    public void ShowNotModified()
    {
        notmodified.SetActive(true);
        StartCoroutine(HideNotModified());
    }

    private IEnumerator HideNotModified()
    {
        yield return new WaitForSeconds(2f);
        notmodified.SetActive(false);
    }

}
