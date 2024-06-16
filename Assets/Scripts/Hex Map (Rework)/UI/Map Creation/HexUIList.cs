using UnityEngine;

public class HexUIList : MonoBehaviour, IMapListSelection<SO_TileInfo> {
    [ReadOnly, SerializeField]
    private string _type;

    [ReadOnly, SerializeField]
    private SO_TileInfo _data;

    public string type { get { return _type; } set { _type = value; } }
    public SO_TileInfo data { get { return _data; } set { _data = value;  } }
}
