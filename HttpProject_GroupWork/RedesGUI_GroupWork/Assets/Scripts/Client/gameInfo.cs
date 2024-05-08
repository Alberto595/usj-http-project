using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameInfo : MonoBehaviour
{
    [SerializeField]
    private TMP_Text name;
    [SerializeField]
    private TMP_Text dev;
    [SerializeField]
    private TMP_Text year;

    public void UpdateGameInfo(string n, string d, string y)
    {
        name.text = n;
        dev.text = d;
        year.text = y;
    }
    
}
