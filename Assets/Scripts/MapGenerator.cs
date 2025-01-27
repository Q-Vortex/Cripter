using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public int chunkSizeX, chunkSizeY;
    public int gridChellSize;
    public RuleTile ruleTile;
    public Tilemap tilemap;
    public Camera mainCamera;
    public enum Settings {None, Livitation, Realistic};
    public Settings GeneratorSettings;
    private int ChuncCnt = 1;
    private Vector3Int NearestBlock = new Vector3Int(0, 0, 0);
    private Vector3Int FarestBlock = new Vector3Int(0, 0, 0);
    void Start()
    {
        GenerateChunc();
    }

    void GenerateChunc()
    {
        List<Vector3Int> BlockPosition = new List<Vector3Int>();

        Vector3Int StartPos = new Vector3Int((chunkSizeX * ChuncCnt) / 2, chunkSizeY / 2, 0);
        tilemap.SetTile(StartPos, ruleTile);
        BlockPosition.Add(StartPos);

        for (int t = 0; t < 5; t++)
        {
            for (int i = 0; i < BlockPosition.Count; i++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (Random.Range(0, 4) != 1)
                        {
                            Vector3Int NextPos = new Vector3Int(BlockPosition[i].x + x, BlockPosition[i].y + y, 0);
                            if (tilemap.GetTile(NextPos) == null)
                            {
                                if (NextPos.x > NearestBlock.x)
                                {
                                    NearestBlock = NextPos;
                                }
                                else if (NextPos.y > FarestBlock.y)
                                {
                                    FarestBlock = NextPos;
                                    //Debug.Log(NextPos.y);
                                }
                            }
                            tilemap.SetTile(NextPos, ruleTile);
                            BlockPosition.Add(NextPos);
                            BlockPosition.Remove(BlockPosition[i]);
                        }
                    }
                }
            }
        }
        if (GeneratorSettings == Settings.Realistic)
        {
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
        } 
        else if (GeneratorSettings == Settings.Livitation)
        {
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
                    }
                }
            }
        }
        ChuncCnt++;
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
            DestroyChunc();
        }
    }
}