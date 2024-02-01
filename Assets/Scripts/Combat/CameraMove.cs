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
    private float mapMinX;
    private float mapMinY;

    [SerializeField]
    private Camera cam;

    private Vector3 dragOrigin;

    private void Start()
    {
        zoom = cam.orthographicSize;

        Vector3 bottomLeft = GlobalVars.hexagonTile[new Vector3Int(0, 0, 0)].transform.position;
        mapMinX = bottomLeft.x;
        mapMinY = bottomLeft.y;

    }

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
            cam.transform.position += difference;
        }
    }


}
