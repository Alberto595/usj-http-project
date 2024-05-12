using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerbButtons : MonoBehaviour
{
    [SerializeField]
    GameObject[] verbInputs;

    [SerializeField]
    GameObject closeButton;

    public void ActivateVerb(int i)
    {
        //deactivate all
        foreach(GameObject go in verbInputs)
        {
            go.SetActive(false);
        }

        //activate verb if one selected
        if (i >= 0)
        {
            closeButton.SetActive(true);
            verbInputs[i].SetActive(true);
        }
        else
        {
            closeButton.SetActive(false);
        }
    }

}
