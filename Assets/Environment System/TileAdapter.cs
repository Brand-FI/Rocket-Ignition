using UnityEngine;

public class TileAdapter : MonoBehaviour
{
    Renderer render;
    public TileData Data { get; private set; }
    void Awake() => render = GetComponentInChildren<Renderer>();
    public void SetData(TileData data)
    {
        Data = data;
        Initialize();
    }
    void Initialize()
    {
        transform.name = Data.name;
        transform.localScale = new Vector3(20f, 20f, 1f);
        if (render != null) { render.material.color = Data.color; }
    }
    public float GetHeight() => transform.localScale.y;
}
