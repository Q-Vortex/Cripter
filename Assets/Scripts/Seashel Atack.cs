using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject cannonBallPrefab; // Префаб ядра
    public Transform firePoint; // Точка выстрела

    public void Shoot() // Этот метод вызовем из анимации
    {
        if (cannonBallPrefab == null)
        {
            Debug.LogError("Ошибка: префаб ядра не назначен!");
            return;
        }
        Debug.Log("Выстрел!"); // Проверяем, вызывается ли метод
        GameObject cannonBall = Instantiate(cannonBallPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = cannonBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Применяем начальную скорость
            rb.velocity = -firePoint.right * 10f; // 10f - скорость ядра, можно настроить
        }
    }
}
