using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour, IUpdatable
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase groundTile;
    [SerializeField] private GameObject[] buildingPrefabs;
    [SerializeField] private int chunkSize = 16;
    [SerializeField] private int loadRadius = 2;
    [SerializeField, Range(0f, 1f)] private float buildingChance = 0.5f;
    [SerializeField] private float startClearRadius = 6f;

    private const float EdgeMargin = 2f;
    private readonly HashSet<Vector2Int> generated = new();
    private Transform player;
    private Vector2Int currentChunk = new(int.MinValue, int.MinValue);

    void Start()
    {
        GameplayManager.Instance.Register(this);
    }

    public void OnUpdate(float deltaTime)
    {
        player = GameplayManager.Instance.player.currentHero.transform;

        Vector3Int cell = tilemap.WorldToCell(player.position);
        Vector2Int chunk = new Vector2Int
        (
            Mathf.FloorToInt((float)cell.x / chunkSize),
            Mathf.FloorToInt((float)cell.y / chunkSize)
        );

        if (chunk == currentChunk) return; 
        currentChunk = chunk;

        for (int x = -loadRadius; x <= loadRadius; x++)
        {
            for (int y = -loadRadius; y <= loadRadius; y++)
            {
                GenerateChunk(chunk + new Vector2Int(x, y));
            }
        }
    }
    public void OnFixedUpdate(float fixedDeltaTime) { }
    private void GenerateChunk(Vector2Int chunk)
    {
        if (!generated.Add(chunk)) return; 

        Vector2Int origin = chunk * chunkSize;

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                tilemap.SetTile(new Vector3Int(origin.x + x, origin.y + y, 0), groundTile);
            }
        }

        TrySpawnBuilding(origin);
    }

    private void TrySpawnBuilding(Vector2Int origin)
    {
        if (buildingPrefabs.Length == 0 || Random.value > buildingChance) return;

        Vector3 chunkWorldMin = tilemap.CellToWorld(new Vector3Int(origin.x, origin.y, 0));
        float chunkWorldSize = chunkSize * tilemap.layoutGrid.cellSize.x;
        float margin = EdgeMargin * tilemap.layoutGrid.cellSize.x;

        Vector2 position = (Vector2)chunkWorldMin + new Vector2
        (
            Random.Range(margin, chunkWorldSize - margin),
            Random.Range(margin, chunkWorldSize - margin)
        );

        if (position.magnitude < startClearRadius) return;

        GameObject prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
        Instantiate(prefab, position, Quaternion.identity, transform);
    }
}