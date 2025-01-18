using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    public Camera mainCamera;
    
    public int chunkWidth = 20;
    public int chunkHeight = 10;
    public float chunkSpacing = 2f;
    public float noiseScale = 0.1f;

    public Tilemap tilemap;
    public Tile waterTile; 
    public Tile groundTile;
    public Tile highGroundTile;

    private List<GameObject> activeChunks = new List<GameObject>();
    private Vector3 nextChunkPosition;

    void Start()
    {
        float leftBound = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).x;
        nextChunkPosition = new Vector3(Mathf.Round(leftBound), 0, 0);

        GenerateChunk(nextChunkPosition);
    }
    void Update()
    {
        float leftBound = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).x;
        float rightBound = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane)).x;
        

        if (activeChunks.Count > 0 && activeChunks[0].transform.position.x + chunkWidth < Mathf.Round(leftBound))
        {
            ClearChunkTiles(activeChunks[0].transform.position);
            Destroy(activeChunks[0]);
            activeChunks.RemoveAt(0);
        }

        if (nextChunkPosition.x < rightBound + chunkWidth)
        {
            GenerateChunk(nextChunkPosition);
        }
    }

    void GenerateChunk(Vector3 position)
    {
        position.x = Mathf.Floor(position.x);

        GameObject chunk = new GameObject("Chunk");
        chunk.transform.position = position;

        GenerateTerrain(chunk.transform);

        activeChunks.Add(chunk);

        nextChunkPosition = new Vector3(Mathf.Round(position.x + chunkWidth + chunkSpacing), position.y, position.z);
    }
    void GenerateTerrain(Transform chunkTransform)
    {
        for (int x = 0; x < chunkWidth; x += 3)
        {
            Tile underTile = null;
            for (int y = 0  ; y < chunkHeight * 3; y += 3)
            {
                
                if (y == 0)
                {
                    float perlinValue = Mathf.PerlinNoise((x + chunkTransform.position.x) * noiseScale,
                                                      (y + chunkTransform.position.y) * noiseScale);

                    Tile tileToPlace = perlinValue > 0.4f ? highGroundTile : waterTile;
                    underTile = tileToPlace;

                    Vector3Int tilePosition = new Vector3Int(
                        Mathf.FloorToInt(x + chunkTransform.position.x),
                        Mathf.FloorToInt(y + chunkTransform.position.y),
                        0
                    );

                    tilemap.SetTile(tilePosition, tileToPlace);
                }
                else
                {
                    if (!(underTile == null))
                    {
                        if (y == chunkHeight * 2)
                        {
                            underTile = (underTile == waterTile) ? waterTile : groundTile;
                        }

                        Vector3Int tilePosition = new Vector3Int(
                            Mathf.FloorToInt(x + chunkTransform.position.x),
                            Mathf.FloorToInt(y + chunkTransform.position.y),
                            0
                        );

                        tilemap.SetTile(tilePosition, underTile);
                    } else { Debug.LogError("Tile generation error...");  }
                    
                }
            }
        }


    }

    void ClearChunkTiles(Vector3 chunkPosition)
    {
        for (int x = 0; x < chunkWidth; x++)
        {
            for (int y = 0; y < chunkHeight * 3; y++)
            {
                Vector3Int tilePosition = new Vector3Int(
                    Mathf.FloorToInt(x + chunkPosition.x),
                    Mathf.FloorToInt(y + chunkPosition.y),
                    0
                );

                tilemap.SetTile(tilePosition, null);
            }
        }
    }
}
