using UnityEngine;
using UnityEngine.Assertions;

public class CameraMovement : MonoBehaviour
{
    private Vector3 cameraPosition;
    [Header("Camera Settings")]
    public float cameraSpeed = 0.1f;

    [SerializeField]
    private Camera cameraFreeWalk;
    public float zoomSpeed = 1.1f;
    public float minZoomFOV = 10f;
    public float maxZoomFOV = 160f;

    private void Awake()
    {
        cameraFreeWalk = GetComponent<Camera>();
        Assert.IsNotNull(cameraFreeWalk);
        Assert.IsFalse(cameraFreeWalk.orthographic, "There isn't a FOV on an orthographic camera.");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        cameraPosition = transform.position;
    }

    // Update is called once per frame
    public void Update()
    {
        var camSpeed = Input.GetKey(KeyCode.LeftShift) ? cameraSpeed * 3 : cameraSpeed;

        if (Input.GetKey(KeyCode.W)) cameraPosition.y += camSpeed;
        if(Input.GetKey(KeyCode.S)) cameraPosition.y -= camSpeed;
        if(Input.GetKey(KeyCode.D)) cameraPosition.x += camSpeed;
        if(Input.GetKey(KeyCode.A)) cameraPosition.x -= camSpeed;
        
        transform.position = cameraPosition;

        var zmSpeed = Input.GetKey(KeyCode.LeftShift) ? Mathf.Sqrt(Mathf.Sqrt(zoomSpeed)) : zoomSpeed;
        if (Input.GetAxis("Mouse ScrollWheel") > 0) Zoom(1 / zmSpeed);
        if (Input.GetAxis("Mouse ScrollWheel") < 0) Zoom(zmSpeed);
    }

    public void Zoom(float zoom)
    {
        cameraFreeWalk.fieldOfView *= zoom;
        if (cameraFreeWalk.fieldOfView < minZoomFOV) cameraFreeWalk.fieldOfView = minZoomFOV;
        if (cameraFreeWalk.fieldOfView > maxZoomFOV) cameraFreeWalk.fieldOfView = maxZoomFOV;
    }

    public void ZoomIn()
    {
        cameraFreeWalk.fieldOfView /= zoomSpeed;
        if (cameraFreeWalk.fieldOfView < minZoomFOV) cameraFreeWalk.fieldOfView = minZoomFOV;
    }

    public void ZoomOut()
    {
        cameraFreeWalk.fieldOfView *= zoomSpeed;
        if (cameraFreeWalk.fieldOfView > maxZoomFOV) cameraFreeWalk.fieldOfView = maxZoomFOV;
    }
}
