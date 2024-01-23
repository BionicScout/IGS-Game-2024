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
    public TragetEnemy targetEnemy;

    bool shootMode = false;

    public void Start()
    {
        //enemy = GameObject.FindGameObjectWithTag("Enemy");
        

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, enemy.transform.position);
    }

    public void ReadyToShoot(){
        shootMode = true;
    }

    public void OnMouseDown()
    {
        if(shootMode == true) 
        {
            targetEnemy.OnMouseDown();
        }
    }

    public void Shoot()
    {
        Instantiate(arrowPrefab, shootingPoint.position, Quaternion.identity);
    }
}
