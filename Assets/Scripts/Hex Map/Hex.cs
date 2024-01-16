using System.Collections.Generic;
using UnityEngine;

public class Hex {
    //Cube Coords of Hex Location
    public int q, r, s;
    public GameObject attachedObj;

    //Direction of Neighorbor Tiles
    List<Vector3Int> hex_directions = new List<Vector3Int> {
        new Vector3Int(1, 0, -1), new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1),
        new Vector3Int(-1, 0, 1), new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, -1)
    };

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

    //Axial Coord Constructor
    public Hex(int x , int y) {
        q = x; r = y; s = -x - y;
    }


/*********************************
    Operations Overrides
*********************************/
    public static bool operator ==(Hex a, Hex b) {
        if(a.q == b.q && a.r == b.r && a.s == b.s) {
            return true;
        }

        return false;
    }

    public static bool operator !=(Hex a , Hex b) {
        return !(a == b);
    }

    public static Hex operator +(Hex a , Hex b) {
        return new Hex(a.q + b.q , a.r + b.r , a.s + b.s);
    }

    public static Hex operator +(Hex a , Vector3Int b) {
        return new Hex(a.q + b.x , a.r + b.y , a.s + b.z);
    }

    public static Hex operator -(Hex a , Hex b) {
        return new Hex(a.q - b.q , a.r - b.r , a.s - b.s);
    }

    public static Hex operator *(Hex a , int k) {
        return new Hex(a.q * k , a.r * k , a.s * k);
    }

/*********************************
    Basic Operations (Distnace, Neighbors) 
*********************************/
    public int hex_length(Hex hex) {
        return (int)((Mathf.Abs(hex.q) + Mathf.Abs(hex.r) + Mathf.Abs(hex.s)) / 2.0f);
    }

    public int hex_distance(Hex a , Hex b) {
        return hex_length(a-b);
    }



    public Vector3Int hex_direction(int direction) {
        return hex_directions[direction % 6];
    }

    public Hex hex_neighbor(Hex hex , int direction) {
        return hex + hex_direction(direction);
    }








}
