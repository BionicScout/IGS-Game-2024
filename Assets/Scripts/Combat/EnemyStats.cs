using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float curHealth = 10;
    public int maxHealth = 10;
    public int move;
    public int power;
    public int defense;

    [SerializeField] HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealthBar(curHealth, maxHealth);

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(int dmg)
    {
        dmg -= defense;
        curHealth -= dmg;
        healthBar.UpdateHealthBar(curHealth, maxHealth);
    }
}
