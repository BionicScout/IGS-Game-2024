
using UnityEngine;

public class Command {
    public Vector3Int startSpace;
    public Vector3Int moveSpace;
    public Vector3Int attackTile;

    public Command(Vector3Int startSpace, Vector3Int move) { 
        this.startSpace = startSpace;
        moveSpace = move; 
    }
}
