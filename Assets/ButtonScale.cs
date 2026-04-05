using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleEffect : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler
{
    Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Mengecil saat ditekan
        transform.localScale = originalScale * 0.9f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Balik ke normal
        transform.localScale = originalScale;
    }
}