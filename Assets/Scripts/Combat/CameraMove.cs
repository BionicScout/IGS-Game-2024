using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private float zoom;
    private float zoomStep = 4f;
    private float minZoom = 2f;
    private float maxZoom = 10f;
    private float velocity = 0f;
    private float smoothTime = 0.25f;

    [SerializeField]
    private Camera cam;

    //[SerializeField]
    //private SpriteRenderer mapRenderer;

    //private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    private Vector3 dragOrigin;

    private void Start()
    {
        zoom = cam.orthographicSize;
    }
    //private void Awake()
    //{
    //    mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
    //    mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;

    //    mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
    //    mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
    //}

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * zoomStep;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity, smoothTime);

        PanCamera();
    }

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.GetMouseButton(1))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            //cam.transform.position = ClampCamera(cam.transform.position + difference);
            cam.transform.position += difference;
        }
    }

    //public void ZoomIn()
    //{
    //    float newSize = cam.orthographicSize + zoomStep;
    //    cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

    //    //cam.transform.position = ClampCamera(cam.transform.position);
    //}    
    //public void ZoomOut()
    //{
    //    float newSize = cam.orthographicSize - zoomStep;
    //    cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

    //    //cam.transform.position = ClampCamera(cam.transform.position);
    //}

    //private Vector3 ClampCamera(Vector3 targetPosition)
    //{
    //    float camHeight = cam.orthographicSize;
    //    float camWidth = cam.orthographicSize * cam.aspect;

    //    float minX = mapMinX + camWidth;
    //    float maxX = mapMaxX - camWidth;

    //    float minY = mapMinY + camHeight;
    //    float maxY = mapMaxY - camHeight;

    //    float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
    //    float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

    //    return new Vector3(newX, newY, targetPosition.z);
    //}
}
