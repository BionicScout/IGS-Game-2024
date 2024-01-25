using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public BoxCollider2D range;
    private Transform enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        range.edgeRadius = 2;
        if (Vector2.Distance(enemy.position, transform.position) <= range.edgeRadius) 
        {
           Debug.Log("Enemy was attacked");
        }

        //animation and effecting enemy health

    }
}
