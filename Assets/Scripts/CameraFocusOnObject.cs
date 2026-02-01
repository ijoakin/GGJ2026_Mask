using UnityEngine;

public class CameraFocusOnObject : MonoBehaviour
{
    public Transform objectToView;
    public float focusDistance = 3f;
    public float focusSpeed = 2f;

    private Transform cam;
    private bool isFocusing = false;
    private Vector3 originalPos;
    private Quaternion originalRot;

    void Start()
    {
        cam = Camera.main.transform;
        originalPos = cam.position;
        originalRot = cam.rotation;
    }

    void Update()
    {
        if (isFocusing)
        {
            // Posición ideal de la cámara según el tamaño del objeto
            Bounds bounds = CalculateBounds(objectToView.gameObject);

            Vector3 direction = (cam.position - objectToView.position).normalized;
            Vector3 targetPos = objectToView.position + direction * focusDistance;

            cam.position = Vector3.Lerp(cam.position, targetPos, Time.deltaTime * focusSpeed);
            cam.LookAt(objectToView.position);
        }
    }

    public void StartFocus()
    {
        isFocusing = true;
    }

    public void StopFocus()
    {
        isFocusing = false;
        cam.position = originalPos;
        cam.rotation = originalRot;
    }

    // Calcula tamaño del objeto para encuadrarlo correctamente
    Bounds CalculateBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(renderers[0].bounds.center, Vector3.zero);

        foreach (Renderer r in renderers)
            bounds.Encapsulate(r.bounds);

        return bounds;
    }
}