using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EntityTMapFunc : MonoBehaviour
{
    private Tilemap tilemap;
    private Rigidbody2D rb;
    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3Int cellPosition = tilemap.WorldToCell(other.transform.position);
            tilemap.SetTile(cellPosition, null);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3Int cellPosition = tilemap.WorldToCell(other.transform.position);
            if (tilemap.GetTile()
            tilemap.SetTile(cellPosition, null);
        }
    }
}
