using JetBrains.Annotations;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditorInternal;
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
    public Tile water_tile;
    public Tile grass_corner_tile;
    public Tile grass_corner_tile_merrored;
    public Tile grass_column_tile;
    public Tile corner_tile;
    public Tile corner_tile_merrored;
    public Tile column_tile;
    
    private float realChuncSize;
    private int chuncCnt = 0;
    private Vector3Int lastPos = new Vector3Int(0, 0, 0);
    private Vector3Int lastVerticalControlPos;
    void Start()
    {
        realChuncSize = blockSize * blockCnt;
        createVerticalChunc();
        createHorisontalChunc();
        
        generateFirstChunc();
    }

    void generateFirstChunc()
    {
        for (int i = -3; i < 0; i++)
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
            int xPos = (i * blockSize) + (int)(realChuncSize * chuncCnt);
            int yPos = 0;
            if (Random.Range(0, 5) != 1)
            {
                for (int j = 1; j < yBlockCnt; j++)
                {
                    yPos = -j * blockSize + lastPos.y;
                    tmap.SetTile(new Vector3Int(xPos, yPos, 0), dirt_tile);
                }
                yPos = 0 + lastPos.y;
                tmap.SetTile(new Vector3Int(xPos, yPos, 0), grass_tile);
            }
            else
            {
                yPos = 0 + lastPos.y;
                tmap.SetTile(new Vector3Int(xPos, yPos, 0), water_tile);
            }
        }


        //for (int i = 0  ; i < blockCnt; i++)
        //{
        //    Vector3Int currentTilePosition = GetTilePosition(i);
        //    Vector3Int previousTilePosition = GetTilePosition(i - 1);
        //    Vector3Int nextTilePosition = GetTilePosition(i + 1);

        //    if (tmap.GetTile(currentTilePosition) == grass_tile)
        //    {
        //        if (tmap.GetTile(previousTilePosition) == water_tile)
        //        {
        //            SetCornerTiles(currentTilePosition, grass_corner_tile, corner_tile);
        //        }
        //        else if (tmap.GetTile(nextTilePosition) == water_tile)
        //        {
        //            SetCornerTiles(currentTilePosition, grass_corner_tile_merrored, corner_tile_merrored);
        //        }
        //        if (tmap.GetTile(previousTilePosition) == water_tile 
        //            && tmap.GetTile(nextTilePosition) == water_tile) {
        //            SetCornerTiles(currentTilePosition, grass_column_tile, column_tile);
        //        }
        //    }
        //}
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

    void createVerticalChunc()
    {
        // Getting the main variables.
        int x = 0;
        int i = lastPos.y / blockSize;
        char pos = 'L';
        int chunkSize = 20;
        int chunkWidth = 10 * blockSize;
        // Start the main cicle.
        for (int y = 1; y < (chunkSize * blockSize); y += 0) 
        {
            // Creating platforms with height = 3 blocks of dirt.
            for (int s = 0; s < Random.Range(3, 6); s++)
            {
                int xPos = (int)(realChuncSize * chuncCnt) + x * blockSize;
                int yPos = (y + i) * blockSize + s * blockSize;
                tmap.SetTile(new Vector3Int(xPos, yPos , 0), dirt_tile);
            }
            y += 3;
            
            // Making a little space between platforms.

            // Getting position deferention.
            if (pos == 'R') pos = 'L'; 
                else pos = 'R';

            // Getting x direction based on position diferention.
            int rndXpos = Random.Range(3, 6);
            if (pos == 'R') x -= rndXpos; 
                else x += rndXpos;

            //Saving vertica chunc size.
            lastPos = new Vector3Int(x * blockSize, (y + i) * blockSize, 0);
            lastVerticalControlPos = lastPos;
        }
    }

    void DeleteChunc()
    {
        for (int i = (int)(mainCamera.transform.position.x - realChuncSize * 2); i < (int)mainCamera.transform.position.x - 12; i++)
        {
            for (int j = 0; j < yBlockCnt; j++)
                tmap.SetTile(new Vector3Int(i + lastPos.x * blockSize, -j * blockSize + lastPos.y * blockSize, 0), null);
        }
    }

    void Update()
    {
        Debug.Log($"Xpos={lastPos.x}, Ypos={lastPos.y}");
        if (mainCamera.transform.position.x > (realChuncSize * chuncCnt) - 12)
        {
            if (Random.Range(0, 5) == 1)
            {
                createVerticalChunc();
            }
            else
            {
                lastVerticalControlPos.y += 100;
                createHorisontalChunc();
            }
            DeleteChunc();
            
        }
    }
}