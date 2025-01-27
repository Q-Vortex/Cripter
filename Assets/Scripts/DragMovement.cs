using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DragMovement : MonoBehaviour
{
    public float power = 10f;
    public Rigidbody2D rb;

    public Vector2 minPower;
    public Vector2 maxPower;
    public Sprite FallingSprite;
    public Sprite jumpEffect;
    public GameObject TextureEffect;

    Camera cam;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    private Animator animator;
    private SpriteRenderer spriteRenderer1;
    private SpriteRenderer spriteRenderer2;
    private float timeOut = 1;

    float atemps = 2;

    private void Start()
    {
        cam = Camera.main;


        animator = GetComponent<Animator>();
        spriteRenderer1 = GetComponent<SpriteRenderer>();
        spriteRenderer2 = TextureEffect.AddComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision){
        ContactPoint2D contact = collision.contacts[0];  // ���������� ������ ����� ��������

        if (contact.point.y < transform.position.y)
        {
            Vector3 position = transform.position;
            if (TextureEffect != null) TextureEffect.transform.position = position;
        }
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
            }

            if (Input.GetMouseButtonUp(0))
            {
                endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                endPoint.z = 15;

                force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
                rb.AddForce(force * power, ForceMode2D.Impulse);

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
                    spriteRenderer2.sprite = jumpEffect;
                    StartCoroutine(delay(0.1f, spriteRenderer2));
                }
            }
        }

        Vector3 position = transform.position;
        if (position.y <= -12)
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
    IEnumerator delay(float time, SpriteRenderer spriteRender)
    {
        yield return new WaitForSeconds(time);
        spriteRender.sprite = null;
    }

    private IEnumerator ResetAttackParameter(string name, float time)
    {
        yield return new WaitForSeconds(time);

        animator.SetFloat(name, 0f);
        timeOut = 1;
    }
    
}

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
    public Sprite FallingSprite;
    public Sprite jumpEffect;
    public GameObject TextureEffect;

    public TrajectoryLine tl;

    Camera cam;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    private Animator animator;
    private SpriteRenderer spriteRenderer1;
    private SpriteRenderer spriteRenderer2;
    private float timeOut = 1;

    float atemps = 2;

    private void Start()
    {
        cam = Camera.main;
        tl = GetComponent<TrajectoryLine>();

        animator = GetComponent<Animator>();
        spriteRenderer1 = GetComponent<SpriteRenderer>();
        spriteRenderer2 = TextureEffect.AddComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision){
        ContactPoint2D contact = collision.contacts[0];  // ���������� ������ ����� ��������

        if (contact.point.y < transform.position.y)
        {
            Vector3 position = transform.position;
            if (TextureEffect != null) TextureEffect.transform.position = position;
        }
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
                    spriteRenderer2.sprite = jumpEffect;
                    StartCoroutine(delay(0.1f, spriteRenderer2));
                }
            }
        }

        Vector3 position = transform.position;
        if (position.y <= -12)
        {
            transform.position = new Vector3(0f, 0f, 0f);
        }

        if (rb.velocity.y < -0.1f)
        {
            spriteRenderer1.sprite = FallingSprite;
            animator.enabled = false;
        }
        else
            animator.enabled = true;

        
    }
    IEnumerator delay(float time, SpriteRenderer spriteRender)
    {
        yield return new WaitForSeconds(time);
        spriteRender.sprite = null;
    }

    private IEnumerator ResetAttackParameter(string name, float time)
    {
        yield return new WaitForSeconds(time);

        animator.SetFloat(name, 0f);
        timeOut = 1;
    }



}
