using UnityEngine;

public class LineConnector : MonoBehaviour
{
    public RectTransform startPoint; // Canvas pakai RectTransform
    public Transform endPoint;

    LineRenderer line;

    void Start()
    {
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = 0.002f;
        line.endWidth = 0.001f;
        line.startColor = Color.white;
        line.endColor = Color.yellow;
        line.material = new Material(Shader.Find("Unlit/Color"));
        line.positionCount = 2;
        line.useWorldSpace = true; // ← PENTING
    }

    void Update()
    {
        if (startPoint != null && endPoint != null)
        {
            line.SetPosition(0, startPoint.position);
            line.SetPosition(1, endPoint.position);
        }
    }
}