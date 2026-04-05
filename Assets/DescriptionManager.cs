using UnityEngine;
using TMPro;

public class DescriptionManager : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI textDesc;
    public TextMeshProUGUI TitleText;

    void Start()
    {
        panel.SetActive(false); 
    }

    public void ShowDescription(string komponen, string desc)
    {
        
        TitleText.text = komponen;
        textDesc.text = desc;
        panel.SetActive(true);
    }

    public void HideDescription()
    {
        panel.SetActive(false);
    }
}