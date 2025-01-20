using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovement : MonoBehaviour
{
    public float slideSpeed = 2f; // Скорость спуска по стене
    private Rigidbody2D rb;
    private bool isClinging = false;
    private float defaultGravityScale;
    private bool isAtBottom = false; // Проверка на достижение нижней границы

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb.gravityScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isClinging = true;
            rb.gravityScale = 0;
            isAtBottom = false; 
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isClinging = false;
            rb.gravityScale = defaultGravityScale;
            isAtBottom = false; 
        }
    }

    void Update()
    {
        if (isClinging && !isAtBottom)
        {
            
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);

            
            if (transform.position.y <= -5f)
            {
                isAtBottom = true;
                rb.velocity = Vector2.zero;
            }
        }
    }
}
