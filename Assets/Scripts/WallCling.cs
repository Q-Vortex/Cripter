using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovement : MonoBehaviour
{
    public float slideSpeed = 2f; // Скорость спуска по стене
    private Rigidbody2D rb;
    private bool isClinging = false;
    private float defaultGravityScale;
    //private bool isAtBottom = false; // Проверка на достижение нижней границы

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
            rb.gravityScale = 0; // Отключаем гравитацию при прилипания к стене
            //isAtBottom = false; // Сброс флага при касании стены
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isClinging = false;
            rb.gravityScale = defaultGravityScale; // Восстанавливаем гравитацию
            //isAtBottom = false; // Сброс флага при выходе из столкновения
        }
    }

    /*
    void Update()
    {
        if (isClinging && !isAtBottom)
        {
            // Если персонаж цепляется за стену, он автоматически спускается
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed); // Спуск вниз с ограниченной скоростью

            // Проверка, достиг ли персонаж нижней границы
            if (transform.position.y <= GetBottomPosition()) // Предполагаем, что это точка нижней границы
            {
                isAtBottom = true; // Останавливаем спуск, когда достигли нижней границы
                rb.velocity = Vector2.zero; // Останавливаем движение
            }
        }
    }
    */

    // Функция для получения позиции нижней границы стены (можно уточнить по вашему сценарию)
    private float GetBottomPosition()
    {
        // Здесь мы просто предполагаем, что нижняя граница будет на какой-то фиксированной позиции
        // Это можно заменить на более сложную логику, если нужна динамическая проверка
        return -5f; // Например, нижняя граница на Y = -5
    }
}
