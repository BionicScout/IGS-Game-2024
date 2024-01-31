using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        GetPosition();
    }
    public void GetPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.transform.position);
                return hit.collider.gameObject.transform.position;
            }
        }
    }

}

