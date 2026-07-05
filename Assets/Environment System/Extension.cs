using UnityEngine;

public enum FlightState { Idle, Flying, Falling, Landed }

public enum TileType { Ground, Sky, Space }

public class Tile
{
    public GameObject gameObject;
    public TileAdapter adapter;
}

[System.Serializable]
public class TileData
{
    public int id;
    public string name;
    public TileType type;
    public GameObject gameObject;
    public Color color;
}