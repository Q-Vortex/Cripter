using System.Collections;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float maxDistance = 15f;   // Максимальная дистанция полета
    public LayerMask blockLayer;      // Слой блоков
    public int damage = 1;

    private Vector2 startPosition;    // Начальная позиция ядра

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Проверяем дистанцию
        if (Vector2.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject); // Уничтожить ядро после преодоления дистанции
        }

        // Raycast проверка на столкновение
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 0.1f, blockLayer);
        if (hit.collider != null)
        {
            Destroy(gameObject); // Уничтожить ядро при столкновении
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Проверяем, попало ли ядро в игрока
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Наносим урон
            }

            Destroy(gameObject); // Уничтожаем ядро после попадания в игрока
        }
    }
}
