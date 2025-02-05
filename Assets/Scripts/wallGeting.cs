using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class wallGeting : MonoBehaviour
{
    public Rigidbody2D rb;
    public float slideSpeed = 2f;

    private bool isSliding;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if ((CheckBlocksAround("+x") && rb.velocity.y < 0) ||
            (CheckBlocksAround("-x") && rb.velocity.y < 0))
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }
    }
    void FixedUpdate()
    {
        if (isSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }
    }

    bool CheckBlocksAround(string tipeGet)
    {
        float rayDistance = 0.9f;
        LayerMask mask = LayerMask.GetMask("Block");

        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, rayDistance, mask);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, mask);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, rayDistance, mask);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, rayDistance, mask);

        if (hitUp.collider != null && tipeGet == "+y") return true;
        if (hitDown.collider != null && tipeGet == "-y") return true;
        if (hitRight.collider != null && tipeGet == "+x") return true;
        if (hitLeft.collider != null && tipeGet == "-x") return true;
        return false;
    }
}
