using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class MapGenerator : MonoBehaviour
{
    public int chunkSizeX = 20, chunkSizeY = 10;
    public int gridChellSize;
    public RuleTile ruleTile;
    public Tilemap tilemap;
    public Camera mainCamera;
    public GameObject coinPrefab;

    public int expansionSteps = 5; 
    public int minDistance = 3;

    public enum Settings {None, Livitation, Realistic};
    public Settings GeneratorSettings;

    private int ChuncCnt = 1;
    private HashSet<Vector3Int> islandBlocks = new HashSet<Vector3Int>();

    private Vector3Int NearestBlock = new Vector3Int(0, 0, 0);
    private Vector3Int FarestBlock = new Vector3Int(0, 0, 0);
    
    void Start()
    {
        GenerateChunc();
    }

    void GenerateChunc()
    {
        
        islandBlocks.Clear();
        Vector3Int startPos = new Vector3Int((chunkSizeX * ChuncCnt) + Random.Range(0, 5), chunkSizeY / 2, 0);
        tilemap.SetTile(startPos, ruleTile);
        islandBlocks.Add(startPos);

        for (int step = 0; step < expansionSteps; step++)
        {
            List<Vector3Int> newBlocks = new List<Vector3Int>();

            foreach (var pos in islandBlocks)
            {
                Vector3Int[] directions = {
                    new Vector3Int(1, 0, 0),  // Вправо
                    new Vector3Int(-1, 0, 0), // Влево
                    new Vector3Int(0, 1, 0),  // Вверх
                    new Vector3Int(0, -1, 0)  // Вниз
                };

                foreach (var dir in directions)
                {
                    Vector3Int nextPos = pos + dir;

                    if (tilemap.GetTile(nextPos) == null)
                    {
                        if (nextPos.x > NearestBlock.x)
                        {
                            NearestBlock = nextPos;
                        }
                        else if (nextPos.y > FarestBlock.y)
                        {
                            FarestBlock = nextPos;
                        }
                    }
                    if (tilemap.GetTile(nextPos) == null && Random.Range(0, 4) != 1)
                    {
                        tilemap.SetTile(nextPos, ruleTile);
                        newBlocks.Add(nextPos);
                    }
                }
            }
            islandBlocks.UnionWith(newBlocks);
        }
        FillGaps();

        ChuncCnt++;
    }

    void FillGaps()
    {
        List<Vector3Int> gapsToFill = new List<Vector3Int>(); // Список для заполнения

        foreach (var pos in islandBlocks)
        {
            Vector3Int[] neighbors = {
                new Vector3Int(3, 0, 0), new Vector3Int(-3, 0, 0),
                new Vector3Int(0, 3, 0), new Vector3Int(0, -3, 0)
            };

            foreach (var dir in neighbors)
            {
                Vector3Int nextPos = pos + dir;

                // Если вокруг есть тайлы, но в центре пусто - заполняем
                if (tilemap.GetTile(nextPos) == null)
                {
                    int adjacentCount = 0;
                    foreach (var checkDir in neighbors)
                    {
                        if (tilemap.GetTile(nextPos + checkDir) != null)
                        {
                            adjacentCount++;
                        }
                    }

                    if (adjacentCount >= 3)
                    { // Если тайл окружён минимум с 3 сторон
                        gapsToFill.Add(nextPos);
                    }
                }
            }
        }
        for (int y = FarestBlock.y; y >= -10; y--)
        {
            for (int x = NearestBlock.x; x >= 0; x--)
            {
                Vector3Int NowPos = new Vector3Int(x, y, 0);
                Vector3Int TopPos = new Vector3Int(x, y + 1, 0);
                Vector3Int BottomPos = new Vector3Int(x, y - 1, 0);
                Vector3Int LeftPos = new Vector3Int(x - 1, y, 0);
                Vector3Int RightPos = new Vector3Int(x + 1, y, 0);

                if (tilemap.GetTile(NowPos) == null)
                {
                    if (tilemap.GetTile(TopPos) != null && tilemap.GetTile(BottomPos) != null)
                    {
                        tilemap.SetTile(NowPos, ruleTile);
                    }
                    if (tilemap.GetTile(LeftPos) != null && tilemap.GetTile(RightPos) != null && tilemap.GetTile(TopPos) != null ||
                        tilemap.GetTile(LeftPos) != null && tilemap.GetTile(RightPos) != null && tilemap.GetTile(BottomPos) != null)
                    {
                        tilemap.SetTile(NowPos, ruleTile);
                    }

                }
                else if (tilemap.GetTile(NowPos) != null)
                {
                    if (tilemap.GetTile(LeftPos) == null && tilemap.GetTile(RightPos) == null && tilemap.GetTile(TopPos) == null)
                    {
                        tilemap.SetTile(NowPos, null);
                    }
                    else if (tilemap.GetTile(LeftPos) == null && tilemap.GetTile(RightPos) == null &&
                        tilemap.GetTile(TopPos) == null && tilemap.GetTile(BottomPos) == null)
                    {
                        tilemap.SetTile(NowPos, null);
                    }
                }
            }
        }
        foreach (var gap in gapsToFill)
        {
            tilemap.SetTile(gap, ruleTile);
            islandBlocks.Add(gap);
        }
    }

    void SpawnCoins()
    {
        List<Vector3Int> groundTiles = new List<Vector3Int>();

        // Ищем все тайлы, на которых можно разместить монеты
        foreach (var pos in islandBlocks)
        {
            Vector3Int abovePos = pos + new Vector3Int(0, 3, 0); // Проверяем клетку выше
            if (tilemap.GetTile(abovePos) == null)
            {
                groundTiles.Add(pos);
            }
        }

        // Размещаем монеты на случайных тайлах
        foreach (var tile in groundTiles)
        {
            if (Random.Range(0, 3) == 0)
            { // 33% шанс появления монеты
                Vector3 worldPos = tilemap.CellToWorld(tile) + new Vector3(0, 1, 0);
                Instantiate(coinPrefab, worldPos, Quaternion.identity);
            }
        }
    }


    void DestroyChunc()
    {
        for (int x = 0; x < (int)(mainCamera.transform.position.x / gridChellSize - 20); x++)
        {
            for (int y = -10; y < chunkSizeY; y++)
            {
                tilemap.SetTile(new Vector3Int(x, -y, 0), null);
            }
            
        }
    }
    void Update()
    {
        if (mainCamera.transform.position.x > NearestBlock.x * gridChellSize - 50)
        {
            GenerateChunc();
            SpawnCoins();
            DestroyChunc();
        }
    }
}