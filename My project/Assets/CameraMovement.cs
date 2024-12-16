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
        if(Input.GetKey(KeyCode.W)) cameraPosition.y += cameraSpeed;
        if(Input.GetKey(KeyCode.S)) cameraPosition.y -= cameraSpeed;
        if(Input.GetKey(KeyCode.D)) cameraPosition.x += cameraSpeed;
        if(Input.GetKey(KeyCode.A)) cameraPosition.x -= cameraSpeed;
        
        transform.position = cameraPosition;

        if (Input.GetAxis("Mouse ScrollWheel") > 0) ZoomIn();
        if (Input.GetAxis("Mouse ScrollWheel") < 0) ZoomOut();
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
