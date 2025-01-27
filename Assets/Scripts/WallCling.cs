using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMoovement : MonoBehaviour
{
    public float slideSpeed = 0.5f; // Скорость спуска по стене
    //public float wallCheckDistance = 0.1f; // Расстояние для проверки стены
    private Rigidbody2D rb;
    private bool isClinging = false;
    private float defaultGravityScale;
    private int CollisiomCNT = 0;

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
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
            rb.gravityScale = 0.1f;
            CollisiomCNT++;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            CollisiomCNT--;

            if (CollisiomCNT == 0)
            {
                isClinging = false;
                rb.gravityScale = defaultGravityScale;
            }
        }
    }
    void Update()
    {
        if (isClinging && rb.velocity.y < -slideSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }
    }
}
