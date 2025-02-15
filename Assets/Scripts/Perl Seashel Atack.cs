using System.Collections;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float maxDistance = 15f;   // ������������ ��������� ������
    public LayerMask blockLayer;      // ���� ������
    public int damage = 1;

    private Vector2 startPosition;    // ��������� ������� ����

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // ��������� ���������
        if (Vector2.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject); // ���������� ���� ����� ����������� ���������
        }

        // Raycast �������� �� ������������
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 0.1f, blockLayer);
        if (hit.collider != null)
        {
            Destroy(gameObject); // ���������� ���� ��� ������������
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ���������, ������ �� ���� � ������
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // ������� ����
            }

            Destroy(gameObject); // ���������� ���� ����� ��������� � ������
        }
    }
}
