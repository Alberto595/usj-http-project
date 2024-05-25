using UnityEngine;
using UnityEngine.UI;

public class VerbButtons : MonoBehaviour
{
    [SerializeField]
    GameObject[] verbInputs;

    [SerializeField]
    Button[] verbButtons;

    [SerializeField]
    GameObject closeButton;

    [SerializeField]
    Sprite unselected;
    [SerializeField]
    Sprite selected;

    public void ActivateVerb(int i)
    {
        //desactivate all
        foreach(GameObject go in verbInputs)
        {
            go.SetActive(false);
        }

        //set all buttons to unselected
        foreach(Button b in verbButtons)
        {
            b.image.sprite = unselected;
        }

        //activate verb if one selected
        if (i >= 0)
        {
            closeButton.SetActive(true);
            verbInputs[i].SetActive(true);
            verbButtons[i].image.sprite = selected;
        }
        else
        {
            closeButton.SetActive(false);
        }
    }

}
