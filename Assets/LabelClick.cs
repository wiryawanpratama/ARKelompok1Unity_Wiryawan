using UnityEngine;

public class LabelClick : MonoBehaviour
{
    public string jenis;
    public string komponen;
    public APIManager apiManager;

    public void OnClick()
    {
        Debug.Log("KEKLIK: " + komponen);
        apiManager.GetData(jenis, komponen);
    }
}