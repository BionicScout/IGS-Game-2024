using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TragetEnemy : MonoBehaviour
{
    private Renderer renderer; 
    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Debug.Log(hitInfo.collider.gameObject.name);
            }
        }
    }

    private void OnMouseEnter() 
    {
        renderer.material.color = Color.white;
    }
    private void OnMouseExit() 
    {
        renderer.material.color = Color.red;
    }
}
