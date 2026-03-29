using UnityEngine;
using TMPro;

public class DescriptionManager : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI textDesc;

    public void ShowDescription(string desc)
    {
        panel.SetActive(true);
        textDesc.text = desc;
    }

    public void HideDescription()
    {
        panel.SetActive(false);
    }
}