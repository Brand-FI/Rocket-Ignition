using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public static Parallax Instance;

    [Header("Tile Models")]
    [SerializeField] TileData ground;
    [SerializeField] TileData sky;
    [SerializeField] List<TileData> spaces;

    [Header("Settings")]
    [SerializeField] int limit = 3;
    [SerializeField] Rocket rocket;

    List<Tile> tiles = new();

    FlightState state = FlightState.Stay;

    float start = 0;
    float next;
    bool started;

    void Awake() => Instance = this;

    void Start()
    {
        next = start;
        SpawnTile(ground);
        SpawnTile(sky);
        while (tiles.Count < limit + 2) { SpawnSpaceTile(); }
    }

    public void Play()
    {
        if (started) { return; }
        started = true;
        state = FlightState.Flying;
    }

    public void Stop()
    {
        if (!started) { return; }
        started = false;
        state = FlightState.Stay;
    }

    void Update()
    {
        if (state == FlightState.Flying) { MoveAscend(); }
    }

    void MoveAscend()
    {
        Vector3 move = Vector3.down * rocket.velocity * Time.deltaTime;
        foreach (Tile tile in tiles) { tile.gameObject.transform.position += move; }
        UpdateTilePositions();
    }

    void UpdateTilePositions()
    {
        float lowerLimit = -30f;
        for (int i = tiles.Count - 1; i >= 0; i--)
        {
            Tile tile = tiles[i];
            if (tile.gameObject.transform.position.y < lowerLimit)
            {
                tiles.RemoveAt(i);
                Destroy(tile.gameObject);
                SpawnSpaceTile();
            }
        }
    }

    void SpawnSpaceTile()
    {
        TileData data = RandomSpaceModel();
        GameObject obj = Instantiate(data.gameObject, transform);
        if (!obj.TryGetComponent(out TileAdapter adapter)) { adapter = obj.AddComponent<TileAdapter>(); }
        adapter.SetData(data);
        float y = GetHighest() + adapter.GetHeight();
        obj.transform.position = new Vector3(0, y, 0);
        tiles.Add(new Tile { gameObject = obj, adapter = adapter });
    }

    void SpawnTile(TileData data)
    {
        GameObject obj = Instantiate(data.gameObject, transform);
        if (!obj.TryGetComponent(out TileAdapter adapter)) { adapter = obj.AddComponent<TileAdapter>(); }
        adapter.SetData(data);
        obj.transform.position = new Vector3(0, next, 0);
        next += adapter.GetHeight();
        tiles.Add(new Tile { gameObject = obj, adapter = adapter });
    }

    float GetHighest()
    {
        if (tiles.Count == 0) { return start; }
        float highest = tiles[0].gameObject.transform.position.y;
        foreach (Tile tile in tiles) { if (tile.gameObject.transform.position.y > highest) { highest = tile.gameObject.transform.position.y; } }
        return highest;
    }

    TileData RandomSpaceModel() => spaces[Random.Range(0, spaces.Count)];
}
