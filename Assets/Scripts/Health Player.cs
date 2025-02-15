using UnityEngine;
using UnityEngine.SceneManagement; // Для смены сцен

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3; // Максимальное количество жизней
    private int currentHealth; // Текущее здоровье

    void Start()
    {
        currentHealth = maxHealth; // Устанавливаем начальное здоровье
    }

    // Метод для получения урона
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Игрок получил урон! Осталось жизней: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Метод для обработки смерти персонажа
    void Die()
    {
        Debug.Log("Игрок погиб! Загружаем экран смерти...");
        SceneManager.LoadScene(0); // Загружаем сцену смерти
    }
}
