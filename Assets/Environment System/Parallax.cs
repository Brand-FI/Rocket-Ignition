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
    [SerializeField] float speed = 8f;  //Placeholder Rocket Speed
    [SerializeField] float gravity = 20f;
    [SerializeField] float maxHeight = 500f;    //Placeholder Rocket Max Height


    private FlightState state = FlightState.Idle;


    private float start;
    private float next;

    private float currentHeight;

    private float velocity;
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
        if (!started) { return; }

        started = false;
        state = FlightState.Falling;
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

            case FlightState.Falling:
                MoveFall();
                break;

            case FlightState.Landed:
                break;
        }
    }

    void MoveAscend()
    {
        Vector3 move = Vector3.down * speed * Time.deltaTime;

        foreach (Tile tile in tiles)
        {
            tile.gameObject.transform.position += move;
        }

        currentHeight += speed * Time.deltaTime;

        UpdateTilePositions();

        if (currentHeight >= maxHeight)
        {
            velocity = 0;
            state = FlightState.Falling;
        }
    }

    void MoveFall()
    {
        velocity += gravity * Time.deltaTime;

        Vector3 move = Vector3.up * velocity * Time.deltaTime;

        foreach (Tile tile in tiles)
        {
            tile.gameObject.transform.position += move;
        }

        currentHeight -= velocity * Time.deltaTime;

        if (currentHeight <= 0)
        {
            currentHeight = 0;
            state = FlightState.Landed;
        }

        UpdateTilePositions();
    }

    void UpdateTilePositions()
    {
        float upperLimit = 30f;
        float lowerLimit = -30f;

        foreach (Tile tile in tiles)
        {
            if (state == FlightState.Flying)
            {
                if (tile.gameObject.transform.position.y < lowerLimit)
                {
                    TileToTop(tile);
                    break;
                }
            }
            else if (state == FlightState.Falling)
            {
                if (tile.gameObject.transform.position.y > upperLimit)
                {
                    TileToBottom(tile);
                    break;
                }
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

    void TileToBottom(Tile tile)
    {
        float lowest = GetLowest();
        float height = tile.adapter.GetHeight();

        TileData data;
        if (history.Count > 0) { data = history.Pop(); }
        else
        {
            if (tile.adapter.Data.type == TileType.Space) { data = sky; }
            else { data = ground; }
        }

        tile.adapter.SetData(data);
        tile.gameObject.transform.position = new Vector3(0, lowest - height, 0);

        if (data.type == TileType.Ground) { state = FlightState.Landed; }
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

    float GetLowest()
    {
        float lowest = float.MaxValue;
        foreach (Tile tile in tiles) { if (tile.gameObject.transform.position.y < lowest) { lowest = tile.gameObject.transform.position.y; } }
        return lowest;
    }
}
