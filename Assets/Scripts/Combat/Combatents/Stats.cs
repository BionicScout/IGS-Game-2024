using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : ScriptableObject
{
    public string charName;
    public Sprite Sprite;

    public float curHealth = 10;
    public int maxHealth = 10;
    public int move;
    public int power;
    public int defense;
}
