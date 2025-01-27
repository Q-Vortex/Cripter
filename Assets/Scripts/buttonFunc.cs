using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonFunc : MonoBehaviour
{
    public GameObject canvasObject;
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
                canvasObject.SetActive(true); // Показать Canvas
            }
        }
    }
}
