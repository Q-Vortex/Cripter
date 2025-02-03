using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragMovement : MonoBehaviour
{
    public float power = 10f;
    public Rigidbody2D rb;

    public Vector2 minPower;
    public Vector2 maxPower;
    public Sprite FallingSprite;
    public Sprite jumpEffect;
    public TrajectoryLine tl;

    private Camera cam;
    private Vector2 force;
    private Vector3 startPoint;
    private Vector3 endPoint;

    private Animator animator;
    private SpriteRenderer spriteRenderer1;
    private float timeOut = 1;

    float atemps = 2;

    private void Start()
    {
        cam = Camera.main;
        tl = GetComponent<TrajectoryLine>();

        animator = GetComponent<Animator>();
        spriteRenderer1 = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision){
        
        atemps = 2f;
    }

    private void Update()
    {
        if (timeOut != 0 && atemps != 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                startPoint.z = 15;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                startPoint.z = 15;

                tl.RenderLine(startPoint, currentPoint);
            }

            if (Input.GetMouseButtonUp(0))
            {
                endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                endPoint.z = 15;

                force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
                rb.AddForce(force * power, ForceMode2D.Impulse);
                tl.EndLine();

                //Animation
                if (atemps == 1)
                {
                    animator.SetFloat("AttackToMove", 1f);
                    atemps--;
                    StartCoroutine(ResetAttackParameter("AttackToMove", 0.4f));
                }
                else
                {
                    animator.SetFloat("jump", 1f);
                    atemps--;
                    StartCoroutine(ResetAttackParameter("jump", 0.4f));

                }
            }
        }

        Vector3 position = transform.position;
        if (position.y <= 5)
        {
            SceneManager.LoadSceneAsync(0);
        }

        if (rb.velocity.y < -0.1f)
        {
            spriteRenderer1.sprite = FallingSprite;
            animator.enabled = false;
        }
        else
            animator.enabled = true;
    }

    private IEnumerator ResetAttackParameter(string name, float time)
    {
        yield return new WaitForSeconds(time);

        animator.SetFloat(name, 0f);
        timeOut = 1;
    }
}
