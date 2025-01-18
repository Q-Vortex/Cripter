using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DragMovement : MonoBehaviour
{
    public float power = 10f;
    public Rigidbody2D rb;

    public Vector2 minPower;
    public Vector2 maxPower;

    Camera cam;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    private Animator animator;
    private float timeOut = 1;
    float atemps = 2;
    private void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision){ atemps = 2f; }

    private void Update()
    {
        if (timeOut != 0 && atemps != 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                startPoint.z = 15;
                // Debug.Log(startPoint);
            }

            if (Input.GetMouseButtonUp(0))
            {
                endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                endPoint.z = 15;

                force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
                rb.AddForce(force * power, ForceMode2D.Impulse);

                

                if (atemps == 1)
                {
                    animator.SetFloat("AttackToMove", 1f);
                    atemps--;
                    StartCoroutine(ResetAttackParameter("AttackToMove", 0.4f));
                } else
                {
                    animator.SetFloat("jump", 1f);
                    atemps--;
                    StartCoroutine(ResetAttackParameter("jump", 0.4f));
                }
            }
                
            
        }
    }

    private IEnumerator ResetAttackParameter(string name, float time)
    {
        yield return new WaitForSeconds(time);

        animator.SetFloat(name, 0f);
        timeOut = 1;
    }
    
}
