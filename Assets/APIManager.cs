using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class APIManager : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI textDesc;

    public void GetData(string jenis, string komponen)
    {
        StartCoroutine(FetchData(jenis, komponen));
    }

    IEnumerator FetchData(string jenis, string komponen)
    {
        panel.SetActive(true);
        string url = "https://8af4-182-10-129-245.ngrok-free.app/api/get-detail/" + jenis + "/" + komponen;

        UnityWebRequest www = UnityWebRequest.Get(url);

        // Tambahkan baris ini agar ngrok skip halaman warning
        www.SetRequestHeader("ngrok-skip-browser-warning", "true");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Data data = JsonUtility.FromJson<Data>(www.downloadHandler.text);
            textDesc.text = data.deskripsi;
        }
        else
        {
            textDesc.text = "Gagal ambil data";
            Debug.LogError("Error: " + www.error);
            Debug.LogError("Response: " + www.downloadHandler.text);
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