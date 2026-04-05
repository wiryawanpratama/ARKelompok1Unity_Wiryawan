using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class APIManager : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI textDesc;
    public DescriptionManager descriptionManager; 

    public void GetData(string jenis, string komponen)
    {
        StartCoroutine(FetchData(jenis, komponen));
    }

    IEnumerator FetchData(string jenis, string komponen)
    {
        
        string url = "https://c289-182-10-130-155.ngrok-free.app/api/get-detail/"
                     + jenis + "/" + komponen;

        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("ngrok-skip-browser-warning", "true");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Data data = JsonUtility.FromJson<Data>(www.downloadHandler.text);

            // Kirim komponen + deskripsi ke DescriptionManager
            descriptionManager.ShowDescription(data.komponen, data.deskripsi);
        }
        else
        {
            descriptionManager.ShowDescription("Error", "Gagal ambil data");
            Debug.LogError("Error: " + www.error);
        }
    }
}

[System.Serializable]
public class Data
{
    public string komponen;
    public string key;
    public string deskripsi;
}