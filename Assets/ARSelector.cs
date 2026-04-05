using UnityEngine;
using TMPro;
using System.Collections;

public class ARSelector : MonoBehaviour
{
    public GameObject modelTargetArduino;
    public GameObject modelTargetUltrasonic;
    public TMP_Dropdown dropdown;

    void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownChanged);
        // Tunggu Vuforia fully initialized dulu
        StartCoroutine(InitDefault());
    }

    IEnumerator InitDefault()
    {
        modelTargetArduino.SetActive(false);
        modelTargetUltrasonic.SetActive(false);
        yield return new WaitForSeconds(2f); // tunggu Vuforia init
        modelTargetArduino.SetActive(true);
    }

    void OnDropdownChanged(int index)
    {
        if (index == 0) StartCoroutine(SwitchToArduino());
        else if (index == 1) StartCoroutine(SwitchToUltrasonic());
    }

    IEnumerator SwitchToArduino()
    {
        modelTargetArduino.SetActive(false);
        modelTargetUltrasonic.SetActive(false);
        yield return new WaitForSeconds(1f); // delay lebih lama
        modelTargetArduino.SetActive(true);
    }

    IEnumerator SwitchToUltrasonic()
    {
        Debug.Log("Switch to ultrasonic START");
        modelTargetArduino.SetActive(false);
        modelTargetUltrasonic.SetActive(false);
        yield return new WaitForSeconds(1f);
        modelTargetUltrasonic.SetActive(true);
        Debug.Log("Ultrasonic active: " + modelTargetUltrasonic.activeSelf);
        Debug.Log("Arduino active: " + modelTargetArduino.activeSelf);
    }
}