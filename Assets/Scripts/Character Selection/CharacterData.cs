using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterData : ScriptableObject
{
    public Characters[] character;

    public int CharacterCount
    {
        get
        {
            return character.Length;
        }
    }

    public Characters GetCharacter(int index)
    {
        return character[index];
    }
}
