using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownScript : MonoBehaviour
{
    private TMP_Text selectedFile;

    private void Awake()
    {
        selectedFile = GetComponent<TMP_Text>();
    }

    public void UpdateGlobalPath()
    {
        UrlManager.Instance.ChangePath(selectedFile.text);
    }

}
