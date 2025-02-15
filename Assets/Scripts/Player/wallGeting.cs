using UnityEngine;

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
        if (CheckBlocksAround("+x") && rb.velocity.y < 0 && !CheckBlocksAround("-y", 3f))
        {
            isSliding = true;
            GetComponent<SpriteRenderer>().flipX = false; 
        }
        else if (CheckBlocksAround("-x") && rb.velocity.y < 0 && !CheckBlocksAround("-y", 3f))
        {
            isSliding = true;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            isSliding = false;
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (CheckBlocksAround("-y", 1.22f))
        {
            isSliding = false;
            GetComponent<SpriteRenderer>().flipX = false;

            Player player = FindObjectOfType<Player>();
            player.atemps = 2f;
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
        float rayDistance = 1f;
        LayerMask mask = LayerMask.GetMask("Block");

        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, rayDistance, mask);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, rayDistance, mask);

        if (hitRight.collider != null && tipeGet == "+x") return true;
        if (hitLeft.collider != null && tipeGet == "-x") return true;
        return false;
    }

    bool CheckBlocksAround(string tipeGet, float RayDistanceY)
    {
        float rayDistanceY = RayDistanceY;
        LayerMask mask = LayerMask.GetMask("Block");

        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, rayDistanceY, mask);

        if (hitDown.collider != null && tipeGet == "-y") return true;
        return false;
    }
}
