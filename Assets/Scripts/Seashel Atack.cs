using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject cannonBallPrefab; // ������ ����
    public Transform firePoint; // ����� ��������

    public void Shoot() // ���� ����� ������� �� ��������
    {
        if (cannonBallPrefab == null)
        {
            Debug.LogError("������: ������ ���� �� ��������!");
            return;
        }
        Debug.Log("�������!"); // ���������, ���������� �� �����
        GameObject cannonBall = Instantiate(cannonBallPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = cannonBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // ��������� ��������� ��������
            rb.velocity = -firePoint.right * 10f; // 10f - �������� ����, ����� ���������
        }
    }
}
