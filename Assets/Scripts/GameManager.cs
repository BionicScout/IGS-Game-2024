using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Characters[] characters;
    public Characters curCharacter;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (characters.Length > 0 && curCharacter == null)
        {
            curCharacter = characters[0];
        }
    }

    private void SetCharacter(Characters character)
    {
        curCharacter = character;
    }
}
