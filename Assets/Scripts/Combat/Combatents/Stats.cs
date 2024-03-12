using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "XXXStats" , menuName = "ScriptableObjects/UnitStats" , order = 2)]
public class Stats : ScriptableObject
{
    public string charName;
    public string charType;
    public Sprite sprite;
    public int charLevel;

    public float curHealth = 10;
    public int maxHealth = 10;
    public int move;
    public int attackRange;
    public int power;
    public int defense;

    public void Start()
    {
        curHealth = maxHealth;
    }

    public int Damage(int dmg)
    {
        dmg -= defense;
        curHealth -= dmg;
        return dmg;
    }
    public void Heal(int heal)
    {
        if (curHealth + heal >= 10)
        {
            curHealth = maxHealth;
        }
        curHealth += heal;

    }

    public Stats Copy() {
        Stats copy = CreateInstance<Stats>();

        copy.charName = charName;
        copy.charType = charType;
        copy.sprite = sprite;
        copy.charLevel = charLevel;

        copy.curHealth = curHealth;
        copy.maxHealth = maxHealth;
        copy.move = move;
        copy.attackRange = attackRange;
        copy.power = power;
        copy.defense = defense;

        return copy;
    }
}
