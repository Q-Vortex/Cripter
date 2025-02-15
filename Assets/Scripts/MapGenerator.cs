using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public int chunkSizeX = 20, chunkSizeY = 10;
    public int gridChellSize;
    public RuleTile ruleTile;
    public Tilemap BlockTilemap;
    public Tilemap EntityTilemap;
    public Camera mainCamera;
    public AnimatedTile GoldenCoinTile;
    public AnimatedTile SilverCoinTile;
    public int expansionSteps = 5; 
    public int minDistance = 3;

    public enum Settings {None, Livitation, Realistic};
    public Settings GeneratorSettings;

    private int ChuncCnt = 1;
    private HashSet<Vector3Int> islandBlocks = new HashSet<Vector3Int>();
    private List<Vector3Int> surfaceBlocks = new List<Vector3Int>();

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
        BlockTilemap.SetTile(startPos, ruleTile);
        islandBlocks.Add(startPos);

        for (int step = 0; step < expansionSteps; step++)
        {
            List<Vector3Int> newBlocks = new List<Vector3Int>();

            foreach (var pos in islandBlocks)
            {
                Vector3Int[] directions = {
                    new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0),
                    new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0)
                };

                foreach (var dir in directions)
                {
                    Vector3Int nextPos = pos + dir;

                    if (BlockTilemap.GetTile(nextPos) == null)
                    {
                        if (nextPos.x > NearestBlock.x)
                        {
                            NearestBlock = nextPos;
                        }
                        if (nextPos.y > FarestBlock.y)
                        {
                            FarestBlock = nextPos;
                        }
                    }
                    if (BlockTilemap.GetTile(nextPos) == null && Random.Range(0, 4) != 1)
                    {
                        BlockTilemap.SetTile(nextPos, ruleTile);
                        newBlocks.Add(nextPos);
                    }
                }
            }
            islandBlocks.UnionWith(newBlocks);
        }
        FillGaps();
        ChuncCnt++;

        foreach (var pos in islandBlocks)
        {
            Vector3Int above = new Vector3Int(pos.x, pos.y + 1, pos.z);
            if (BlockTilemap.GetTile(above) == null)
            {
                surfaceBlocks.Add(pos);
            }
        }
    }

    void FillGaps()
    {
        List<Vector3Int> gapsToFill = new List<Vector3Int>();

        foreach (var pos in islandBlocks)
        {
            Vector3Int[] neighbors = {
                new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0),
                new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0)
            };

            foreach (var dir in neighbors)
            {
                Vector3Int nextPos = pos + dir;

                if (BlockTilemap.GetTile(nextPos) == null)
                {
                    int adjacentCount = 0;
                    foreach (var checkDir in neighbors)
                    {
                        if (BlockTilemap.GetTile(nextPos + checkDir) != null)
                        {
                            adjacentCount++;
                        }
                    }

                    if (adjacentCount >= 3)
                    {
                        gapsToFill.Add(nextPos);
                    }
                }
            }
        }
        
        foreach (var gap in gapsToFill)
        {
            BlockTilemap.SetTile(gap, ruleTile);
            islandBlocks.Add(gap);
        }
    }

    void CreateCoin()
    {
        foreach (var pos in islandBlocks)
        {
            Vector3Int TopPos = new Vector3Int(pos.x, pos.y + 1, pos.z);
            Vector3Int BottomPos = new Vector3Int(pos.x, pos.y - 1, pos.z);

            if (BlockTilemap.GetTile(TopPos) == null && BlockTilemap.GetTile(BottomPos) != null)
            {
                if (Random.Range(0, 8) != 1 && Random.Range(0, 8) >= 4)
                {
                    EntityTilemap.SetTile(TopPos, SilverCoinTile);
                }
                else if(Random.Range(0, 8) >= 4 && Random.Range(0, 8) != 1)
                {
                    EntityTilemap.SetTile(TopPos, GoldenCoinTile);
                } else
                {
                    EntityTilemap.SetTile(TopPos, null);
                }
               
            }
        }
    }

    void DestroyChunc()
    {
        for (int x = 0; x < (int)(mainCamera.transform.position.x / gridChellSize - 20); x++)
        {
            for (int y = -10; y < chunkSizeY; y++)
            {
                BlockTilemap.SetTile(new Vector3Int(x, -y, 0), null);
                EntityTilemap.SetTile(new Vector3Int(x, -y, 0), null);
                EntityTilemap.SetTile(new Vector3Int(x, -y + 1, 0), null);
            }

        }
    }

    void Update()
    {
        if (mainCamera.transform.position.x > NearestBlock.x * gridChellSize - 50)
        {
            GenerateChunc();
            CreateCoin();
            DestroyChunc();
        }
    }
}