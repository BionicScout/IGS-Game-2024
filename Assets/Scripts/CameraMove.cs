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

        ////gets bottom and left side of map
        //Vector3 bottomLeft = GlobalVars.hexagonTile[new Vector3Int(0, 0, 0)].transform.position;
        //mapMinX = bottomLeft.x;
        //mapMinY = bottomLeft.y;

        ////gets top and right side of map
        //Vector3 TopRight = GlobalVars.hexagonTile[GlobalVars.topRightHex].transform.position;
        //mapMaxX = TopRight.x;
        //mapMaxY = TopRight.y;

    }

    private void LateUpdate()
    {
        //zooms in and out
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * zoomStep;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity, smoothTime);



        //checks the bounds and wont let the camera pass them
        //use Mathf.clamp
        //float xBounds = +cam.aspect / 2f;
        //if (cam.transform.position.x < xBounds)
        //{
        //    float cameraPos = xBounds;
        //}

        //float yBounds = +cam.orthographicSize / 2f;
        //if (cam.transform.position.y < yBounds)
        //{
        //    float cameraPos = yBounds;
        //}

        PanCamera();
    }

    private void PanCamera()
    {
        //moves camera when right click is held and moved
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.GetMouseButton(1))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference;
            cam.transform.position = ClampCam(cam.transform.position + difference);
        }

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
