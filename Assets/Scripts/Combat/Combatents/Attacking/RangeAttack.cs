using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.InputSystem;

public class RangeAttack : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject arrowPrefab;
    private GameObject enemy;

    bool shootMode = false;

    public void Start()
    {
        //enemy = GameObject.FindGameObjectWithTag("Enemy");
        /*So what you need to do in the RangeAttack script, when the input button is pressed 
        then you need to send a ray cast from the mouse to the object on screen (which is the enemy). 
        The enemy needs to have a collider on it. 
        This raycast should get the GameObject, which you can get the position from.*/

    }

    // Update is called once per frame
    void Update()
    {
        //float distance = Vector2.Distance(transform.position, enemy.transform.position);
    }

    public void ReadyToShoot()
    {
        shootMode = true;
    }

    public void OnMouseDown()
    {
        if (shootMode == true)
        {
            //targetEnemy.OnMouseDown();
        }
    }

    public void Shoot()
    {
        Instantiate(arrowPrefab, shootingPoint.position, Quaternion.identity);
    }
}
