using UnityEngine;

public class CameraRTSController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float edgeSize = 10f;
    public float zoomSpeed = 500f;
    public float minY = 10f;
    public float maxY = 30f;

    public Vector2 mapLimitsX = new Vector2(115, 195);
    public Vector2 mapLimitsZ = new Vector2(7, 87);

    private void Start()
    {
        // Forzar altura inicial correcta
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(40f, minY, maxY); // O simplemente pos.y = 30;
        transform.position = pos;
    }

    private void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - edgeSize)
            pos.z += moveSpeed * Time.deltaTime;
        if (Input.GetKey("s") || Input.mousePosition.y <= edgeSize)
            pos.z -= moveSpeed * Time.deltaTime;
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - edgeSize)
            pos.x += moveSpeed * Time.deltaTime;
        if (Input.GetKey("a") || Input.mousePosition.x <= edgeSize)
            pos.x -= moveSpeed * Time.deltaTime;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * zoomSpeed * Time.deltaTime;

        // Aplicar límites
        pos.x = Mathf.Clamp(pos.x, mapLimitsX.x, mapLimitsX.y);
        pos.z = Mathf.Clamp(pos.z, mapLimitsZ.x, mapLimitsZ.y);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
