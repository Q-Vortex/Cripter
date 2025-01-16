using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraBoundTilemap : MonoBehaviour
{
    public Tilemap tilemap; 
    public TileBase debugTile; 
    public Camera mainCamera;
    public TileBase waterTile;

    void Start()
    {
        GenerateDebugTiles();
    }

    void GenerateDebugTiles()
    {
        Vector3 minScreenPoint = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        minScreenPoint += mainCamera.transform.position;
        Vector3 maxScreenPoint = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        maxScreenPoint += mainCamera.transform.position;

        int minX = Mathf.FloorToInt(minScreenPoint.x);
        int maxX = Mathf.CeilToInt(maxScreenPoint.x); 
        int minY = 1;
        int maxY = Mathf.CeilToInt(maxScreenPoint.y);

        for (int x = minX; x < maxX; x += 3)
        {
            int randomValue = Random.Range(0, 3);
            if (randomValue != 1) tilemap.SetTile(new Vector3Int(x, maxY / 2, 0), debugTile);
            else { tilemap.SetTile(new Vector3Int(x, maxY / 2, 0), waterTile); }
            
        }
    }

    
}
