using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TragetEnemy : MonoBehaviour
{
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseDown()
    {
         enemy.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

}
