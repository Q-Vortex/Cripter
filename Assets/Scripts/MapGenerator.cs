using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Camera mainCamera;
    public int blockSize;
    public int blockCnt;
    public int yBlockCnt;
    public float RealBlockSize;
    public Tilemap tmap;
    public Tile grass_tile;
    public Tile dirt_tile;
    public Tile water_tile;

    private int chuncSize;
    private float realChuncSize;
    private int chuncCnt = 0;
    void Start()
    {
        chuncSize = blockCnt * blockSize;
        realChuncSize = RealBlockSize * blockCnt;
        createChucn();
    }

    void createChucn() {
        for (int i = 0; i < blockCnt; i++) {
            if (Random.Range(0, 3) != 1)
            {
                tmap.SetTile(new Vector3Int((i * blockSize) + (int)(realChuncSize * chuncCnt), 0, 0), grass_tile);
            }
            else
            {
                tmap.SetTile(new Vector3Int((i * blockSize) + (int)(realChuncSize * chuncCnt), 0, 0), water_tile);
            }
        }
        chuncCnt++;
    }
    
    void DeleteChunc()
    {
        for (int i = (int)(mainCamera.transform.position.x - realChuncSize * 2); i < (int)mainCamera.transform.position.x - 12; i++)
        {
            tmap.SetTile(new Vector3Int(i, 0, 0), null);
        }

    }

    void Update()
    {
        if (mainCamera.transform.position.x > (chuncSize * chuncCnt) - 12)
        {
            createChucn();
            DeleteChunc();
        }


        Debug.Log($"cam pos = {mainCamera.transform.position.x}, ch & cyi {(chuncSize * chuncCnt) - 12}");
    }
}
