using UnityEngine;

public class ARLabel : MonoBehaviour
{
    public Transform targetPoint;
    public Transform labelCanvas;
    public Camera arCamera;

    public Color warnaGaris = Color.white;
    public Color warnaDot = Color.red;
    public float lebarGaris = 0.001f;
    public float ukuranDot = 0.008f;

    private LineRenderer lr;
    private GameObject dot;

    void Start()
    {
        // Buat garis
        GameObject objGaris = new GameObject("Line");
        objGaris.transform.SetParent(this.transform);
        lr = objGaris.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = warnaGaris;
        lr.endColor = warnaGaris;
        lr.startWidth = lebarGaris;
        lr.endWidth = lebarGaris;
        lr.positionCount = 2;
        lr.useWorldSpace = true;

        // Buat dot
        dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        dot.name = "Dot";
        dot.transform.SetParent(this.transform);
        dot.transform.localScale = Vector3.one * ukuranDot;
        Renderer r = dot.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Sprites/Default"));
        r.material.color = warnaDot;
        Destroy(dot.GetComponent<SphereCollider>());
    }

    void Update()
    {
        if (targetPoint == null || labelCanvas == null) return;

        Vector3 start = labelCanvas.position;
        Vector3 end = targetPoint.position;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        dot.transform.position = end;

        if (arCamera != null)
        {
            labelCanvas.LookAt(arCamera.transform);
            labelCanvas.Rotate(0, 180f, 0);
        }
    }
}