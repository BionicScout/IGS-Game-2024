using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private float zoom;
    private float zoomStep = 4f;
    private float minZoom = 2f;
    private float maxZoom = 10f;
    private float velocity = 0f;
    private float smoothTime = 0.25f;

    [SerializeField]
    public SpriteRenderer spriteRenderer;

    private float mapMinX;
    private float mapMinY;
    private float mapMaxX;
    private float mapMaxY;

    private void Awake()
    {
        mapMinX = spriteRenderer.transform.position.x - spriteRenderer.bounds.size.x / 2f;
        mapMaxX = spriteRenderer.transform.position.x + spriteRenderer.bounds.size.x / 2f;

        mapMinY = spriteRenderer.transform.position.y - spriteRenderer.bounds.size.y / 2f;
        mapMaxY = spriteRenderer.transform.position.y + spriteRenderer.bounds.size.y / 2f;
    }

    private Vector3 dragOrigin;

    private void Start()
    {
        zoom = cam.orthographicSize;

    }

    private void LateUpdate()
    {

        //moves camera when right click is held and moved
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference;
            cam.transform.position = ClampCam(cam.transform.position + difference);
        }

        //zooms in and out
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * zoomStep;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity, smoothTime);

    }

    private Vector3 ClampCam(Vector3 targetPos)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPos.x, minX, maxX);
        float newY = Mathf.Clamp(targetPos.y, minY, maxY);

        return new Vector3(newX, newY, targetPos.z);
    }
}
