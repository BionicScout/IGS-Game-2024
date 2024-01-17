using System.Collections.Generic;
using UnityEngine;

public class Hex {
    //Cube Coords of Hex Location
    public int q, r, s;
    public GameObject attachedObj;

    //Direction of Neighorbor Tiles
    static List<Vector3Int> hex_directions = new List<Vector3Int> {
        new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, -1), new Vector3Int(1, 0, -1), 
        new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1), new Vector3Int(-1, 0, 1)
    };

    public static Dictionary<Vector3Int, Hex> neighbors = new Dictionary<Vector3Int, Hex>();

    /*********************************
        Constructors 
    *********************************/

    //Cube Coord Constructor
    public Hex(int x, int y, int z) {
        if(x + y + z != 0) {
            Debug.LogWarning("Cube Coordantes do not equal 0");
        }

        q = x; r = y; s = z;
    }

    public Hex(Vector3Int coords) {
        if(coords.x + coords.y + coords.z != 0) {
            Debug.LogWarning("Cube Coordantes do not equal 0");
        }

        q = coords.x;
        r = coords.y;
        s = coords.z;
    }

    //Axial Coord Constructor
    public Hex(int x , int y) {
        q = x; r = y; s = -x - y;
    }


/*********************************
    Operations Overrides
*********************************/

//Equal and Not Equal
    public static bool operator ==(Hex a, Hex b) {
        if(a.q == b.q && a.r == b.r && a.s == b.s) {
            return true;
        }

        return false;
    }

    public static bool operator !=(Hex a , Hex b) {
        return !(a == b);
    }

//Addtion 
    public static Hex operator +(Hex a , Hex b) {
        Vector3Int pos = new Vector3Int(a.q + b.q , a.r + b.r , a.s + b.s);
        return GlobalVars.hexList[pos];
    }

    public static Hex operator +(Hex a , Vector3Int b) {
        Vector3Int pos = new Vector3Int(a.q + b.x , a.r + b.y , a.s + b.z);
        return GlobalVars.hexList[pos];
    }
    public static Hex operator +(Vector3Int b, Hex a) {
        Vector3Int pos = new Vector3Int(a.q + b.x , a.r + b.y , a.s + b.z);
        return GlobalVars.hexList[pos];
    }

//Subtraction 
    public static Hex operator -(Hex a , Hex b) {
        Vector3Int pos = new Vector3Int(a.q - b.q , a.r - b.r , a.s - b.s);
        return GlobalVars.hexList[pos];
    }

    public static Hex operator -(Hex a , Vector3Int b) {
        Vector3Int pos = new Vector3Int(a.q - b.x , a.r - b.y , a.s - b.z);
        return GlobalVars.hexList[pos];
    }

    public static Hex operator -(Vector3Int a, Hex b) {
        Vector3Int pos = new Vector3Int(a.x - b.q, a.y - b.r, a.z - b.s);
        return GlobalVars.hexList[pos];
    }

//Multiplication
    public static Hex operator *(Hex a , int k) {
        Vector3Int pos = new Vector3Int(a.q * k , a.r * k , a.s * k);
        return GlobalVars.hexList[pos];
    }

/*********************************
    Basic Operations (Distnace, Neighbors) 
*********************************/
    public int hex_distance(Hex other) {
        Hex temp = new Vector3Int(q, r, s) - other;
        return (int)((Mathf.Abs(temp.q) + Mathf.Abs(temp.r) + Mathf.Abs(temp.s)) / 2.0f);
    }


    public Hex hex_neighbor(int direction) {
        return neighbors[new Vector3Int(q , r , s) + hex_direction(direction)];
    }

    /*********************************
        Global Operations (Distnace, Neighbors) 
    *********************************/
    //public int hex_length(Hex hex) {
    //    return (int)((Mathf.Abs(hex.q) + Mathf.Abs(hex.r) + Mathf.Abs(hex.s)) / 2.0f);
    //}

    public int hex_distance(Hex a , Hex b) {
        Hex temp = a - b;
        return (int)((Mathf.Abs(temp.q) + Mathf.Abs(temp.r) + Mathf.Abs(temp.s)) / 2.0f);
    }

    public Vector3Int hex_direction(int direction) {
        return hex_directions[direction % 6];
    }

    public Hex hex_neighbor(Hex hex , int direction) {
        return hex + hex_direction(direction);
    }





}
