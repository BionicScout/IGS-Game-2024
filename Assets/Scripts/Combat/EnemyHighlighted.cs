using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHighlighted : MonoBehaviour
{
    private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        renderer.material.color = Color.grey;
    }
    private void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }
}
