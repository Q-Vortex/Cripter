using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Camera mainCamera;
    public int blockSize;
    public int blockCnt;
    public int yBlockCnt;
    public Tilemap tmap;

    public Tile grass_tile;
    public Tile dirt_tile;
    public Tile grass_corner_tile;
    public Tile corner_tile;
    public Tile grass_corner_tile_merrored;
    public Tile corner_tile_merrored;
    public Tile grass_column_tile;
    public Tile column_tile;
    public Tile water_tile;

    private float realChuncSize;
    private int chuncCnt = 0;
    private int verticalPosition = 0;
    void Start()
    {
        realChuncSize = blockSize * blockCnt;
        createHorisontalChunc();
        //createVerticalChunc();
        generateFirstChunc();
    }

    void generateFirstChunc()
    {
        for (int i = -6; i < 0; i++)
        {
            for (int j = 1; j < yBlockCnt; j++)
            {
                tmap.SetTile(new Vector3Int(i * blockSize, -j * blockSize, 0), dirt_tile);
            }
            tmap.SetTile(new Vector3Int(i * blockSize, 0, 0), grass_tile);
        }
       
    }

    void createHorisontalChunc()
    {
        for (int i = 0; i < blockCnt; i++)
        {
            if (Random.Range(0, 5) != 1)
            {
                for (int j = 1; j < yBlockCnt; j++)
                {
                    tmap.SetTile(new Vector3Int((i * blockSize) + (int)(realChuncSize * chuncCnt), (-j + verticalPosition) * blockSize, 0), dirt_tile);
                }
                tmap.SetTile(new Vector3Int((i * blockSize) + (int)(realChuncSize * chuncCnt), verticalPosition * blockSize, 0), grass_tile);

            }
            else
            {
                tmap.SetTile(new Vector3Int((i * blockSize) + (int)(realChuncSize * chuncCnt), verticalPosition * blockSize , 0), water_tile);
            }
        }

        for (int i = 0; i < blockCnt; i++)
        {
            Vector3Int currentTilePosition = GetTilePosition(i);
            Vector3Int previousTilePosition = GetTilePosition(i - 1);
            Vector3Int nextTilePosition = GetTilePosition(i + 1);

            if (tmap.GetTile(currentTilePosition) == grass_tile)
            {
                if (tmap.GetTile(previousTilePosition) == water_tile)
                {
                    SetCornerTiles(currentTilePosition, grass_corner_tile, corner_tile);
                }
                else if (tmap.GetTile(nextTilePosition) == water_tile)
                {
                    SetCornerTiles(currentTilePosition, grass_corner_tile_merrored, corner_tile_merrored);
                }
                if (tmap.GetTile(previousTilePosition) == water_tile 
                    && tmap.GetTile(nextTilePosition) == water_tile) {
                    SetCornerTiles(currentTilePosition, grass_column_tile, column_tile);
                }
            }
        }
        chuncCnt++;
    }
    
    void createVerticalChunc()
    {
        int x;
        x = chuncCnt * (int)realChuncSize;
        
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                tmap.SetTile(new Vector3Int(x + 3, i * blockSize * 6 + j * blockSize, 0), dirt_tile);
            }

            if (Random.Range(0, 2) == 1)
            {
                x += Random.Range(2, 10);
            }
            else
            {
                x -= Random.Range(2, 10);
            }
            verticalPosition++;
        }
        chuncCnt++;
    }

    Vector3Int GetTilePosition(int index)
    {
        return new Vector3Int((index * blockSize) + (int)(realChuncSize * chuncCnt), 0, 0);
    }
    void SetCornerTiles(Vector3Int basePosition, TileBase topTile, TileBase sideTile)
    {
        tmap.SetTile(basePosition, topTile);
        for (int j = 1; j < yBlockCnt; j++)
        {
            Vector3Int sideTilePosition = new Vector3Int(basePosition.x, basePosition.y - j * blockSize, basePosition.z);
            tmap.SetTile(sideTilePosition, sideTile);
        }
    }

    void DeleteChunc()
    {
        for (int i = (int)(mainCamera.transform.position.x - realChuncSize * 2); i < (int)mainCamera.transform.position.x - 12; i++)
        {
            for (int j = 0; j < yBlockCnt; j++)
                tmap.SetTile(new Vector3Int(i, -j * blockSize, 0), null);
        }
    }

    void Update()
    {
        if (mainCamera.transform.position.x > (realChuncSize * chuncCnt) - 12)
        {
            if (Random.Range(0, 4) == 1)
            {  
                createVerticalChunc();
            } else
            {
                createHorisontalChunc();
            }
            DeleteChunc();
        }

        // Debug.Log($"cam pos = {mainCamera.transform.position.x}, ch & cyi {(realChuncSize * chuncCnt) - 12}");
    }
}