using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int curHealth = 10;
    public int maxHealth = 10;
    public int move;
    public int power;
    public int defense;
    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Damage(int dmg)
    {
        dmg -= defense;
        curHealth -= dmg;
    }
}
