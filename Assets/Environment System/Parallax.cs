using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public static Parallax Instance;

    List<Tile> tiles = new();
    Stack<TileData> history = new();

    [Header("Tile Models")]
    [SerializeField] TileData ground;
    [SerializeField] TileData sky;
    [SerializeField] List<TileData> spaces;

    [Header("Settings")]
    [SerializeField] int limit = 3;

    [SerializeField] float maxHeight = 500f;    //Placeholder Rocket Max Height

    public Rocket rocket;

    private FlightState state = FlightState.Idle;


    private float start;
    private float next;

    private bool started;

    void Awake() => Instance = this;

    void Start()
    {
        next = start;

        SpawnTile(ground);
        SpawnTile(sky);

        for (int i = 0; i < limit - 2; i++) { SpawnTile(GetRandomSpace()); }
    }

    public void Play()
    {
        if (started) { return; }

        started = true;
        state = FlightState.Flying;
    }

    public void Stop()
    {
        if (!started) return;

        started = false;
        state = FlightState.Landed;
    }

    void Update()
    {
        switch (state)
        {
            case FlightState.Idle:
                break;

            case FlightState.Flying:
                MoveAscend();
                break;

            case FlightState.Landed:
                break;
        }
    }

    void MoveAscend()
    {
        Vector3 move = Vector3.down * rocket.velocity * Time.deltaTime;

        foreach (Tile tile in tiles)
        {
            tile.gameObject.transform.position += move;
        }

        UpdateTilePositions();
    }
    void UpdateTilePositions()
    {
        float lowerLimit = -30f;

        foreach (Tile tile in tiles)
        {
            if (tile.gameObject.transform.position.y < lowerLimit)
            {
                TileToTop(tile);
                break;
            }
        }
    }

    void TileToTop(Tile tile)
    {
        float highest = GetHighest();
        float height = tile.adapter.GetHeight();

        TileData data = GetRandomSpace();
        history.Push(data);

        tile.adapter.SetData(data);
        tile.gameObject.transform.position = new Vector3(0, highest + height, 0);
    }

    void SpawnTile(TileData data)
    {
        GameObject tile = Instantiate(data.gameObject, transform);
        if (!tile.TryGetComponent(out TileAdapter adapter)) { adapter = tile.AddComponent<TileAdapter>(); }
        adapter.SetData(data);

        tile.transform.position = new Vector3(0, next, 0);
        next += adapter.GetHeight();

        tiles.Add(new Tile { gameObject = tile, adapter = adapter });
        if (data.type == TileType.Space) { history.Push(data); }
    }

    TileData GetRandomSpace()
    {
        return spaces[Random.Range(0, spaces.Count)];
    }

    float GetHighest()
    {
        float highest = float.MinValue;
        foreach (Tile tile in tiles) { if (tile.gameObject.transform.position.y > highest) { highest = tile.gameObject.transform.position.y; } }
        return highest;
    }
}
