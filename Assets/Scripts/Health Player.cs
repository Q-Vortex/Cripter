using UnityEngine;
using UnityEngine.SceneManagement; // ��� ����� ����

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3; // ������������ ���������� ������
    private int currentHealth; // ������� ��������

    void Start()
    {
        currentHealth = maxHealth; // ������������� ��������� ��������
    }

    // ����� ��� ��������� �����
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("����� ������� ����! �������� ������: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ����� ��� ��������� ������ ���������
    void Die()
    {
        Debug.Log("����� �����! ��������� ����� ������...");
        SceneManager.LoadScene(0); // ��������� ����� ������
    }
}
