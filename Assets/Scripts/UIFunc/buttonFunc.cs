using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonFunc : MonoBehaviour
{
    public GameObject canvasObject;
    public List<Animator> animator;
    public List<GameObject> GameObject;

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void ExitGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ContinueGame()
    {
        canvasObject.SetActive(false);
    }

    public void OpenSettins()
    {
        GameObject[0].SetActive(true);
        GameObject[1].SetActive(true);
        animator[0].Play("settingsBoard", -1, 0f);
        animator[1].Play("HeaderSettiingsBoard", -1, 0f);
        GameObject[2].SetActive(false);
        GameObject[3].SetActive(false);
    }

    public void CloseSettins()
    {
        GameObject[0].SetActive(false);
        GameObject[1].SetActive(false);
        GameObject[2].SetActive(true);
        GameObject[3].SetActive(true);
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvasObject.activeSelf)
            {
                canvasObject.SetActive(false);
            }
            else
            {
                canvasObject.SetActive(true);
            }
        }
    }
}
