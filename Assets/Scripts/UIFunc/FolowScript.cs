using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Collections;
using UnityEngine;

public class FolowScript : MonoBehaviour
{
    public Camera MainCamera;
    private Collider2D collider;
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }
    void Update()
    {
        Vector3 MainCameraPos = MainCamera.transform.position;
        Vector3 targetPos = new Vector3(MainCameraPos.x, 5, 0);

        if (MainCamera.transform.position.x > transform.position.x + collider.bounds.size.x - 50)
        {
            transform.position = targetPos;
        }
    }
}
